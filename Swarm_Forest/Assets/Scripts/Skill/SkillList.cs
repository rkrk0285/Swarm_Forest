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
        //AllSkills = new Dictionary<int, string>
        //{
        //    {0, "SkillPrefab/FireBall"},
        //};        
    }
    public static GameObject Get(int ID){        
        if (!AllSkills.ContainsKey(ID)) 
            throw new KeyNotFoundException($"There is no skill have ID: {ID}");       
        return Resources.Load<GameObject>(AllSkills[ID]);
    }

    private static Dictionary<int, string> AllSkills = new Dictionary<int, string>{
            {0, "SkillPrefab/FireBall"},
            {1, "SkillPrefab/ThrowingStar"},
            {2, "SkillPrefab/StickyBomb"},
            {3, "SkillPrefab/HyperBeam"},
        };
}