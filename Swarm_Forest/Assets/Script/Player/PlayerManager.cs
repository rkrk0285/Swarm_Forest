using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Camera player_Camera;
    float velocity = 5f;
    bool autoMove = false;
    Vector3 destination;

    private void Update()
    {
        if (autoMove)
        {
            setPlayerDirection(destination);
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
        player_Camera.transform.position = new Vector3(transform.position.x, 10, transform.position.z);
    }

    public void movePlayer(Vector3 pos)
    {
        autoMove = true;
        destination = new Vector3(pos.x, 0, pos.z);
    }   
}
