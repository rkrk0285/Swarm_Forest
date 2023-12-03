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
    private float spawn_Interval = 5f;    
    private float spawn_Timer = 0.0f;
            
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
            Instantiate(obj, spawnPos, Quaternion.identity, parent_Transform);               
            obj.GetComponent<ICharacter>().ID = Info.ObjectId;
        }

        while(nextUpdateObjects.Count != 0)
        {
            var info = nextUpdateObjects.Dequeue();
            GameObject obj = FindObject(info.ObjectId);

            if (obj == null)
                continue;

            obj.GetComponent<ICharacter>().HealthPoint = info.HP;
        }
    }


    void InitializeNetworkHandler()
    {
        gameNetworkingManager.GetComponent<GameNetworkingManager>().InstantiateObjectEventHandler += InstantiateObjectHandler;
        gameNetworkingManager.GetComponent<GameNetworkingManager>().UpdateObjectEventHandler += UpdateObjectHandler;
    }

    Queue<InstantiateObject> nextInstantiateObjects = new Queue<InstantiateObject>();
    void InstantiateObjectHandler(object sender, InstantiateObject instantiateObjectPacket)
    {
        Debug.Log("IOH");
        nextInstantiateObjects.Enqueue(instantiateObjectPacket);
    }

    Queue<UpdateObjectStatus> nextUpdateObjects = new Queue<UpdateObjectStatus>();
    void UpdateObjectHandler(object sender, UpdateObjectStatus updateObjectStatusPacket)
    {
        Debug.Log("UOH");
        nextUpdateObjects.Enqueue(updateObjectStatusPacket);
    }

    void UpdateObjectEventHandler(object sender, EliteSpawnTimer packet)
    {

    }

    void ObjectDeadEventHandler(object sender, ObjectDead packet)
    {
        // ������Ʈ ���̵�� ������Ʈ ����.
        Destroy(FindObject(packet.ObjectId));
    }

    void Send_NormalSpawnRequest(int Type)
    {
        // �Ϲ� �� 4������ ��ȯ
        float z = Random.Range(-400, 400);
        float[] dx = new float[4] { 470, -470, z, z };
        float[] dz = new float[4] { z, z, 470, -470 };
        
        int hp = Get(Type).GetComponent<ICharacter>().HealthPoint;
        for (int i = 0; i < 4; i++)
        {
            // ��û �߼�.
            Send_SpawnRequest(Type, hp, new UnityEngine.Vector3(dx[i], 0, dz[i]));
        }
    }

    void Send_SpawnRequest(int Type, int HP, UnityEngine.Vector3 Pos)
    {
        // ��ȯ ��û.
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
        // Ÿ�� ������ ���� ������Ʈ ����.
        if (!AllObjects.ContainsKey(Type))
            throw new KeyNotFoundException($"There is no skill have Type: {Type}");
        return Resources.Load<GameObject>(AllObjects[Type]);
    }    
}
