using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Camera _camera;
    private SceneController _sceneController;
    private GameObject _player;
    private bool _resetCamera;
    private int _offsetX = 6;
    private int _offsetY = 10;
    private int _offsetZ = -5;
    private int _offsetRotationX = 45;
    private int _offsetRotationY = -45;
    private int _offsetRotationZ = 0;
    private int _allowedOffsetX = 2;
    private int _allowedOffsetZ = 2;
    private float _cameraDriftSpeed = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        _resetCamera = true;
        _camera = Camera.main;
        _sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CalculateGameplayCamera();
    }

    public RaycastHit GetCalculatedMouseHitInfo()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hitInfo) ? hitInfo : new RaycastHit();
    }


    void CalculateGameplayCamera()
    {
        if (_sceneController.GameplayAllowed == false)
        {
            return;
        }
        AttachCameraToPlayer();
        DriftCameraToPlayer();
    }

    void AttachCameraToPlayer()
    {
        if (_resetCamera == true)
        {
            _camera.transform.position = _player.transform.position;
            _camera.transform.position += new Vector3(_offsetX, _offsetY, _offsetZ);
            _camera.transform.rotation = Quaternion.Euler(_offsetRotationX, _offsetRotationY, _offsetRotationZ);
            _resetCamera = false;
        } else
        {
            if (_camera.transform.position.x - _offsetX - _allowedOffsetX > _player.transform.position.x)
            {
                _camera.transform.position -= new Vector3(_camera.transform.position.x - _offsetX - _allowedOffsetX - _player.transform.position.x, 0, 0);
            }
            if (_camera.transform.position.z - _offsetZ - _allowedOffsetZ > _player.transform.position.z)
            {
                _camera.transform.position -= new Vector3(0, 0, _camera.transform.position.z - _offsetZ - _allowedOffsetZ - _player.transform.position.z);
            }
            if (_camera.transform.position.x - _offsetX + _allowedOffsetX < _player.transform.position.x)
            {
                _camera.transform.position -= new Vector3(_camera.transform.position.x - _offsetX + _allowedOffsetX - _player.transform.position.x, 0, 0);
            }
            if (_camera.transform.position.z - _offsetZ + _allowedOffsetZ < _player.transform.position.z)
            {
                _camera.transform.position -= new Vector3(0, 0, _camera.transform.position.z - _offsetZ + _allowedOffsetZ - _player.transform.position.z);
            }
        }
    }

    void DriftCameraToPlayer()
    {
        float newX = Mathf.MoveTowards(_camera.transform.position.x - _offsetX, _player.transform.position.x, _cameraDriftSpeed);
        float newZ = Mathf.MoveTowards(_camera.transform.position.z - _offsetZ, _player.transform.position.z, _cameraDriftSpeed);
        _camera.transform.position = new Vector3(newX + _offsetX, _camera.transform.position.y, newZ + _offsetZ);
    }

}
