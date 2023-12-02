
using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Domino.Networking.HTTP;
using UnityEditor.UI;
using UnityEngine.Assertions;
using Google.Protobuf;
using Google.Protobuf.GameProtocol;
using UnityEngine;

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
            public const int BUF_MAX = 512;
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

        public Session(int UserId, string SessionId){
            this.UserId = UserId;
            this.SessionId = SessionId;
        }

        ~Session(){
            if(UserId != -1) SignOut().Wait();
        }

        private Socket m_socket;
        public int UserId{get; private set;} = -1;
        public string SessionId{get; private set;}
        public int RoomId{get; private set;}

        public string MatchServerAddr{get;set;}
        public int MatchServerPort = 6789;
        public string GameServerAddr{get;set;}
        public int GameServerPort = 5678;

        public bool Connected{
            get{
                if(m_socket is null) return false;
                return m_socket.Connected;
            }
        }

        async Task SignIn(){

            HttpClient client = new HttpClient();

            AuthenticationRequest request = AuthenticationRequest.Factory.Create("111111", "111111");
            var option = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var httpResponse = await client.PostAsJsonAsync("http://localhost:5172/login", request, option);

            var response = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResponse>(option);
            
            UserId = response.userId;
            SessionId = response.sessionId;
            MatchServerAddr = response.matchServerAddress;
            GameServerAddr = response.gameServerAddress;
            UnityEngine.Debug.Log("SignIn");
        }

        public async Task SignOut(){
            HttpClient client = new HttpClient();
            var request = new {
                userId = UserId,
                sessionId = SessionId
            };
            var option = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var httpResponse = await client.PostAsJsonAsync("http://localhost:5172/logout", request, option);
            await httpResponse.Content.ReadFromJsonAsync<object>(option);
            UnityEngine.Debug.Log("SignOut");
        }

        public async void ConnectMatchmakingServer(){
            if(UserId == -1)
                await SignIn();
            else{
                LoadServerAddress();
            }
            Connect(MatchServerAddr, MatchServerPort);
        }

        public async void ConnectGameServer(){
            if(UserId == -1)
                await SignIn();
            else{
                LoadServerAddress();
            }
            Connect(GameServerAddr, GameServerPort);
        }

        void LoadServerAddress(){
            MatchServerAddr = PlayerPrefs.GetString("MatchServerAddr");
            GameServerAddr = PlayerPrefs.GetString("GameServerAddr");
        }

        public void Connect(string host, int port){
            m_socket ??= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            m_socket.Connect(host, port);

            if(!m_socket.Connected){
                m_socket = null;
                return;
            }

            Thread.Sleep(100);
            
            byte[] authBuffer = new byte[40];
            Array.Copy(BitConverter.GetBytes(UserId), 0, authBuffer, 0, 4);
            Array.Copy(Encoding.UTF8.GetBytes(SessionId), 0, authBuffer, 4, 36);

            Send(authBuffer);

            BeginReceive();
        }

        public void Send(IMessage packet)
        {
            string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            PacketId msgId = (PacketId)Enum.Parse(typeof(PacketId), msgName);
            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
            Send(new ArraySegment<byte>(sendBuffer));
        }

        public void Send(ArraySegment<byte> bytes){
            if(!Connected) {
                UnityEngine.Debug.Log("Not Connected");
                return;
            }

            var sendState = new CommunicationState();
            sendState.socket = m_socket;
            //Array.Copy(bytes, sendState.buffer, bytes.Length);
            //sendState.length = bytes.Length;

            m_socket.BeginSend(bytes.Array, 0, bytes.Count, SocketFlags.None, new AsyncCallback(SendCallback), sendState);
        }

        public void Disconnect(){
            if(Connected){
                m_socket.BeginDisconnect(false, new AsyncCallback(DisconnectCallback), new AsyncState{
                    socket = m_socket
                });
            }
        }

        private void BeginReceive(){
            var recvState = new CommunicationState();
            recvState.socket = m_socket;
            m_socket.BeginReceive(recvState.buffer, 0, CommunicationState.BUF_MAX, SocketFlags.None, new AsyncCallback(ReceiveCallback), recvState);
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
            BeginReceive();
        }

        private void SendCallback(IAsyncResult ar){
            var state = ar.AsyncState as CommunicationState;
            Assert.IsNotNull(state);
            var socket = state.socket;

            var length = socket.EndSend(ar, out var error);

            //UnityEngine.Debug.Log(error);

            var sent = error == SocketError.Success;

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
    }
}
