using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domino.Networking.TCP;
using Domino.Networking.Event;
using System;
using Unity.VisualScripting;

public class MatchingMakingManager : MonoBehaviour
{
    private static MatchingMakingManager sharedInstance = null;
    public static MatchingMakingManager SharedInstance{
        get{
            return sharedInstance;
        }
    }

    private void Awake(){
        sharedInstance = this;
    }

    private Session Session{get;set;}
    // Start is called before the first frame update
    void Start()
    {
        if(Session != null){
            Session.Disconnect();
            Session = null;
        }
        
        var ip = ""; // GlobalSetting.RemoteIP;
        var port = 1234; // GlobalSetting.RemotePort;

        Session = Session.SharedInstance;

        Session.ConnectedEvent += OnConnect;
        Session.ReceivedEvent += OnReceive;
        Session.DisconnectedEvent += OnDisconnected;

        Session.Connect(ip, port);
    }

    public void Send(byte[] bytes){
        Session.Send(bytes);
    }

    private void OnConnect(object sender, ConnectivityEventArgs e){
        if(!e.Connected){
            // Do Something - Cannot connect to remote

            return;
        }
        // Send register sign to matchmaking server 
        // Send(Coder.Encode(MatchRegister.Factory.Create()));
    }

    private void OnReceive(object sender, ReceivedEventArgs e){
        var packetID = Coder.Decode<PacketBase>(e.Buffer).PacketID;

        // TODO: 
        // 1. create event for each case 
        // 2. deserialize byte array (e.Buffer) and invoke event with deserialized object
        switch(packetID){
            case PacketID.MatchCreated:{

            }
            break;
            case PacketID.MoveObject:{

            }
            break;
            case PacketID.InstantiateObject:{

            }
            break;
            case PacketID.UpdateObjectStatus:{

            }
            break;
            case PacketID.EliteSpawnTimer:{

            }
            break;
        }
    }

    private void OnDisconnected(object sender, ConnectivityEventArgs e){
        // if this function called while game is running (except close application)
        // it means client disconnected from remote (server)

    }

    // UNNECESSARY 
    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
