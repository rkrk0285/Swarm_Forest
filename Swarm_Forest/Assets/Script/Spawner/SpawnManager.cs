using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Plane;
    public float spawn_Interval = 0.5f;
    private float spawn_Timer = 0.0f;
        
    private void Update()
    {
        spawn_Timer += Time.deltaTime;
        
        if (spawn_Timer >= spawn_Interval)
        {
            SpawnEnemy();
            spawn_Timer = 0f;
        }
    }

    void SpawnEnemy()
    {        
        float z = Random.Range(-20, 20);
        float map_size = Plane.transform.localScale.x * 10;
        Debug.Log(map_size);
        Instantiate(Enemy1, new Vector3(map_size / 2 - 3f, 0, z), Quaternion.identity);
        Instantiate(Enemy1, new Vector3(3f - map_size / 2, 0, z), Quaternion.identity);
        Instantiate(Enemy1, new Vector3(z, 0, map_size / 2 - 3f), Quaternion.identity);
        Instantiate(Enemy1, new Vector3(z, 0, 3f - map_size / 2), Quaternion.identity);
    }
}
