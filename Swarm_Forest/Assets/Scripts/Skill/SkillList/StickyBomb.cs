using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : Skill
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
