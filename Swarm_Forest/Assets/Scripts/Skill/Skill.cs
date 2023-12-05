using System;
using Domino.Networking.TCP;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Skill: MonoBehaviour{

    public int CasterId;
    public int ID;
    public int Type;
    public int BaseDamage;    
    public float PercentDamage;    
    public float LifeTime;    
    public float Cooldown;
    public float HealthPoint;
    public Action Action;    
    public GameObject gameNetworkingManager;
    public bool isActivated = false;

    public virtual void Activate(ICharacter caster, GameObject Effect, Vector3 direction)
    {
        // DO NOTHING
        // Override this function when inherrit this class to child class
    }

    public GameObject InstantiateEffect(GameObject skillObject, Vector3 origin)
    {
        return Instantiate(
                skillObject,
                origin,
                Quaternion.identity,
                GameManager.instance.transform
            );
    }
}