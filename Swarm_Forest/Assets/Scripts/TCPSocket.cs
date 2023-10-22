using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Assertions;

public class ConnectEventArgs{
    public TcpClient tcpClient;
}

public class TcpSocket
{
    /// <summary>
    /// For Singleton object
    /// </summary>
    static TcpSocket sharedInstance = null;
    static TcpSocket SharedInstance(){
        sharedInstance ??= new TcpSocket();
        return sharedInstance;
    }

    private TcpClient _tcpClient;

    public delegate void ConnectedEventHandler(object sender, bool connected);

    public event ConnectedEventHandler ConnectedEvent;

    public TcpSocket(){
        this._tcpClient = new TcpClient(AddressFamily.InterNetwork);
    }

    public void BeginConnect(string host, int port){
        Assert.IsNotNull(this._tcpClient);

        this._tcpClient.BeginConnect
        (
            host, port, 
            new AsyncCallback(this.BeginConnectCallback), 
            new ConnectEventArgs
            {
                tcpClient = this._tcpClient          
            }
        );
    }

    private void BeginConnectCallback(IAsyncResult ar){
        var args = ar.AsyncState as ConnectEventArgs;
        Assert.IsNotNull(args);

        var tcpClient = args.tcpClient;

        tcpClient.EndConnect(ar);

        ConnectedEvent?.Invoke(this, tcpClient.Connected);
    }
}
