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
    private Vector3 offset = new Vector3(0, 0, 80 / (Mathf.Sin(50 * Mathf.Deg2Rad) / Mathf.Cos(50 * Mathf.Deg2Rad)));    
    private void Start()
    {             
        Type = 0;
        Prefab = this.gameObject;
        AttackPoint = 5;
        MaxHealthPoint = 50010;
        HealthPoint = 50010;
        gameNetworkingManager = GameObject.Find("GameManager").GetComponent<GameNetworkingManager>().gameObject;
    }
    private void Update()
    {
        if (autoMove)
        {            
            transform.position = destination;
            player_Camera.transform.position = new Vector3(transform.position.x, 80, transform.position.z) - offset;
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
        if (other.gameObject.tag == "Enemy")
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
