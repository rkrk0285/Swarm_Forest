using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    Plane _plane;
    
    void Update()
    {
        input_Control();
    }

    void input_Control()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 오른쪽 클릭 입력
        if (Input.GetMouseButton(1))
        {            
            if (Physics.Raycast(ray, out hit))
            {                
                Player.GetComponent<PlayerManager>().movePlayer(hit.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q 입력");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W 입력");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E 입력");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R 입력");
        }
    }
}
