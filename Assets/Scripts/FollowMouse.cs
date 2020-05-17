using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float moveSpeed;
    public Camera camera;
    private Vector3 mousePosition;

    // Update is called once per frame
    void Update()
    {
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        var targetPosition = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        transform.SetPositionAndRotation(targetPosition, Quaternion.identity);
    }
}
