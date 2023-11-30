using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    public override void Activate(ICharacter caster, GameObject Effect, Vector3 direction)
    {
        var effect = InstantiateEffect(Effect, caster.transform.position);
        effect.GetComponent<Rigidbody>().AddForce(direction * 100f, ForceMode.Impulse);
        
        Destroy(effect, LifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // 데미지 계산하는 곳.
            if (other.gameObject.GetComponent<ICharacter>().Damaged(BaseDamage, ID))
                Destroy(this.gameObject);
        }
    }    
}
