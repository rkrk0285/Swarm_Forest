using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class InputManager : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    Plane _plane;
    [SerializeField]
    private GameObject GameManager;

    void Update()
    {
        input_Control();
    }

    void input_Control()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ������ Ŭ�� �Է�
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
            GameManager.GetComponent<SkillManager>().cast_Fireball();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W �Է�");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E �Է�");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R �Է�");
        }
    }
}
*/