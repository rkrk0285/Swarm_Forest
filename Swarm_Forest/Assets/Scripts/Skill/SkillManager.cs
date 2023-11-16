using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class SkillManager: MonoBehaviour{ 
    void Start(){
        ResetLastCastedTimes();
    }

    // <ID, Last Cast Time>
    private Dictionary<int, float> LastCastedTimes;

    public void Cast(ICharacter caster, Skill skill, Vector3 direction){
        if(!CanCastSkill(skill)) return;
        if(skill is PassiveSkill) return;

        var effect =  
            InstantiateEffect(caster.transform.position, skill);
        
        if(skill is ProjectileSkill projectileSkill){
            projectileSkill.Action(effect, direction, skill.Force);
        }
        if(skill is PressingSkill pressingSkill){
            pressingSkill.Action(effect, direction);
        }
        
        Destroy(effect, skill.LifeTime);
    }

    private bool CanCastSkill(Skill skill){
        if(!LastCastedTimes.ContainsKey(skill.ID)){
            LastCastedTimes.Add(skill.ID, -1);
        }

        var currentTime = Time.time;
        var lastCastedTime = LastCastedTimes[skill.ID];

        if(
            lastCastedTime != -1 && 
            currentTime - lastCastedTime < skill.Cooldown
        ){
            return false;
        }

        LastCastedTimes[skill.ID] = currentTime;

        return true;
    }

    private GameObject InstantiateEffect(Vector3 origin, Skill skill){
        return UnityEngine.Object.Instantiate(
                skill.EffectPrefab, 
                origin, 
                Quaternion.identity,
                gameObject.transform
            );
    }

    public void Reset(){
        DestroyAllChildren();
        ResetLastCastedTimes();
    }

    // For clear
    private void DestroyAllChildren(){
        foreach (Transform child in gameObject.transform){
            Destroy(child.gameObject);
        }
    }

    private void ResetLastCastedTimes(){
        LastCastedTimes = new Dictionary<int, float>();
    }
}