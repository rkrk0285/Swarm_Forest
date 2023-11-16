
using System.Collections.Generic;
using UnityEngine;

partial class SkillManager{
    public static Skill GetSkill(int ID){
        return AllSkills[ID];
    }

    private static Dictionary<int, Skill> AllSkills = new Dictionary<int, Skill>(){
        {0, new ProjectileSkill(){
            EffectPrefab = (GameObject)Resources.Load(""),
            ID = 0,
            BaseDamage = 10,
            PercentDamage = 1.15f,
            LifeTime = 5f,
            // Second
            Cooldown = 3f,
            Force = 10f,
            Action = (EffectObject, Direction, Force) => {
                EffectObject.GetComponent<Rigidbody>().AddForce(Direction * Force, ForceMode.Impulse);
            }
        }},
    };
}