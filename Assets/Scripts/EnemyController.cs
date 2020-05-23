using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
public class EnemyController : MonoBehaviour
{
    [SerializeField] private FieldOfView prefabFieldOfView;
    private FieldOfView fieldOfView;

    public Animator animator;
    public ParticleSystem hitPS;
    public AIPath aiPath;
    private GameObject[] noiseObjects;
    private Vector2 _facing;

    private void Start()
    {
        noiseObjects = GameObject.FindGameObjectsWithTag("NoiseObject");
        fieldOfView = Instantiate(prefabFieldOfView, null).GetComponent<FieldOfView>();
    }
    
    private void Update()
    {
        GameObject gameObject;
        Vector2 vectorDifference;
        float noiseLevel;
        GetComponent<AIDestinationSetter>().target = null;
        for (int i = 0; i < noiseObjects.Length; i++)
        {
            gameObject = noiseObjects[i];
            Vector2 enemyVector = new Vector2(transform.position.x, transform.position.y);
            Vector2 objectVector = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            vectorDifference = enemyVector - objectVector;
            noiseLevel = gameObject.GetComponent<NoiseSource>().noiseLevel;

            if (vectorDifference.magnitude < noiseLevel)
            {
                GetComponent<AIDestinationSetter>().target = gameObject.transform;
            }
        }


        //set animator parameters and facing
        var xVel = aiPath.velocity.x;
        var yVel = aiPath.velocity.y;
        if (aiPath.velocity.sqrMagnitude > 0.001)
        {
            animator.SetBool("isMoving", true);
            _facing.x = aiPath.velocity.normalized.x;
            _facing.y = aiPath.velocity.normalized.y;
        }else
        {
            animator.SetBool("isMoving", false);
        }
        animator.SetFloat("Horizontal", _facing.x);
        animator.SetFloat("Vertical", _facing.y);
        
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(new Vector3(_facing.x, _facing.y, 0));
    }

    private void OnParticleCollision(GameObject other)
    {
        WeaponInfo damageInfo = other.GetComponent<WeaponInfo>();
        var damage = 0f;
        if (damageInfo != null)
        {
            damage = damageInfo.damage;
        }
        Debug.Log(damage + " damage taken by " + this.gameObject.name);
        if (hitPS != null)
        {
            var ma = hitPS.main;
            ma.startLifetime = new ParticleSystem.MinMaxCurve(0.07f*damage, 0.2f*damage);
            hitPS.Play();
        }
    }
}
