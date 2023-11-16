using System;
using System.Collections.Generic;
using UnityEngine;

partial class SkillList{
    private SkillList sharedInstance = null;
    public SkillList SharedInstance{
        get{
            return sharedInstance ??= new SkillList();
        }
    }

    private SkillList(){
        AllSkills = new Dictionary<int, Skill>
        {
            // 0
            {
                0,
                new ProjectileSkill
                {
                    ID = 0,
                    EffectPrefabPath = "Assets/Prefab/FireBall",
                    BaseDamage = 10,
                    PercentDamage = 1.15f,
                    LifeTime = 5f,
                    Cooldown = 3f,
                    Force = 10f,
                    Action = (EffectObject, Direction, Force) =>
                    {
                        EffectObject.GetComponent<Rigidbody>().AddForce(Direction * Force, ForceMode.Impulse);
                    },
                    TriggerEntered = (GameObject, Collider) => {
                        UnityEngine.Object.Destroy(GameObject);
                    }
                }
            },
            // 1
            
        };

    }
    public Skill Get(int ID){
        return AllSkills[ID];
    }

    private static Dictionary<int, Skill> AllSkills;
}