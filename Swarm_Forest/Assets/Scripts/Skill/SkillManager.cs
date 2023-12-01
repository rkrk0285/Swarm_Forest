using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager: MonoBehaviour{    
    void Start(){
        ResetLastCastedTimes();        
    }

    // <ID, Last Cast Time>
    private Dictionary<int, float> LastCastedTimes;    
    public void Cast(ICharacter caster, int skillNum, Vector3 direction)
    {        
        GameObject skillObject = SkillList.Get(skillNum);   
        
        if (!skillObject.TryGetComponent<Skill>(out var skill)) return;        
        if (!CanCastSkill(skill)) return;
           
        skill.Activate(caster, skillObject, direction);
    }

    private bool CanCastSkill(Skill skill){
        if(!LastCastedTimes.ContainsKey(skill.ID)){
            LastCastedTimes.Add(skill.ID, -1);
        }

        var currentTime = Time.time;
        var lastCastedTime = LastCastedTimes[skill.ID];

        if(lastCastedTime != -1 && currentTime - lastCastedTime < skill.Cooldown)
        {
            return false;
        }

        LastCastedTimes[skill.ID] = currentTime;
        return true;
    }

    private GameObject InstantiateEffect(GameObject skillObject, Vector3 origin){
        return Instantiate(
                skillObject, 
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