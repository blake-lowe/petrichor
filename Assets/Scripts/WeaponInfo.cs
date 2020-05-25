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

    private int _lastCurrentBullets;

    private void OnEnable()//set relevant shader parameters
    {
        var sh = ps.shape;
        sh.angle = spread;
        ps.emission.SetBurst(0, new ParticleSystem.Burst(0, bulletsPerShot, 1, 1 / fireRate));
        var ma = ps.main;
        ma.loop = false;
    }
}
