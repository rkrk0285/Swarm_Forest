using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{    
    Vector3 destination;
    private NavMeshAgent nvAgent;

    private void Start()
    {
        nvAgent = GetComponent<NavMeshAgent>();                
    }
    private void Update()
    {
        destination = GameObject.Find("Player").transform.position;
        nvAgent.SetDestination(destination);
    }
}
