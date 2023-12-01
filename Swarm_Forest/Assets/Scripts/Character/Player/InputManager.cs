using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    Plane _plane;    

    //임시
    private PlayerManager playerManager;
    private void Start()
    {
        playerManager = Player.GetComponent<PlayerManager>();
    }
    //

    void Update()
    {
        input_Control();
    }

    void input_Control()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
         
        if (Input.GetMouseButton(1))
        {            
            if (Physics.Raycast(ray, out hit))
            {                
                Player.GetComponent<PlayerManager>().movePlayer(hit.point);
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
    private Vector3 NormalizeRayPoint(Vector3 rayPoint)
    {
        var direction = rayPoint - Player.transform.position;
        direction.y = 1f;
        direction = direction.normalized;

        return direction;
    }

    private Vector3 MousePositionOnMap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var raycastHit))
        {
            return raycastHit.point;
        }

        return Vector3.zero;
    }
    #endregion

    // 임시
    public void add_Skill(KeyCode keyCode, int num)
    {
        playerManager.AddSkill(keyCode, num);
        playerManager.switchSkillUI(keyCode, num);
    }
}