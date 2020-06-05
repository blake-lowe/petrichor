using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GadgetController : MonoBehaviour
{
    public Gadget gadget;
    private Vector3 _startPos;
    public Vector3 targetPos;
    public float throwForce = 7;
    public bool doesSpin = true;
    private Rigidbody2D _rb;//kinematic
    private float _startTime;
    private float _destroyTime;
    private bool _shouldDestroy;
    public ParticleSystem c4ps;
    public Light2D flashbangLight;
    private bool _flashActive;
    public float flashIntensity = 10;
    
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
        if (doesSpin)
        {
            var eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 360 * 3 * y);
        }
    }

    public void ActivateAbility()
    {
        switch (gadget.name)
        {
            case "Bubble":
                Debug.Log("pop");
                destroyInSeconds(0.25f);
                break;
            case "C-4":
                //vfx
                flashbangLight.enabled = true;
                flashbangLight.intensity = flashIntensity;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //emit damaging particles
                c4ps.Play();
                destroyInSeconds(1f);
                break;
            case "Flashbang":
                //vfx
                flashbangLight.enabled = true;
                flashbangLight.intensity = flashIntensity;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //stun enemies
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
                destroyInSeconds(0.25f);
                break;
        }
        
    }

    private void destroyInSeconds(float seconds)
    {
        _destroyTime = Time.time + seconds;
        _shouldDestroy = true;
    }

    private void Update()
    {
        var currentTime = Time.time;
        
        if (currentTime > _destroyTime && _shouldDestroy)
        {
            Destroy(gameObject);
        }

        switch (gadget.name)
        {
            case "Bubble":
                Debug.Log("pop");
                break;
            case "C-4":
                //falls through
            case "Flashbang":
                if (_shouldDestroy)
                {
                    flashbangLight.intensity = flashIntensity * (F(_destroyTime - currentTime));
                }
                break;
        }
    }

    private float F(float x)//logistic function
    {
        return (2 / (1 + Mathf.Exp(-throwForce * x))) - 1;
    }
}
