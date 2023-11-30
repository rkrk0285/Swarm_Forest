using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardShot : Skill
{    
    public override void Activate(ICharacter caster, GameObject Effect, Vector3 direction)
    {
        Vector3 left = Quaternion.Euler(0, -10f, 0) * direction;
        Vector3 right = Quaternion.Euler(0, 10f, 0) * direction;
        
        LaunchSkillEffect(caster, Effect, direction);
        LaunchSkillEffect(caster, Effect, left);
        LaunchSkillEffect(caster, Effect, right);
    }

    private void LaunchSkillEffect(ICharacter caster, GameObject Effect, Vector3 direction)
    {
        var effect = InstantiateEffect(Effect, caster.transform.position);
        effect.transform.rotation = GameManager.instance.playerManager.Prefab.transform.rotation * Quaternion.Euler(0, -90f, 0);
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
