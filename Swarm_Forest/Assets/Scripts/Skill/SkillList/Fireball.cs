using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    bool isTriggered = false;    
    public override void Activate(ICharacter caster, GameObject Effect, Vector3 direction)
    {
        gameNetworkingManager = GameObject.Find("GameManager");
        int casterId = gameNetworkingManager.GetComponent<ObjectManager>().current_PlayerID;
        gameNetworkingManager.GetComponent<GameNetworkingManager>().CastSkill(casterId, 100, 1, caster.transform.position, caster.transform.position + direction * 300);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameNetworkingManager == null)
            gameNetworkingManager = GameObject.Find("GameManager");

        if (isTriggered == true)
            return;

        // ĳ�����̰� ���� ĳ���Ͱ� �ƴ� ���.
        if (other.gameObject.tag == "Character" && other.gameObject.GetComponent<ICharacter>().ID != CasterId)
        {
            // ������ ����ϴ� ��.            
            if (other.gameObject.GetComponent<ICharacter>().Damaged(BaseDamage, ID))
            {                
                isTriggered = true;
                gameNetworkingManager.GetComponent<GameNetworkingManager>().UpdateObjectStatus(ID, 0);
            }
        }
    }

    private float timer = 0.0f;
    private void Update()
    {
        if (isActivated == true)
        {            
            if (timer > LifeTime)
            {                                
                gameNetworkingManager.GetComponent<GameNetworkingManager>().UpdateObjectStatus(ID, -1);
                isActivated = false;
            }
            timer += Time.deltaTime;
        }
    }
}
