using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NormalEnemy : ICharacter
{
    public GameObject ChaseObject;
    public int ChaseObjectID;
    private float timer = 0.0f;    
    private NavMeshAgent nvAgent;

    private void Start()
    {        
        Type = 10;
        Prefab = this.gameObject;
        AttackPoint = 5;
        MaxHealthPoint = 30;
        HealthPoint = 30;
        MovementVelocity = 5f;
        gameNetworkingManager = GameObject.Find("GameManager").gameObject;

        nvAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (timer > 5f)
        {
            if (ChaseObject != null && gameNetworkingManager.GetComponent<ObjectManager>().current_PlayerID == ChaseObjectID)
            {
                //nvAgent.SetDestination(ChaseObject.transform.position);
                gameNetworkingManager.GetComponent<GameNetworkingManager>().MoveObject(ID, ChaseObject.transform.position);
            }
            timer = 0f;
        }
        timer += Time.deltaTime;
    }
}