using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        {100, "SkillPrefab/FireBall"},
        {101, "SkillPrefab/ThrowingStar"},        
        {102, "SkillPrefab/Missile"},
        {103, "SkillPrefab/ForwardShot"},
        {104, "SkillPrefab/HyperBeam"},
    };

    public GameObject gameNetworkingManagerObject;
    GameNetworkingManager gameNetworkingManager;
    public Transform parent_Transform;
    private float spawn_Interval = 50f; // For Debug.
    private float spawn_Timer = 45f;
    public int current_PlayerID;
    public int current_PlayerType;
    public GameObject NormalEnemy;
    public Transform normal_Transform;

    
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

        while (nextInstantiateObjects.Count != 0)
        {
            var info = nextInstantiateObjects.Dequeue();
            GameObject obj = Get(info.ObjectType);

            UnityEngine.Vector3 spawnPos = info.Position.ToUnityVector3();
            GameObject clone = Instantiate(obj, spawnPos, Quaternion.identity, parent_Transform);

            // 캐릭터의 경우.
            Debug.Log(info.ObjectType);
            if (info.ObjectType < 10)
            {            
                clone.GetComponent<ICharacter>().ID = info.ObjectId;
                clone.GetComponent<ICharacter>().HealthPoint = info.HP;

                // 현재 플레이어 아이디 넣기.
                if (current_PlayerType == info.ObjectType && current_PlayerID == -1)
                {
                    current_PlayerID = info.ObjectId;
                }
            }                       
            else if (info.ObjectType == 10)
            {                                            
                clone.GetComponent<ICharacter>().ID = info.ObjectId;
                clone.GetComponent<ICharacter>().HealthPoint = info.HP;
                clone.GetComponent<NormalEnemy>().ChaseObjectID = info.CasterId;
                clone.GetComponent<NormalEnemy>().ChaseObject = FindObject(info.CasterId);
            }
            // 스킬의 경우.
            else
            {                
                Skill skill = clone.GetComponent<Skill>();
                skill.ID = info.ObjectId;
                skill.HealthPoint = info.HP;
                skill.isActivated = true;
                skill.CasterId = info.CasterId;                                
            }
            currentObjects.Add(info.ObjectId, clone);
        }

        while (nextUpdateObjects.Count != 0)
        {
            var info = nextUpdateObjects.Dequeue();
            GameObject obj = FindObject(info.ObjectId);

            if (obj == null)
                continue;

            if (obj.TryGetComponent<ICharacter>(out var icharacter))
                icharacter.HealthPoint = info.HP;
            else if (obj.TryGetComponent<Skill>(out var skill))
                skill.HealthPoint = info.HP;
        }

        while (nextDeadObjects.Count != 0)
        {
            var info = nextDeadObjects.Dequeue();
            var objectToDelete = FindObject(info.ObjectId);

            if (objectToDelete == null)                            
                continue;            

            currentObjects.Remove(info.ObjectId);
            Destroy(objectToDelete);
        }
        
        while (nextMoveObjects.Count != 0)
        {
            var info = nextMoveObjects.Dequeue();
            GameObject obj = FindObject(info.ObjectId);

            if (obj == null)
                continue;

            UnityEngine.Vector3 destination = new UnityEngine.Vector3(info.Position.ToUnityVector3().x, 0, info.Position.ToUnityVector3().z);
            UnityEngine.Vector3 obj_currrentPos = new UnityEngine.Vector3(obj.transform.position.x, 0, obj.transform.position.z);

            obj.transform.rotation = Quaternion.LookRotation(destination - obj_currrentPos);
            obj.transform.position = destination;
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
        UnityEngine.Vector3 SpawnPos = UnityEngine.Vector3.zero;
        switch (playerLocationPacket.Location)
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
        // 일반 적 2마리씩 소환
        float z = Random.Range(100, 900);
        float[] dx = new float[2] { 970, 30};        

        int hp = Get(Type).GetComponent<ICharacter>().HealthPoint;
        for (int i = 0; i < 2; i++)
        {
            // 요청 발송.            
            gameNetworkingManager.InstantiateObject(current_PlayerID, Type, hp, new UnityEngine.Vector3(dx[i], 0, z));
        }
    }

    private Dictionary<int, GameObject> currentObjects = new Dictionary<int, GameObject>();
    public GameObject FindObject(int FindID)
    {        
        if (!currentObjects.ContainsKey(FindID))
            return null;
        return currentObjects[FindID];
    }
    public static GameObject Get(int Type)
    {
        // 타입 값으로 게임 오브젝트 리턴.
        if (!AllObjects.ContainsKey(Type))
            throw new KeyNotFoundException($"There is no Character have Type: {Type}");
        return Resources.Load<GameObject>(AllObjects[Type]);
    }
}
