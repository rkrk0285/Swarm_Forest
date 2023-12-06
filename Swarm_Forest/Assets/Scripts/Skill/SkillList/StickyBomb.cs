using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : Skill
{
    bool isTriggered = false;
    public override void Activate(ICharacter caster, GameObject Effect, Vector3 direction)
    {
        gameNetworkingManager = GameObject.Find("GameManager");
        int casterId = gameNetworkingManager.GetComponent<ObjectManager>().current_PlayerID;
        gameNetworkingManager.GetComponent<GameNetworkingManager>().CastSkill(casterId, 102, 1, caster.transform.position, caster.transform.position + direction * 1000);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameNetworkingManager == null)
            gameNetworkingManager = GameObject.Find("GameManager");

        if (isTriggered == true)
            return;

        // 캐릭터이고 본인 캐릭터가 아닌 경우.
        if (other.gameObject.tag == "Character" && other.gameObject.GetComponent<ICharacter>().ID != CasterId)
        {
            // 데미지 계산하는 곳.            
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
                if (CasterId == gameNetworkingManager.GetComponent<ObjectManager>().current_PlayerID)
                {
                    gameNetworkingManager.GetComponent<GameNetworkingManager>().UpdateObjectStatus(ID, -1);
                    isActivated = false;
                }
                this.gameObject.SetActive(false);
            }
            timer += Time.deltaTime;
        }
    }
}
