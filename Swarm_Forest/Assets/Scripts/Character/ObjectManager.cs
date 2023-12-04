using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.GameProtocol;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static Dictionary<int, string> AllObjects = new Dictionary<int, string> 
    {
        {6, "ObjectPrefab/Player" },
        {7, "ObjectPrefab/Player" },
        {8, "ObjectPrefab/Player" },
        {9, "ObjectPrefab/Player" },
        {10, "ObjectPrefab/NormalEnemy" },
    };

    public GameObject gameNetworkingManagerObject;
    GameNetworkingManager gameNetworkingManager;
    public Transform parent_Transform;
    private float spawn_Interval = 100f; // For Debug.
    private float spawn_Timer = 0f;
    public int current_PlayerID;
    public int current_PlayerType;
    private void Start()
    {
        gameNetworkingManager = gameNetworkingManagerObject.GetComponent<GameNetworkingManager>();
        InitializeNetworkHandler();
        current_PlayerID = -1;
    }

    private void Update()
    {
        spawn_Timer += Time.deltaTime;

        if (spawn_Timer >= spawn_Interval)
        {
            Send_NormalSpawnRequest(10);
            spawn_Timer = 0f;
        }
        
        while(nextInstantiateObjects.Count != 0)
        {            
            var Info = nextInstantiateObjects.Dequeue();
            
            GameObject obj = Get(Info.ObjectType);
            UnityEngine.Vector3 spawnPos = Info.Position.ToUnityVector3();
                           
            GameObject clone = Instantiate(obj, spawnPos, Quaternion.identity, parent_Transform);            
            clone.GetComponent<ICharacter>().ID = Info.ObjectId;

            // 현재 플레이어 아이디 넣기.
            if (current_PlayerType == Info.ObjectType && current_PlayerID == -1)
            {
                Debug.Log(Info.ObjectId);
                current_PlayerID = Info.ObjectId;
            }
        }

        while(nextUpdateObjects.Count != 0)
        {
            var info = nextUpdateObjects.Dequeue();
            GameObject obj = FindObject(info.ObjectId);

            if (obj == null)
                continue;

            obj.GetComponent<ICharacter>().HealthPoint = info.HP;
        }

        while(nextDeadObjects.Count != 0)
        {
            var Info = nextDeadObjects.Dequeue();            
            Destroy(FindObject(Info.ObjectId));
        }

        while (nextMoveObjects.Count != 0)
        {
            var info = nextMoveObjects.Dequeue();
            GameObject obj = FindObject(info.ObjectId);

            obj.transform.position = info.Position.ToUnityVector3();
        }
    }


    void InitializeNetworkHandler()
    {
        gameNetworkingManager.MoveObjectEventHandler += MoveObjectHandler;
        gameNetworkingManager.InstantiateObjectEventHandler += InstantiateObjectHandler;
        gameNetworkingManager.UpdateObjectEventHandler += UpdateObjectHandler;
        gameNetworkingManager.PlayerLocationEventHandler += PlayerLocationHandler;
        gameNetworkingManager.ObjectDeadEventHandler += ObjectDeadEventHandler;
    }
    
    Queue<InstantiateObject> nextInstantiateObjects = new Queue<InstantiateObject>();    
    void InstantiateObjectHandler(object sender, InstantiateObject instantiateObjectPacket)
    {        
        nextInstantiateObjects.Enqueue(instantiateObjectPacket);
    }

    Queue<UpdateObjectStatus> nextUpdateObjects = new Queue<UpdateObjectStatus>();
    void UpdateObjectHandler(object sender, UpdateObjectStatus updateObjectStatusPacket)
    {        
        nextUpdateObjects.Enqueue(updateObjectStatusPacket);
    }

    Queue<MoveObject> nextMoveObjects = new Queue<MoveObject>();
    void MoveObjectHandler(object sender, MoveObject moveObjectPacket)
    {
        if (current_PlayerID == moveObjectPacket.ObjectId)
            return;

        nextMoveObjects.Enqueue(moveObjectPacket);
    }
    void PlayerLocationHandler(object sender, PlayerLocation playerLocationPacket)
    {
        Debug.Log("PLH");
        UnityEngine.Vector3 SpawnPos = UnityEngine.Vector3.zero;
        switch(playerLocationPacket.Location)
        {
            case 1:
                SpawnPos = new UnityEngine.Vector3(500, 0, 100);
                break;
            case 2:
                SpawnPos = new UnityEngine.Vector3(900, 0, 500);
                break;
            case 3:
                SpawnPos = new UnityEngine.Vector3(500, 0, 900);
                break;
            case 4:
                SpawnPos = new UnityEngine.Vector3(100, 0, 500);
                break;
            default:
                return;
        }
        current_PlayerType = playerLocationPacket.Location + 5;
        gameNetworkingManager.InstantiateObject(current_PlayerType, 500, SpawnPos);
    }
    void UpdateObjectEventHandler(object sender, EliteSpawnTimer packet)
    {

    }

    Queue<ObjectDead> nextDeadObjects = new Queue<ObjectDead>();
    void ObjectDeadEventHandler(object sender, ObjectDead packet)
    {
        // 오브젝트 아이디로 오브젝트 삭제.
        nextDeadObjects.Enqueue(packet);        
    }

    void Send_NormalSpawnRequest(int Type)
    {
        // 일반 적 4마리씩 소환
        float z = Random.Range(100, 900);
        float[] dx = new float[4] { 970, 30, z, z };
        float[] dz = new float[4] { z, z, 970, 30 };
        
        int hp = Get(Type).GetComponent<ICharacter>().HealthPoint;
        for (int i = 0; i < 4; i++)
        {
            // 요청 발송.
            Send_SpawnRequest(Type, hp, new UnityEngine.Vector3(dx[i], 0, dz[i]));
        }
    }

    void Send_SpawnRequest(int Type, int HP, UnityEngine.Vector3 Pos)
    {
        // 소환 요청.
        gameNetworkingManager.InstantiateObject(Type, HP, Pos);
    }    

    public GameObject FindObject(int FindID)
    {
        for(int i = 0; i < parent_Transform.childCount; i++)
        {
            if (parent_Transform.GetChild(i).GetComponent<ICharacter>().ID == FindID)            
                return parent_Transform.GetChild(i).gameObject;            
        }
        return null;
    }
    public static GameObject Get(int Type)
    {
        // 타입 값으로 게임 오브젝트 리턴.
        if (!AllObjects.ContainsKey(Type))
            throw new KeyNotFoundException($"There is no Character have Type: {Type}");
        return Resources.Load<GameObject>(AllObjects[Type]);
    }    
}
