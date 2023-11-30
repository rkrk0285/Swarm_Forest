using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : ICharacter
{    
    private void Start()
    {
        ID = 1;
        Prefab = this.gameObject;
        AttackPoint = 5f;
        MaxHealthPoint = 50f;
        HealthPoint = 50f;
        MovementVelocity = 5f;
    }    
}