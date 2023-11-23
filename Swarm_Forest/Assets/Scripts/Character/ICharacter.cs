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

    public Dictionary<KeyCode, GameObject> Skills;

    public void AddSkill(KeyCode keyCode, int ID){
        if(!Skills.ContainsKey(keyCode))
            throw new KeyNotFoundException($"Invalid KeyCode value: {keyCode}");

        var skillObject = SkillList.Get(ID);
        Skills[keyCode] = skillObject;
    }
    
    public void RemoveSkill(KeyCode keyCode){
        if(!Skills.ContainsKey(keyCode))
            throw new KeyNotFoundException($"Invalid KeyCode value: {keyCode}");

        Skills[keyCode] = null;
    }

    private event Action<int> PassiveSkillEffect;

    void Start(){
        Skills = new Dictionary<KeyCode, GameObject>{
            {KeyCode.Q, null},
            {KeyCode.W, null},
            {KeyCode.E, null},
            {KeyCode.R, null}
        };
    }
}