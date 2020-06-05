using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    public ParticleSystem ps;
    public string gunName;
    public float damage;
    public bool isFullAuto;
    public float fireRate;
    public float spread;
    public int bulletsPerShot;
    public int totalBullets;
    public int currentBullets = 0;
    public int noiseLevel = 0;
    public float noiseDuration = 1;
    public bool skipInitialization;
    private int _lastCurrentBullets;

    public List<ParticleCollisionEvent> collisionEvents;

    private void OnEnable()//set relevant shader parameters
    {
        if (!skipInitialization)
        {
            var sh = ps.shape;
            sh.angle = spread;
            ps.emission.SetBurst(0, new ParticleSystem.Burst(0, bulletsPerShot, 1, 1 / fireRate));
            var ma = ps.main;
            ma.loop = false;
        }
    }

    private void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }
}
