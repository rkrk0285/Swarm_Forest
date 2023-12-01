using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class PlayerManager : ICharacter
{
    [SerializeField]
    private Slider HP_Slider;
    [SerializeField]
    private GameObject CenterUI;

    public Sprite[] SkillIcon;
    public Camera player_Camera;        
    
    private bool autoMove = false;    
    private Vector3 destination;
    private Vector3 offset = new Vector3(0, 0, 80 / (Mathf.Sin(50 * Mathf.Deg2Rad) / Mathf.Cos(50 * Mathf.Deg2Rad)));
    private NavMeshAgent nvAgent;    
    private void Start()
    {
        nvAgent = GetComponent<NavMeshAgent>();
        ID = 0;
        Prefab = this.gameObject;
        AttackPoint = 5f;
        MaxHealthPoint = 500f;
        HealthPoint = 500f;
        //MovementVelocity = 10f;
    }
    private void Update()
    {
        if (autoMove)
        {            
            nvAgent.SetDestination(destination);
            player_Camera.transform.position = new Vector3(transform.position.x, 80, transform.position.z) - offset;
        }
        HP_Slider.value = HealthPoint / MaxHealthPoint;
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
            Debug.Log("적 플 충돌");
            float damage = other.gameObject.GetComponent<ICharacter>().AttackPoint;
            Damaged(AttackPoint, -2);
        }
    }

    public void switchSkillUI(KeyCode keyCode, int skillID)
    {
        switch (keyCode)
        {
            case KeyCode.Q:
                CenterUI.transform.Find("Skill_Q").GetComponent<Image>().sprite = SkillIcon[skillID];
                break;
            case KeyCode.W:
                CenterUI.transform.Find("Skill_W").GetComponent<Image>().sprite = SkillIcon[skillID];
                break;
            case KeyCode.E:
                CenterUI.transform.Find("Skill_E").GetComponent<Image>().sprite = SkillIcon[skillID];
                break;
            case KeyCode.R:
                CenterUI.transform.Find("Skill_R").GetComponent<Image>().sprite = SkillIcon[skillID];
                break;
        }
    }
}
