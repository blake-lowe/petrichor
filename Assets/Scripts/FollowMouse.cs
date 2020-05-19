using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float moveSpeed;
    public Camera mainCamera;
    private Vector3 _mousePosition;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        _mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var targetPosition = new Vector3(_mousePosition.x, _mousePosition.y, transform.position.z);
        transform.SetPositionAndRotation(targetPosition, Quaternion.identity);
    }
}
