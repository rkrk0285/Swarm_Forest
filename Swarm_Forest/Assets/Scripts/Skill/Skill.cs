using System;
using Domino.Networking.TCP;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Skill: MonoBehaviour{
    
    public int ID;
    public float BaseDamage;
    public float PercentDamage;    
    public float LifeTime;
    public float Cooldown;
    public Action Action;

    public virtual void Activate(GameObject Effect, Vector3 direction)
    {
        // DO NOTHING
        // Override this function when inherrit this class to child class
    }     
}