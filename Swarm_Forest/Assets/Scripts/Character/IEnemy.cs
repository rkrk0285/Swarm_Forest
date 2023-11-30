using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnemy : ICharacter
{
    private Dictionary<int, float> LastHitTimes = new Dictionary<int, float>();
    public void Damaged(float damagedHP, int skillID)
    {
        if (!CanHit(skillID))
            return;
        
        HealthPoint -= damagedHP;        
        if (HealthPoint <= 0)
            DieCharacter();

        Debug.Log(HealthPoint);
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
}
