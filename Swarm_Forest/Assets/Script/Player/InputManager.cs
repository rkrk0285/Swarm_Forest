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
    private ICharacter Player_ICharacter;
    private void Start()
    {
        Player_ICharacter = Player.GetComponent<ICharacter>();        
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
            Debug.Log("Q �Է�");
            Player_ICharacter.AddSkill(KeyCode.Q, 0);
            GameManager.instance.skillManager.Cast(Player_ICharacter, Player_ICharacter.Skills[KeyCode.Q], NormalizeRayPoint(MousePositionOnMap()));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W �Է�");
            Player_ICharacter.AddSkill(KeyCode.W, 1);
            GameManager.instance.skillManager.Cast(Player_ICharacter, Player_ICharacter.Skills[KeyCode.W], NormalizeRayPoint(MousePositionOnMap()));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E �Է�");
            Player_ICharacter.AddSkill(KeyCode.E, 2);
            GameManager.instance.skillManager.Cast(Player_ICharacter, Player_ICharacter.Skills[KeyCode.E], NormalizeRayPoint(MousePositionOnMap()));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R �Է�");
            Player_ICharacter.AddSkill(KeyCode.R, 3);
            GameManager.instance.skillManager.Cast(Player_ICharacter, Player_ICharacter.Skills[KeyCode.R], NormalizeRayPoint(MousePositionOnMap()));
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
}