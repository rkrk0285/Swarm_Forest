using System;
using System.Linq;
using System.Collections.Generic;
using Domino.Networking.Event;
using Domino.Networking.TCP;
using Google.Protobuf;
using Google.Protobuf.GameProtocol;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using Unity.Mathematics;


class GameNetworkingManager: MonoBehaviour{

    public GameObject ObjectManager;
    Session session;

    public int UserId{get; private set;}
    public string SessionId{get; private set;}
    public int RoomId{get; private set;}

    Dictionary<ushort, Func<ArraySegment<byte>, IMessage>> packetMakers = new Dictionary<ushort, Func<ArraySegment<byte>, IMessage>>();
    Dictionary<ushort, Action<Session, IMessage>> packetHandlers = new Dictionary<ushort, Action<Session, IMessage>>();

    void Start(){
        LoadSessionState();
        InitializePacketHandlers();
        InitializeSession();
    }

    void InitializePacketHandlers(){
        packetMakers.Add((ushort)PacketId.Moveobject, MakePacket<MoveObject>);
        packetMakers.Add((ushort)PacketId.Objectdead, MakePacket<ObjectDead>);
        packetMakers.Add((ushort)PacketId.Objectidres, MakePacket<ObjectIDRes>);
        packetMakers.Add((ushort)PacketId.Instantiateobject, MakePacket<InstantiateObject>);
        packetMakers.Add((ushort)PacketId.Updateobjectstatus, MakePacket<UpdateObjectStatus>);
        packetMakers.Add((ushort)PacketId.Elitespawntimer, MakePacket<EliteSpawnTimer>);

        packetHandlers.Add((ushort)PacketId.Moveobject, MoveObject);
        packetHandlers.Add((ushort)PacketId.Objectdead, ObjectDead);
        packetHandlers.Add((ushort)PacketId.Objectidres, ObjectIDRes);
        packetHandlers.Add((ushort)PacketId.Instantiateobject, InstantiateObject);
        packetHandlers.Add((ushort)PacketId.Updateobjectstatus, UpdateObjectStatus);
        packetHandlers.Add((ushort)PacketId.Elitespawntimer, EliteSpawnTimer);
    }

    void LoadSessionState(){
        UserId = PlayerPrefs.GetInt("UserId");
        RoomId = PlayerPrefs.GetInt("RoomId");
        SessionId = PlayerPrefs.GetString("SessionId");
    }

    void InitializeSession(){
        session = new Session(UserId, SessionId);
        session.ReceivedEvent += OnReceive;
        session.ConnectGameServer();
    }

    public EventHandler<MoveObject> MoveObjectEventHandler;
    void MoveObject(Session session, IMessage packet){
        var moveObjectPacket = packet as MoveObject;
        MoveObjectEventHandler?.Invoke(this, moveObjectPacket);
    }

    public EventHandler<ObjectDead> ObjectDeadEventHandler;
    void ObjectDead(Session session, IMessage packet){
        var objectDeadPacket = packet as ObjectDead;
        ObjectDeadEventHandler?.Invoke(this, objectDeadPacket);
    }

    //Queue<int> NewObjectIds = new Queue<int>();
    Queue<InstantiateObject> NewObjects = new();

    void ObjectIDRes(Session session, IMessage packet){
        var res = packet as ObjectIDRes;
        //NewObjectIds.Enqueue(res.ObjectId);
        NewObjects.Peek().ObjectId = res.ObjectId;
    }

    public EventHandler<InstantiateObject> InstantiateObjectEventHandler;
    void InstantiateObject(Session session, IMessage packet){
        var instantiatePacket = packet as InstantiateObject;
        instantiatePacket.ObjectId = -1;
        session.Send(new ObjectIDReq());
    }

    public EventHandler<EliteSpawnTimer> EliteSpawnTimerEventHandler;
    void EliteSpawnTimer(Session session, IMessage packet){
        var eliteSpawnTimerPacket = packet as EliteSpawnTimer;
        EliteSpawnTimerEventHandler?.Invoke(this, eliteSpawnTimerPacket);
    }

    public EventHandler<UpdateObjectStatus> UpdateObjectEventHandler;
    void UpdateObjectStatus(Session session, IMessage packet){
        var updateObjectPacket = packet as UpdateObjectStatus;
        UpdateObjectEventHandler?.Invoke(this, updateObjectPacket);
    }

    void Update(){
        if(NewObjects.Count != 0 && NewObjects.Peek().ObjectId != -1){
            var NewObjectInfo = NewObjects.Dequeue();
            InstantiateObjectEventHandler?.Invoke(this, NewObjectInfo);
        }
    }

    void OnReceive(object sender, ReceivedEventArgs e){
        var session = sender as Session;
        if(e.Buffer is null || e.Buffer.Count() == 0)
        {
            Debug.Log("Disconnected");
            session.Disconnect();
        }

        ushort count = 0;
        ushort packetSize = BitConverter.ToUInt16(e.Buffer, 0);
        count += 2;
        ushort packetId = BitConverter.ToUInt16(e.Buffer, 0 + count);
        count += 2;

        var packetMaker = packetMakers.GetValueOrDefault(packetId, null);
        IMessage packet = packetMaker?.Invoke(new ArraySegment<byte>(e.Buffer, 0, packetSize)) ?? throw new InvalidDataException();
        var packetHandler = packetHandlers.GetValueOrDefault(packetId, null);
        packetHandler?.Invoke(session, packet);
    }

    T MakePacket<T>(ArraySegment<byte> buffer) where T : IMessage, new()
    {
        T packet = new T();
        packet.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
        return packet;
    }
}