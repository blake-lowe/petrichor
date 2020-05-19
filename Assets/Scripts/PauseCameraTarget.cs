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
    public float maxSqrSeparationDist = 1f;
    public new Camera camera;
    public float cameraDistance = 10f;
    public float angleFudgeFactor = 0.01f;
    public float currentSqrSeparationDist;
    public float dot;
    
    
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
        var screenCenterPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2,
            Screen.height / 2, cameraDistance));
        var vectorToCenter = screenCenterPoint - transform.position;
        if (isPaused)
        {
            currentSqrSeparationDist = vectorToCenter.sqrMagnitude;
            dot = Vector3.Dot(movement.normalized, vectorToCenter.normalized);
            if (vectorToCenter.sqrMagnitude < maxSqrSeparationDist)
            {
                transform.Translate(movement * Time.unscaledDeltaTime);
            }
            else
            {
                if (!(Vector3.Dot(movement.normalized, vectorToCenter.normalized) < -angleFudgeFactor))
                {
                    transform.Translate(movement * Time.unscaledDeltaTime);
                }
                else if (!(Vector3.Dot((Vector3.right*movement.x).normalized, vectorToCenter.normalized) < -angleFudgeFactor))
                {
                    transform.Translate(Vector3.right * (movement.x * Time.unscaledDeltaTime));
                }
                else if (!(Vector3.Dot((Vector3.up*movement.y).normalized, vectorToCenter.normalized) < -angleFudgeFactor))
                {
                    transform.Translate(Vector3.up * (movement.y * Time.unscaledDeltaTime));
                }
            }
            
            
        }
        else
        {
            transform.SetPositionAndRotation(playerPosition.position, Quaternion.identity);
        }
    }
}
