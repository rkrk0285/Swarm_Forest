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

        Vector3 mousePos = mouseToRay();
        // 맵 밖을 향할 경우. (임시)
        if (mousePos == Vector3.zero)
            return;

        // 기울인 카메라 보정.        
        Vector3 dirVec = mousePos - Player.transform.position;       
        dirVec.y = 1f;        
        dirVec = dirVec.normalized;
        
        if (skillRigidbody != null)
        {
            skillRigidbody.AddForce(dirVec * skillForce, ForceMode.Impulse);
        }
        Destroy(skill, 5f);
    }

    public Vector3 mouseToRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
