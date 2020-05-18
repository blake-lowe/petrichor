using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDrag : MonoBehaviour
{
    public Rigidbody2D rb;
    public float minimumSpeed;
    public float dragConstant = 0.25f;

    void FixedUpdate()
    {
        rb.AddForce(-dragConstant * rb.mass * rb.velocity, ForceMode2D.Impulse);
        if (rb.velocity.sqrMagnitude < minimumSpeed * minimumSpeed)
        {
            rb.AddForce(-rb.mass * rb.velocity, ForceMode2D.Impulse);
        }
    }
}