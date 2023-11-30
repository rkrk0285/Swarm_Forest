using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Enemy1;    
    public float spawn_Interval = 2f;
    public float map_size;
    private float spawn_Timer = 0.0f;

    private void Start()
    {
        SpawnEnemy();
    }

    //private void Update()
    //{
    //    spawn_Timer += Time.deltaTime;
        
    //    if (spawn_Timer >= spawn_Interval)
    //    {
    //        SpawnEnemy();
    //        spawn_Timer = 0f;
    //    }
    //}

    void SpawnEnemy()
    {        
        float z = Random.Range(-400, 400);        
        float[] dx = new float[4] { map_size - 30f, 30f - map_size, z, z };
        float[] dz = new float[4] { z, z, map_size - 30f, 30f - map_size };
        for (int i = 0; i < 4; i++)
        {
            Instantiate(Enemy1, new Vector3(dx[i], 0, dz[i]), Quaternion.identity);
        }
    }
}
