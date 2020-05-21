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
    public int bulletsPerMagazine;
    public int totalBullets;
    public int currentBullets = 0;

    private void OnEnable()
    {
        var sh = ps.shape;
        sh.angle = spread;
        var cycleCount = isFullAuto ? 0 : 1;//0 is infinite
        ps.emission.SetBurst(0, new ParticleSystem.Burst(0, bulletsPerShot, cycleCount, 1/fireRate));
        var ma = ps.main;
        ma.loop = isFullAuto;
    }

    public void ReduceCurrentBullets(int numBullets)
    {
        currentBullets -= numBullets;
    }

}
