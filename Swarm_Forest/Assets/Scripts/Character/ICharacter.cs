using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICharacter: MonoBehaviour{
    public int ID{get; set;}
    public GameObject Prefab;
    
    public float AttackPoint{get;set;}

    public float MaxHealthPoint{get;set;}
    public float HealthPoint{get;set;}

    public float MovementVelocity{get; set;}

    public Dictionary<KeyCode, Skill> Skills;

    private event Action<int> PassiveSkillEffect;
}