using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PauseCameraTarget : MonoBehaviour
{
    public float speed = 1;
    public bool isPaused = false;
    public Transform playerPosition;
    public CinemachineVirtualCamera pauseVCAM;
    
    
    private Controls _controls;
    private Vector2 _direction = new Vector2(0,0);

    private void Awake()
    {
        _controls = new Controls();
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
    }

    private void Start()
    {
        transform.SetPositionAndRotation(playerPosition.position, Quaternion.identity);
    }
    
    private void Update()
    {
        _direction = _controls.Player.direction.ReadValue<Vector2>();
        var movement = new Vector3(_direction.x * speed, _direction.y * speed, 0);
        if (isPaused)
        {
            transform.Translate(movement);
        }
        else
        {
            transform.SetPositionAndRotation(playerPosition.position, Quaternion.identity);
        }
    }
}
