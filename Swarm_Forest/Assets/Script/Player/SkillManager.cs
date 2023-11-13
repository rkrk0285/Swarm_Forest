using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{    
    public GameObject fireball_Prefab;
    public GameObject Player;  
    public float skillForce = 10f;    
    public void cast_Fireball()
    {
        GameObject skill = Instantiate(fireball_Prefab, Player.transform.position, Quaternion.identity, gameObject.transform);
        Rigidbody skillRigidbody = skill.GetComponent<Rigidbody>();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 기울인 카메라 보정.
        mousePos.z += (Camera.main.transform.position.y / (Mathf.Sin(70 * Mathf.Deg2Rad) / Mathf.Cos(70 * Mathf.Deg2Rad))); 
        Vector3 dirVec = mousePos - Player.transform.position;
        

        Debug.Log("MouPos " + mousePos);
        Debug.Log("Player " + Player.transform.position);

        dirVec.y = 0f;        
        dirVec = dirVec.normalized;
        Debug.Log("DirVec " + dirVec);
        if (skillRigidbody != null)
        {
            skillRigidbody.AddForce(dirVec * skillForce, ForceMode.Impulse);
        }
        Destroy(skill, 5f);
    }
}
