using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using Google.Protobuf.GameProtocol;
using UnityEngine.UI;
using Domino.Networking.Event;
using Unity.VisualScripting;
using System;
using System.Linq;
using Domino.Networking.TCP;
using System.IO;
using UnityEngine.SceneManagement;

public class MatchButton : MonoBehaviour
{
    public GameObject NetworkManager;
    MatchmakingManager networkManager{get;set;}

    Dictionary<ushort, Func<ArraySegment<byte>, IMessage>> packetMakers = new Dictionary<ushort, Func<ArraySegment<byte>, IMessage>>();
    Dictionary<ushort, Action<Session, IMessage>> packetHandlers = new Dictionary<ushort, Action<Session, IMessage>>();

    // Start is called before the first frame update
    void Start()
    {
        InitializeButton();
        InitializePacketHandlers();
        InitializeNetworkManager();
    }

    void InitializePacketHandlers(){
        packetMakers.Add((ushort)PacketId.SReady, MakePacket<S_Ready>);
        packetMakers.Add((ushort)PacketId.SResponse, MakePacket<S_Response>);
        packetHandlers.Add((ushort)PacketId.SReady, ProcessMatchReady);
        packetHandlers.Add((ushort)PacketId.SResponse, ProcessResult);
    }
    
    void InitializeButton(){
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    void InitializeNetworkManager(){
        networkManager = NetworkManager.GetComponent<MatchmakingManager>();
        networkManager.AddReceiveCallback(OnReceive);
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

    void ProcessResult(Session session, IMessage packet){
        var resultPacket = packet as S_Response;

        Debug.Log($"Result: {resultPacket.Successed}");
    }

    void ProcessMatchReady(Session session, IMessage packet){
        var readyPacket = packet as S_Ready;

        Debug.Log("match ready");
        Debug.Log(readyPacket.RoomId);

        // Save authentication status
        SaveSessionState(session, readyPacket);
        // Move Scene
        LoadGameScene();
    }

    void SaveSessionState(Session session, S_Ready readyPacket){
        PlayerPrefs.SetInt("RoomId", readyPacket.RoomId);
        PlayerPrefs.SetInt("UserId", session.UserId);
        PlayerPrefs.SetString("SessionId", session.SessionId);
        PlayerPrefs.SetString("MatchServerAddr", session.MatchServerAddr);
        PlayerPrefs.SetString("GameServerAddr", session.GameServerAddr);
    }

    void LoadGameScene(){
        StartCoroutine(LoadMyAsyncScene());
    }
    
    IEnumerator LoadMyAsyncScene()
    {    
        // AsyncOperation을 통해 Scene Load 정도를 알 수 있다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

        // Scene을 불러오는 것이 완료되면, AsyncOperation은 isDone 상태가 된다.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    bool Register = true;
    void OnClick(){
        if(!networkManager.Connected){
            Debug.Log("Disconnected");
            return;
        }
        //Debug.Log(networkManager.Connected);
        if(Register)networkManager.Register();
        else networkManager.Cancel();

        //Debug.Log(Register ? "Register" : "CancelRegister");

        Register = !Register;
    }

    T MakePacket<T>(ArraySegment<byte> buffer) where T : IMessage, new()
        {
            T packet = new T();
            packet.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
            return packet;
        }
}
