using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domino.Networking.TCP;
using Domino.Networking.Event;
using System;
using Domino.Networking.HTTP;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Google.Protobuf.GameProtocol;

public class MatchmakingManager : MonoBehaviour
{
    Session session;

    void InitializeSession(){
        session = new Session();
        session.ConnectMatchmakingServer();
    }

    public bool Connected => session.Connected;

    ReceiveEventHandler currentFunc = null;

    Queue<ReceiveEventHandler> receiveEventHandlers = new();
    void OnDestroy()
    {
        if (session.Connected) session.Disconnect();
    }
    public void AddReceiveCallback(ReceiveEventHandler func){
        receiveEventHandlers.Enqueue(func);
    }

    public void RemoveReceiveCallback(ReceiveEventHandler func){
        session.ReceivedEvent -= func;
    }

    public void Register(){
        if(!session.Connected) return;
        session.Send(new C_Join());
    }

    public void Cancel(){
        if(!session.Connected) return;
        session.Send(new C_Cancel());
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeSession();
    }

    void Update(){
        while(receiveEventHandlers.Count != 0){
            session.ReceivedEvent += receiveEventHandlers.Dequeue();
        }
    }
}
