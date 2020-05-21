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

    private void OnAwake()
    {
        var sm = ps.shape;
        sm.angle = spread;
        ps.emission.SetBurst(0, new ParticleSystem.Burst(0, bulletsPerShot, 1, 1/fireRate));
        var m = ps.main;
        m.loop = isFullAuto;
    }
    
}
