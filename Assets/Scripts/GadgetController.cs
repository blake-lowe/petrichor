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
    public bool doesSpin = true;
    private Rigidbody2D _rb;//kinematic
    private float _startTime;
    public ParticleSystem flashbangPS;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
        _startTime = Time.time;
    }
    
    private void FixedUpdate()
    {
        var y = F(Time.time - _startTime);
        transform.position = Vector3.Lerp(_startPos, targetPos, y);
        if (doesSpin && y < 0.99f)
        {
            var spinSpeed = 5;
            var eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z - spinSpeed);
        }
    }

    public void ActivateAbility()
    {
        switch (gadget.name)
        {
            case "Bubble":
                Debug.Log("pop");
                break;
            case "Flashbang":
                var numCasts = 128;
                var stunDuration = 3;
                for (var i = 0; i < numCasts; i++)
                {
                    var angle = ((float)i / (float)numCasts) * 2f * Mathf.PI;
                    var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    RaycastHit2D ray = Physics2D.Raycast(transform.position, direction);
                    var enemyController = ray.collider.gameObject.GetComponent<EnemyController>();
                    if (enemyController)
                    {
                        Debug.Log(enemyController.name);
                        enemyController.Stun(stunDuration);
                    }
                }
                break;
        }
        Destroy(gameObject);
    }

    private float F(float x)//logistic function
    {
        return (2 / (1 + Mathf.Exp(-throwForce * x))) - 1;
    }
}
