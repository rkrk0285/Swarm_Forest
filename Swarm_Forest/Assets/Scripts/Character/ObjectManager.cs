using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.GameProtocol;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static Dictionary<int, string> AllObjects = new Dictionary<int, string> 
    {
        {0, "ObjectPrefab/Player" },
        {10, "ObjectPrefab/NormalEnemy" },
    };

    public GameObject gameNetworkingManager;
    public Transform parent_Transform;
    private float spawn_Interval = 10000f; // For Debug.
    private float spawn_Timer = 10000f;
            
    int spawnID;
    private void Start()
    {
        InitializeNetworkHandler();
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
    }


    void InitializeNetworkHandler()
    {
        gameNetworkingManager.GetComponent<GameNetworkingManager>().InstantiateObjectEventHandler += InstantiateObjectHandler;
        gameNetworkingManager.GetComponent<GameNetworkingManager>().UpdateObjectEventHandler += UpdateObjectHandler;
        gameNetworkingManager.GetComponent<GameNetworkingManager>().PlayerLocationEventHandler += PlayerLocationHandler;
        gameNetworkingManager.GetComponent<GameNetworkingManager>().ObjectDeadEventHandler += ObjectDeadEventHandler;
    }

    Queue<InstantiateObject> nextInstantiateObjects = new Queue<InstantiateObject>();
    void InstantiateObjectHandler(object sender, InstantiateObject instantiateObjectPacket)
    {        
        nextInstantiateObjects.Enqueue(instantiateObjectPacket);
    }

    Queue<UpdateObjectStatus> nextUpdateObjects = new Queue<UpdateObjectStatus>();
    void UpdateObjectHandler(object sender, UpdateObjectStatus updateObjectStatusPacket)
    {
        Debug.Log("UOH");
        nextUpdateObjects.Enqueue(updateObjectStatusPacket);
    }
    
    void PlayerLocationHandler(object sender, PlayerLocation playerLocationPacket)
    {
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

        gameNetworkingManager.GetComponent<GameNetworkingManager>().InstantiateObject(0, 500, SpawnPos);
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
        gameNetworkingManager.GetComponent<GameNetworkingManager>().InstantiateObject(Type, HP, Pos);
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
            throw new KeyNotFoundException($"There is no skill have Type: {Type}");
        return Resources.Load<GameObject>(AllObjects[Type]);
    }    
}
