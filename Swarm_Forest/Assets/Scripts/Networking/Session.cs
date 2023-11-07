
using System;
using System.Net.Sockets;
using UnityEngine.Assertions;

namespace Domino.Networking.Event{
    public class ConnectivityEventArgs{
        public bool Connected{get; set;}
    }

    public class ReceivedEventArgs{
        public byte[] Buffer{get;set;}
    }

    public class SentEventArgs{
        public bool Sent{get;set;}
    }

    public delegate void ConnectivityEventHandler(object sender, ConnectivityEventArgs e);
    public delegate void ReceiveEventHandler(object sender, ReceivedEventArgs e);
    public delegate void SendEventHandler(object sender, SentEventArgs e);
}

namespace Domino.Networking.TCP
{
    public class Session{
        private class AsyncState{
            public Socket socket;
        }

        private class CommunicationState: AsyncState{
            public const int BUF_MAX = 256;
            public byte[] buffer = new byte[BUF_MAX];
            public int length;
        }

        public event Event.ConnectivityEventHandler ConnectedEvent;
        public event Event.ConnectivityEventHandler DisconnectedEvent;
        public event Event.ReceiveEventHandler ReceivedEvent;
        public event Event.SendEventHandler SentEvent;

        public Session(){
            DisconnectedEvent += (sender, e)=>{
                m_socket = null;
            };
        }

        private Socket m_socket;
        public int ID{get;set;}

        public bool Connected{
            get{
                if(m_socket is null) return false;
                return m_socket.Connected;
            }
        }

        public void Connect(string host, int port){
            m_socket ??= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            m_socket.BeginConnect(host, port, new AsyncCallback(ConnectCallback), new AsyncState{
                socket = m_socket
            });
        }

        public void Send(byte[] bytes){
            if(!Connected) return;

            var sendState = new CommunicationState();
            Array.Copy(bytes, sendState.buffer, bytes.Length);
            sendState.length = bytes.Length;

            m_socket.BeginSend(sendState.buffer, 0, sendState.length, SocketFlags.None, new AsyncCallback(SendCallback), sendState);
        }

        public void Disconnect(){
            if(Connected){
                m_socket.BeginDisconnect(false, new AsyncCallback(DisconnectCallback), new AsyncState{
                    socket = m_socket
                });
            }
        }

        private void ConnectCallback(IAsyncResult ar){
            var state = ar.AsyncState as AsyncState;
            Assert.IsNotNull(state);
            var socket = state.socket;

            socket.EndConnect(ar);

            ConnectedEvent?.Invoke(this, new Event.ConnectivityEventArgs{
                Connected = socket.Connected
            });

            if(!socket.Connected){
                Disconnect();
                return;
            }

            var recvState = new CommunicationState();
            socket.BeginReceive(recvState.buffer, 0, CommunicationState.BUF_MAX, SocketFlags.None, new AsyncCallback(ReceiveCallback), recvState);
        }

        private void ReceiveCallback(IAsyncResult ar){
            var state = ar.AsyncState as CommunicationState;
            Assert.IsNotNull(state);
            var socket = state.socket;

            var length = socket.EndReceive(ar, out var error);

            if(error != SocketError.Success){
                Disconnect();
                return;
            }

            state.length = length;

            var args = new Event.ReceivedEventArgs{
                Buffer = new byte[length]
            };
            Array.Copy(state.buffer, args.Buffer, length);

            ReceivedEvent?.Invoke(this, args);

            // Make loop via calling BeginReceive continuously
            socket.BeginReceive(state.buffer, 0, CommunicationState.BUF_MAX, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
        }

        private void SendCallback(IAsyncResult ar){
            var state = ar.AsyncState as CommunicationState;
            Assert.IsNotNull(state);
            var socket = state.socket;

            var length = socket.EndSend(ar, out var error);

            var sent = error == SocketError.Success && state.length == length;

            if(!sent){
                Disconnect();
            }

            SentEvent?.Invoke(this, new Event.SentEventArgs{
                Sent = sent
            });
        }

        private void DisconnectCallback(IAsyncResult ar){
            var state = ar.AsyncState as AsyncState;
            Assert.IsNotNull(state);
            var socket = state.socket;

            socket.EndDisconnect(ar);

            DisconnectedEvent?.Invoke(this, new Event.ConnectivityEventArgs{
                Connected = false
            });
        }

        internal void Send(MatchRegister matchRegister)
        {
            throw new NotImplementedException();
        }
    }
}
