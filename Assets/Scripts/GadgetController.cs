using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetController : MonoBehaviour
{
    public Gadget gadget;
    private Vector3 _startPos;
    public Vector3 targetPos;
    public float throwForce = 7;
    private Rigidbody2D _rb;//kinematic
    private float _startTime;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
        _startTime = Time.time;
    }
    
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(_startPos, targetPos, F(Time.time - _startTime));
    }

    public void ActivateAbility()
    {
        switch (gadget.name)
        {
            case "Bubble":
                Debug.Log("pop");
                break;
            case "Flashbang":
                Debug.Log("flash...bang");
                break;
        }
        Destroy(gameObject);
    }

    private float F(float x)//logistic function
    {
        return (2 / (1 + Mathf.Exp(-throwForce * x))) - 1;
    }
}
