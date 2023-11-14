using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerManager : MonoBehaviour
{
    public Camera player_Camera;    
    bool autoMove = false;
    Vector3 destination;
    Vector3 offset = new Vector3(0, 0, 80 / (Mathf.Sin(50 * Mathf.Deg2Rad) / Mathf.Cos(50 * Mathf.Deg2Rad)));

    private NavMeshAgent nvAgent;

    private void Start()
    {
        nvAgent = GetComponent<NavMeshAgent>();        
    }
    private void Update()
    {
        if (autoMove)
        {            
            nvAgent.SetDestination(destination);
            player_Camera.transform.position = new Vector3(transform.position.x, 80, transform.position.z) - offset;
        }        
    }
    public void movePlayer(Vector3 pos)
    {
        autoMove = true;
        destination = new Vector3(pos.x, 0, pos.z);
    }
}
