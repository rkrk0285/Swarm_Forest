using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{    
    public override void Activate(GameObject Effect, Vector3 direction)
    {
        // DO NOTHING
        // Override this function when inherrit this class to child class
        Effect.GetComponent<Rigidbody>().AddForce(direction * 100f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // 데미지 계산하는 곳.

            //
            Destroy(this.gameObject);
        }
    }
}
