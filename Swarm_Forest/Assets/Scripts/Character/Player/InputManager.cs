using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.GameProtocol;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject gameNetworkingManager;
    public PlayerManager playerManager;
    private void Start()
    {
        InitializeNetworkHandler();
    }
    //

    void Update()
    {
        input_Control();     
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
    }

    void MoveObjectHandler(object sender, MoveObject moveObjectPacket)
    {
        if (playerManager.ID != moveObjectPacket.ObjectId)
            return;

        DoMove = true;
        nextPosition = moveObjectPacket.Position.ToUnityVector3();
    }

    void input_Control()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit))
            {
                gameNetworkingManager.GetComponent<GameNetworkingManager>().MoveObject(playerManager.ID, hit.point);
            }
        }        
        if (Input.GetKeyDown(KeyCode.Q))
            GameManager.instance.skillManager.Cast(playerManager, playerManager.Skills[KeyCode.Q], NormalizeRayPoint(MousePositionOnMap()));
        if (Input.GetKeyDown(KeyCode.W))
            GameManager.instance.skillManager.Cast(playerManager, playerManager.Skills[KeyCode.W], NormalizeRayPoint(MousePositionOnMap()));
        if (Input.GetKeyDown(KeyCode.E))
            GameManager.instance.skillManager.Cast(playerManager, playerManager.Skills[KeyCode.E], NormalizeRayPoint(MousePositionOnMap()));
        if (Input.GetKeyDown(KeyCode.R))
            GameManager.instance.skillManager.Cast(playerManager, playerManager.Skills[KeyCode.R], NormalizeRayPoint(MousePositionOnMap()));
    }

    #region Raycasting
    // Adjust Ray point
    private UnityEngine.Vector3 NormalizeRayPoint(UnityEngine.Vector3 rayPoint)
    {
        var direction = rayPoint - Player.transform.position;
        direction.y = 0f;
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
}