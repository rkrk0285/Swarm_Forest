using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.GameProtocol;
using UnityEngine;

public class InputManager : MonoBehaviour
{    
    public GameObject Player;    
    public GameObject gameNetworkingManager;    
    
    private PlayerManager playerManager;
    private void Start()
    {
        playerManager = Player.GetComponent<PlayerManager>();
        InitializeNetworkHandler();
    }
    //

    void Update()
    {
        input_Control();
        MovePlayerObejct();
    }

    // Run in Main Thread Only
    void MovePlayerObejct()
    {
        if (DoMove)
        {
            Player.GetComponent<PlayerManager>().movePlayer(nextPosition);
            DoMove = false;
        }
    }

    bool DoMove = false;
    UnityEngine.Vector3 nextPosition;

    void InitializeNetworkHandler()
    {
        gameNetworkingManager.GetComponent<GameNetworkingManager>().MoveObjectEventHandler += MoveObjectHandler;

        // FOR DEBUG
        gameNetworkingManager.GetComponent<GameNetworkingManager>().InstantiateObject(0, 100, new UnityEngine.Vector3(0, 0, 0));
    }
    
    void MoveObjectHandler(object sender, MoveObject moveObjectPacket)
    {
        DoMove = true;
        nextPosition = moveObjectPacket.Position.ToUnityVector3();
    }

    void input_Control()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButton(1))
        {
            if (Physics.Raycast(ray, out hit))
            {                
                gameNetworkingManager.GetComponent<GameNetworkingManager>().MoveObject(0, hit.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            add_Skill(KeyCode.Q, 0);
            GameManager.instance.skillManager.Cast(playerManager, playerManager.Skills[KeyCode.Q], NormalizeRayPoint(MousePositionOnMap()));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            add_Skill(KeyCode.W, 1);
            GameManager.instance.skillManager.Cast(playerManager, playerManager.Skills[KeyCode.W], NormalizeRayPoint(MousePositionOnMap()));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            add_Skill(KeyCode.E, 2);
            GameManager.instance.skillManager.Cast(playerManager, playerManager.Skills[KeyCode.E], NormalizeRayPoint(MousePositionOnMap()));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            add_Skill(KeyCode.R, 3);
            GameManager.instance.skillManager.Cast(playerManager, playerManager.Skills[KeyCode.R], NormalizeRayPoint(MousePositionOnMap()));
        }
    }

    #region Raycasting
    // Adjust Ray point
    private UnityEngine.Vector3 NormalizeRayPoint(UnityEngine.Vector3 rayPoint)
    {
        var direction = rayPoint - Player.transform.position;
        direction.y = 1f;
        direction = direction.normalized;

        return direction;
    }

    private UnityEngine.Vector3 MousePositionOnMap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var raycastHit))
        {
            return raycastHit.point;
        }

        return UnityEngine.Vector3.zero;
    }
    #endregion

    // 임시
    public void add_Skill(KeyCode keyCode, int num)
    {
        playerManager.AddSkill(keyCode, num);
        playerManager.switchSkillUI(keyCode, num);
    }
}