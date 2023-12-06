using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class PlayerManager : ICharacter
{    
    public Sprite[] SkillIcon;
    public Camera player_Camera;            
    private bool autoMove = false;    
    private Vector3 destination;
    private Vector3 offset = new Vector3(0, 0, 120 / (Mathf.Sin(50 * Mathf.Deg2Rad) / Mathf.Cos(50 * Mathf.Deg2Rad)));
    private GameObject objectManager;
    private void Start()
    {                     
        Prefab = this.gameObject;
        AttackPoint = 5;
        MaxHealthPoint = 500;
        HealthPoint = 500;
        gameNetworkingManager = GameObject.Find("GameManager").GetComponent<GameNetworkingManager>().gameObject;
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>().gameObject;

        // 조작하는 플레이어 캐릭터인 경우, 카메라 할당.
        if (objectManager.GetComponent<ObjectManager>().current_PlayerID == ID)
        {
            player_Camera = Camera.main;
            player_Camera.transform.position = new Vector3(transform.position.x, 120, transform.position.z) - offset;
            GameObject.Find("GameManager").GetComponent<InputManager>().Player = this.gameObject;
            GameObject.Find("GameManager").GetComponent<InputManager>().playerManager = this;
            GameObject.Find("GameManager").GetComponent<GameManager>().playerManager = this;
        }

        AddSkill(KeyCode.Q, 100);
        AddSkill(KeyCode.W, 101);
        AddSkill(KeyCode.E, 103);
        AddSkill(KeyCode.R, 104);
    }
    private void Update()
    {
        if (autoMove)
        {
            this.gameObject.transform.rotation = Quaternion.LookRotation(destination - new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z));
            transform.position = destination;            
            player_Camera.transform.position = new Vector3(transform.position.x, 120, transform.position.z) - offset;            
            autoMove = false;
        }        
    }
    public void movePlayer(Vector3 pos)
    {
        autoMove = true;
        destination = new Vector3(pos.x, 0, pos.z);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {            
            float damage = other.gameObject.GetComponent<ICharacter>().AttackPoint;
            Damaged(AttackPoint, -2);
        }
    }

    //public void switchSkillUI(KeyCode keyCode, int skillID)
    //{
    //    switch (keyCode)
    //    {
    //        case KeyCode.Q:
    //            CenterUI.transform.Find("Skill_Q").GetComponent<Image>().sprite = SkillIcon[skillID];
    //            break;
    //        case KeyCode.W:
    //            CenterUI.transform.Find("Skill_W").GetComponent<Image>().sprite = SkillIcon[skillID];
    //            break;
    //        case KeyCode.E:
    //            CenterUI.transform.Find("Skill_E").GetComponent<Image>().sprite = SkillIcon[skillID];
    //            break;
    //        case KeyCode.R:
    //            CenterUI.transform.Find("Skill_R").GetComponent<Image>().sprite = SkillIcon[skillID];
    //            break;
    //    }
    //}
}
