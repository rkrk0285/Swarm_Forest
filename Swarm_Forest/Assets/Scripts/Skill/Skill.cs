using System;
using Domino.Networking.TCP;
using Unity.VisualScripting;
using UnityEngine;

public class Skill: MonoBehaviour{
    public int ID{get;set;}
    public float BaseDamage{get;set;}
    public float PercentDamage{get;set;}
    public float LifeTime{get;set;}
    public float Cooldown{get;set;}
    public Action Action{get;set;}

    public void Activate(Vector3 direction){
        // DO NOTHING
        // Override this function when inherrit this class to child class
    }
}