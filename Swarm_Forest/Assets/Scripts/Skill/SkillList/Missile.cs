using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Skill
{        
    private ICharacter caster;
    private Vector3 direction;
    
    public override void Activate(ICharacter caster, GameObject Effect, Vector3 direction)
    {
        this.caster = caster;
        this.direction = direction;
        
        var effect = InstantiateEffect(Effect, caster.transform.position);
        effect.GetComponent<Rigidbody>().AddForce(direction * 100f, ForceMode.Impulse);        

        Invoke("ActivateSecond", 0.5f);
    }    

    public void ActivateSecond()
    {
        var effect2 = InstantiateEffect(this.gameObject, caster.transform.position);
        effect2.GetComponent<Rigidbody>().AddForce(direction * 100f, ForceMode.Impulse);        
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

    private float timer = 0.0f;
    private void Update()
    {
        if (timer > LifeTime)
        {
            Destroy(this.gameObject);
        }
        timer += Time.deltaTime;
    }
}
