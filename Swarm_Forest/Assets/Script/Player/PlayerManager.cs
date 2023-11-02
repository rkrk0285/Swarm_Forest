using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerManager : MonoBehaviour
{
    public Camera player_Camera;
    public float velocity = 50f;
    bool autoMove = false;
    Vector3 destination;

    private NavMeshAgent nvAgent;

    private void Start()
    {
        nvAgent = GetComponent<NavMeshAgent>();        
    }
    private void Update()
    {
        if (autoMove)
        {
            //setplayerdirection(destination);
            nvAgent.SetDestination(destination);
            player_Camera.transform.position = new Vector3(transform.position.x, 100, transform.position.z);
        }        
    }

    public void setPlayerDirection(Vector3 pos)
    {
        Vector3 dir = destination - transform.position;
        if (dir.magnitude < 0.0001f)
            autoMove = false;
        else
        {
            float moveDist = Mathf.Clamp(velocity * Time.deltaTime, 0, dir.magnitude);
            transform.position = transform.position + dir.normalized * moveDist;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            transform.LookAt(destination);
        }
        player_Camera.transform.position = new Vector3(transform.position.x, 100, transform.position.z);
    }

    public void movePlayer(Vector3 pos)
    {
        autoMove = true;
        destination = new Vector3(pos.x, 0, pos.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
