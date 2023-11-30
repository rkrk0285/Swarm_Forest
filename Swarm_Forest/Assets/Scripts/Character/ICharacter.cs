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

    public Dictionary<KeyCode, int> Skills = new Dictionary<KeyCode, int>{
            {KeyCode.Q, -1},
            {KeyCode.W, -1},
            {KeyCode.E, -1},
            {KeyCode.R, -1}
        };

    public void AddSkill(KeyCode keyCode, int ID){        
        if(!Skills.ContainsKey(keyCode))
            throw new KeyNotFoundException($"Invalid KeyCode value: {keyCode}");

        //var skillObject = SkillList.Get(ID);
        Skills[keyCode] = ID;
    }
    
    public void RemoveSkill(KeyCode keyCode){
        if(!Skills.ContainsKey(keyCode))
            throw new KeyNotFoundException($"Invalid KeyCode value: {keyCode}");

        Skills[keyCode] = -1;
    }

    protected void DieCharacter()
    {
        Destroy(this.gameObject);
    }

    private event Action<int> PassiveSkillEffect;

    void Start(){                       
    }
}