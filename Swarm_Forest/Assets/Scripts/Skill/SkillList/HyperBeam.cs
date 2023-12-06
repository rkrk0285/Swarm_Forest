using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperBeam : Skill
{
    bool isTriggered = false;
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

    private void OnTriggerStay(Collider other)
    {        
        if (other.gameObject.tag == "Character" && other.gameObject.GetComponent<ICharacter>().ID != CasterId)
        {
            // 데미지 계산하는 곳.                        
            other.gameObject.GetComponent<ICharacter>().Damaged(BaseDamage, ID);
        }
    }

    private void ChangeSkillPosition()
    {
        Transform player_transform = GameManager.instance.playerManager.Prefab.transform;
        this.gameObject.transform.position = new Vector3 (player_transform.position.x, 5f, player_transform.position.z);        
        this.gameObject.transform.rotation = player_transform.rotation;
    }
}
