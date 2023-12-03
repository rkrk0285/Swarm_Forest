using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : ICharacter
{    
    private void Start()
    {
        ID = -1;
        Type = 10;
        Prefab = this.gameObject;
        AttackPoint = 5;
        MaxHealthPoint = 50;
        HealthPoint = 50;
        MovementVelocity = 5f;
    }    
}