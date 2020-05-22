using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem hitPS;
    private GameObject[] noiseObjects;

    private void Start()
    {
        noiseObjects = GameObject.FindGameObjectsWithTag("NoiseObject");
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
