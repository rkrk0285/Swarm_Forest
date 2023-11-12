using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerManager : MonoBehaviour
{
    public Camera player_Camera;    
    bool autoMove = false;
    Vector3 destination;
    Vector3 offset = new Vector3(0, 0, 36.49635f);

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
            player_Camera.transform.position = new Vector3(transform.position.x, 100, transform.position.z) - offset;
        }        
    }
    public void movePlayer(Vector3 pos)
    {
        autoMove = true;
        destination = new Vector3(pos.x, 0, pos.z);
    }
}
