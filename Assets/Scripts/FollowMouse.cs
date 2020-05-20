using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float moveSpeed;
    public Camera mainCamera;
    public bool isUsingScreenSpace;
    
    private Vector3 _mousePosition;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isUsingScreenSpace)
        {
            _mousePosition = Input.mousePosition;
            transform.SetPositionAndRotation(_mousePosition, Quaternion.identity);
        }
        else
        {
            _mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var targetPosition = new Vector3(_mousePosition.x, _mousePosition.y, transform.position.z);
            transform.SetPositionAndRotation(targetPosition, Quaternion.identity);
        }
        
    }
}
