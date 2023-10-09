using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : GeneralController
{
    private SceneController _sceneController;
    private CameraScript _cameraScript;
    private UIController _uiController;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _uiController = GameObject.Find("UIController").GetComponent<UIController>();
        _sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
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
            if (CanMove == false || _character.Health <= 0)
            {
                return;
            }
            ProcessPlayerMovement();
        }
        if (Input.GetMouseButton(1))
        {
            if (CanAttack == false || _character.Health <= 0)
            {
                return;
            }
            ProcessAttack();                
        }
        // if W is pressed, use movement ability
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (CanMove == false || _character.Health <= 0)
            {
                return;
            }
            ProcessMovementAbility();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            _uiController.ToggleInventory();
        }

    }

    private void ProcessMovementAbility()
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
        MovementAbility(target);
    }

    void ProcessAttack()
    {
        RaycastHit mouseHit = _cameraScript.GetCalculatedMouseHitInfo();
        Vector3 target = Vector3.zero;  
        if (mouseHit.point == Vector3.zero)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } else
        {
            target = mouseHit.point;
        }
        Attack(target);
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
