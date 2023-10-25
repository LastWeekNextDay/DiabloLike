using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : GeneralController
{
    private CameraScript _cameraScript;
    private UIController _uiController;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _uiController = GameObject.Find("UIController").GetComponent<UIController>();
        _cameraScript = Camera.main.GetComponent<CameraScript>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        CheckPlayerInput();
    }

    void CheckPlayerInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (CanMove == false)
            {
                return;
            }
            ProcessPlayerMovement();
        }
        if (Input.GetMouseButton(1))
        {
            if (CanAttack == false)
            {
                return;
            }
            ProcessAbility(_character.EquippedAbility.GetComponent<Ability>());                
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (CanMove == false)
            {
                return;
            }
            ProcessAbility(_character.EquippedMovementAbility.GetComponent<Ability>());
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            _uiController.ToggleInventory();
        }
    }

    void ProcessAbility(Ability ability)
    {
        RaycastHit mouseHit = _cameraScript.GetCalculatedMouseHitInfo();
        Vector3 target = Vector3.zero;
        if (mouseHit.point == Vector3.zero)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            target = mouseHit.point;
        }
        Cast(target, ability);
    }

    void ProcessPlayerMovement()
    {
        RaycastHit mouseHit = _cameraScript.GetCalculatedMouseHitInfo();
        if (mouseHit.collider != null)
        {
            Move(mouseHit.point);
        }
    }
}
