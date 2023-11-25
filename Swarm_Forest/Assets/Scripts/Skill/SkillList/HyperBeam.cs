using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperBeam : Skill
{
    private GameObject effect;    
    private void Update()
    {
        ChangeSkillPosition();
    }
    public override void Activate(ICharacter caster, GameObject Effect, Vector3 direction)
    {        
        effect = InstantiateEffect(Effect, caster.transform.position);
        Destroy(effect, LifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("���ʹ�");
        if (other.gameObject.tag == "Enemy")
        {
            // ������ ����ϴ� ��.
            Debug.Log("���ʹ�");
            //
            Destroy(this.gameObject);
        }
    }

    private void ChangeSkillPosition()
    {
        this.gameObject.transform.position = GameManager.instance.playerManager.Prefab.transform.position;
        this.gameObject.transform.rotation = GameManager.instance.playerManager.Prefab.transform.rotation;
    }
}
