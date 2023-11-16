using System;
using Domino.Networking.TCP;
using Unity.VisualScripting;
using UnityEngine;

public class Skill: MonoBehaviour{
    public GameObject EffectPrefab{get;set;} = null;
    public int ID{get;set;}
    public float BaseDamage{get;set;}
    public float PercentDamage{get;set;}
    public float LifeTime{get;set;}
    public float Cooldown{get;set;}
    public float Force{get;set;}
    public Action Action{get;set;}
    public Action<Collider> TriggerEntered{get;set;}


    private void OnTriggerEnter(Collider other){
        Debug.Log(other.gameObject.tag);

        TriggerEntered?.Invoke(other);
    }
}

public class ProjectileSkill: Skill{
    public new Action<GameObject, Vector3, float> Action{get;set;}
}

public class PressingSkill: Skill{
    public new Action<GameObject, Vector3> Action{get;set;}
}

public class PassiveSkill: Skill{
    // 이 Action을 ICharacter에 추가
    public new Action<int> Action{get;set;}
}