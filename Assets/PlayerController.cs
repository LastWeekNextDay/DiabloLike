using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : GeneralController
{
    private SceneController _sceneController;
    private CameraScript _cameraScript;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
    }

    void ProcessAttack()
    {
        RaycastHit mouseHit = _cameraScript.GetCalculatedMouseHitInfo();
        Vector3 direction = Vector3.zero;  
        if (mouseHit.point == Vector3.zero)
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } else
        {
            direction = mouseHit.point;
        }
        Attack(direction);
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
