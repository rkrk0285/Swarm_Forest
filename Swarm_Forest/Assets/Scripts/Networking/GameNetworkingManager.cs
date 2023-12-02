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
    Session session;

    public int UserId{get; private set;}
    public string SessionId{get; private set;}
    public int RoomId{get; private set;}

    private Dictionary<ushort, Func<ArraySegment<byte>, IMessage>> packetMakers = new Dictionary<ushort, Func<ArraySegment<byte>, IMessage>>();
    private Dictionary<ushort, Action<Session, IMessage>> packetHandlers = new Dictionary<ushort, Action<Session, IMessage>>();

    private void Start(){
        LoadSessionState();
        InitializePacketHandlers();
        InitializeSession();
    }

    private void InitializePacketHandlers(){
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

    private void LoadSessionState(){
        UserId = PlayerPrefs.GetInt("UserId");
        RoomId = PlayerPrefs.GetInt("RoomId");
        SessionId = PlayerPrefs.GetString("SessionId");
    }

    private void InitializeSession(){
        session = new Session(UserId, SessionId);
        session.ReceivedEvent += OnReceive;
        session.ConnectGameServer();
    }

    public EventHandler<MoveObject> MoveObjectEventHandler;
    private void MoveObject(Session session, IMessage packet){
        var moveObjectPacket = packet as MoveObject;
        MoveObjectEventHandler?.Invoke(this, moveObjectPacket);
    }
    
    public void MoveObject(int ObjectId, UnityEngine.Vector3 Position){
        session.Send(new MoveObject(){
            ObjectId = ObjectId,
            Position = Position.ToPacketVector3()
        });
    }

    public EventHandler<ObjectDead> ObjectDeadEventHandler;
    private void ObjectDead(Session session, IMessage packet){
        var objectDeadPacket = packet as ObjectDead;
        ObjectDeadEventHandler?.Invoke(this, objectDeadPacket);
    }

    public void ObjectDead(int ObjectId){
        session.Send(new ObjectDead(){
            ObjectId = ObjectId
        });
    }

    private void ObjectIDRes(Session session, IMessage packet){
        var res = packet as ObjectIDRes;
        // DO NOTHING
    }

    public EventHandler<InstantiateObject> InstantiateObjectEventHandler;
    private void InstantiateObject(Session session, IMessage packet){
        var instantiatePacket = packet as InstantiateObject;
        InstantiateObjectEventHandler?.Invoke(instantiatePacket);
    }

    public void InstantiateObject(int ObjectType, int HP, UnityEngine.Vector3 Position){
        session.Send(new InstantiateObject(){
            ObjectId = -1,
            ObjectType = ObjectType,
            HP = HP,
            Position = Position.ToPacketVector3()
        });
    }

    public EventHandler<EliteSpawnTimer> EliteSpawnTimerEventHandler;
    private void EliteSpawnTimer(Session session, IMessage packet){
        var eliteSpawnTimerPacket = packet as EliteSpawnTimer;
        EliteSpawnTimerEventHandler?.Invoke(this, eliteSpawnTimerPacket);
    }

    public EventHandler<UpdateObjectStatus> UpdateObjectEventHandler;
    private void UpdateObjectStatus(Session session, IMessage packet){
        var updateObjectPacket = packet as UpdateObjectStatus;
        UpdateObjectEventHandler?.Invoke(this, updateObjectPacket);
    }

    public void UpdateObjectStatus(int ObjectId, int HP){
        session.Send(new UpdateObjectStatus(){
            ObjectId = ObjectId,
            HP = HP
        });
    }

    private void OnReceive(object sender, ReceivedEventArgs e){
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

    private T MakePacket<T>(ArraySegment<byte> buffer) where T : IMessage, new()
    {
        T packet = new T();
        packet.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
        return packet;
    }
}