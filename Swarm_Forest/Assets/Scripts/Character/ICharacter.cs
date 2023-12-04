using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICharacter: MonoBehaviour{
    public int ID;
    public int Type { get; set; }
    public GameObject Prefab;
    
    public int AttackPoint{get;set;}
    public int MaxHealthPoint{get;set;}
    public int HealthPoint{get;set;}
    public float MovementVelocity{get; set;}

    //private event Action<int> PassiveSkillEffect;    
    public GameObject gameNetworkingManager;

    [SerializeField]
    private Animator Ani;
    private Dictionary<int, float> LastHitTimes = new Dictionary<int, float>();
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
    
    public bool Damaged(int damage, int skillID)
    {
        if (!CanHit(skillID))
            return false;

        // 히트 액션 호출.
        Ani.SetTrigger("HitTrigger");

        // 상태 변경 요청.        
        gameNetworkingManager.GetComponent<GameNetworkingManager>().UpdateObjectStatus(ID, HealthPoint - damage);
        return true;
    }

    private bool CanHit(int ID)
    {
        if (!LastHitTimes.ContainsKey(ID))
            LastHitTimes.Add(ID, -1);

        var currentTime = Time.time;
        var lastHitTime = LastHitTimes[ID];

        if (lastHitTime != -1 && currentTime - lastHitTime < 0.3f)
            return false;

        LastHitTimes[ID] = currentTime;
        return true;
    }

    void Start(){        
    }
}