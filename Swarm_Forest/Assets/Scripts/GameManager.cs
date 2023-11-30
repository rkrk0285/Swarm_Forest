using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public InputManager inputManager;
    public PlayerManager playerManager;
    public SkillManager skillManager;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void add_Skill(int num)
    {

    }
}
