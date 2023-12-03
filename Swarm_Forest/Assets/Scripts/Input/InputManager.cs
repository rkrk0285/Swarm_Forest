//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InputManager : MonoBehaviour
//{
//    [SerializeField]
//    GameObject Player;
//    [SerializeField]
//    Plane _plane;
//    [SerializeField]
//    private GameObject GameManager;


//    const int LMB = 0, RMB = 1;
//    private bool[] MouseDown;

//    private Action[] MouseFunctions;

//    readonly KeyCode[] keyCodes = {
//        KeyCode.Q,
//        KeyCode.W,
//        KeyCode.E,
//        KeyCode.R
//    };

//    Dictionary<KeyCode, bool> KeyDown;

//    void Start()
//    {
//        InitKeyDown();
//    }

//    void Update()
//    {
//        CheckKeyDown();
//        ProcessInput();
//    }

//    void InitKeyDown()
//    {
//        MouseDown = new bool[2];
//        MouseDown[LMB] = false; MouseDown[RMB] = false;

//        MouseFunctions = new Action[2];
//        MouseFunctions[LMB] = KeyDown_LMB;
//        MouseFunctions[RMB] = KeyDown_RMB;

//        KeyDown = new Dictionary<KeyCode, bool>
//        {
//            { KeyCode.Q, false },
//            { KeyCode.W, false },
//            { KeyCode.E, false },
//            { KeyCode.R, false }
//        };
//    }

//    void CheckKeyDown()
//    {
//        // Check Mouse
//        MouseDown[LMB] = Input.GetMouseButtonDown(LMB);
//        MouseDown[RMB] = Input.GetMouseButtonDown(RMB);

//        // Check Keyboard
//        foreach (var keyCode in keyCodes)
//            KeyDown[keyCode] = Input.GetKeyDown(keyCode);
//    }


//    void ProcessInput()
//    {
//        if (MouseDown[LMB]) MouseFunctions[LMB]();
//        if (MouseDown[RMB]) MouseFunctions[RMB]();
//        foreach (var keyCode in keyCodes)
//            if (KeyDown[keyCode]) KeyFunction(keyCode);
//    }

//    #region Input Callbacks, Change Here!!!
//    void KeyDown_LMB()
//    {

//    }

//    void KeyDown_RMB()
//    {
//        // Make move event
//        // Send to server
//    }

//    void KeyFunction(KeyCode keyCode)
//    {
//        var caster = Player.GetComponent<ICharacter>();
//        var skillObject = caster.Skills[keyCode];

//        if (skillObject == null) return;

//        var mousePositionOnMap = NormalizeRayPoint(MousePositionOnMap());

//        GameManager.GetComponent<SkillManager>().Cast(
//            caster,
//            skillObject,
//            mousePositionOnMap
//        );
//    }
//    #endregion

//    #region Raycasting
//    // Adjust Ray point
//    private Vector3 NormalizeRayPoint(Vector3 rayPoint)
//    {
//        var direction = rayPoint - Player.transform.position;
//        direction.y = 1f;
//        direction = direction.normalized;

//        return direction;
//    }

//    private Vector3 MousePositionOnMap()
//    {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//        if (Physics.Raycast(ray, out var raycastHit))
//        {
//            return raycastHit.point;
//        }

//        return Vector3.zero;
//    }
//    #endregion
//}
