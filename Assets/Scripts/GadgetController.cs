using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GadgetController : MonoBehaviour
{
    public PlayerController playerController;
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
    public float stealthShieldRadius;

    private AudioSource _audioSource;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
        _startTime = Time.time;
        if (!playerController)
        {
            Debug.Log("Err: Player Controller not set in Gadget Controller.");
        }
    }
    
    private void FixedUpdate()
    {
        var y = F(Time.time - _startTime);
        transform.position = Vector3.Lerp(_startPos, targetPos, y);
        if (doesSpin)
        {
            var eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 360 * 3 * (y));
        }
    }

    public void ActivateAbility()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = gadget.audioClip;
        _audioSource.Play();
        switch (gadget.name)
        {
            case "Bubble":
                Debug.Log("pop");
                DestroyInSeconds(0.25f);
                break;
            case "C-4":
                //vfx
                flashbangLight.enabled = true;
                flashbangLight.intensity = flashIntensity;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //emit damaging particles
                c4ps.Play();
                DestroyInSeconds(2f);
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
                        enemyController.Stun(stunDuration);
                    }
                }
                DestroyInSeconds(2f);
                break;
            case "Stealth Shield":
                var shieldDuration = 8f;
                DestroyInSeconds(shieldDuration);
                break;
        }
        
    }

    private void DestroyInSeconds(float seconds)
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
                break;
            case "C-4":
                //falls through
            case "Flashbang":
                if (_shouldDestroy)
                {
                    flashbangLight.intensity = flashIntensity * (F(_destroyTime - 1.75f - currentTime));
                }
                break;
            case "Stealth Shield":
                //calc position
                var playerPos = playerController.transform.position;
                var cursorPos = playerController.cursorPosition.position;
                var direction = (cursorPos - playerPos).normalized;
                var shieldPosition = playerPos + stealthShieldRadius * direction;
                //calc rotation
                var shieldRotation = Quaternion.Euler(new Vector3(0, 0,
                    Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
                transform.SetPositionAndRotation(shieldPosition, shieldRotation);
                if (currentTime > _destroyTime - 1)
                {
                    //flash the shield or something to indicate exhaustion
                }
                break;
        }
    }

    private float F(float x)//logistic function
    {
        return (2 / (1 + Mathf.Exp(-throwForce * x))) - 1;
    }
}
