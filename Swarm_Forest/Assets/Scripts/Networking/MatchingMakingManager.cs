using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domino.Networking.TCP;
using Domino.Networking.Event;
using System;
using Unity.VisualScripting;

public class MatchingMakingManager : MonoBehaviour
{
    private Session Session{get;set;} = null;
    // Start is called before the first frame update
    public void Start()
    {
        if(Session != null){
            this.Session.Disconnect();
            Session = new Session();
        }
        
        var ip = ""; // GlobalSetting.RemoteIP;
        var port = 1234; // GlobalSetting.RemotePort;


        Session.ConnectedEvent += OnConnect;
        Session.ReceivedEvent += OnReceive;
        Session.DisconnectedEvent += OnDisconnected;

        Session.Connect(ip, port);
    }

    private void OnConnect(object sender, ConnectivityEventArgs e){
        if(!e.Connected){
            // Do Something - Cannot connect to remote

            return;
        }
        // Send register sign to matchmaking server 
        // Send(Coder.Encode(MatchRegister.Factory.Create()));
        Session.Send(Coder.Encode(MatchRegister.Factory.Create()));
    }

    private void OnReceive(object sender, ReceivedEventArgs e){
        var packetID = Coder.Decode<PacketBase>(e.Buffer).PacketID;

        if(packetID != PacketID.MatchCreated){
            return;
        }

        var packet = Coder.Decode<MatchCreated>(e.Buffer);

        var RoomID = packet.RoomID;

        // TODO: change scene here (game scene, pass room id)



        // After change scene
        Session.Disconnect();
    }

    private void OnDisconnected(object sender, ConnectivityEventArgs e){
        // if this function called while game is running (except close application)
        // it means client disconnected from remote (server)
        Debug.Log("Matchmaking Server Disconnected");
    }

    // UNNECESSARY 
    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
