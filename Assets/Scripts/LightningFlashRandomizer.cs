using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class LightningFlashRandomizer : MonoBehaviour
{
    public Light2D lightningLight;

    public float offIntensity = 0;
    public float onIntensity = 1;

    public int minNumFlashes = 1;
    public int maxNumFlashes = 4;

    
    public float flashOnDuration = 0.1f;

    public float minTimeBetweenFlashes = 0.2f;
    public float maxTimeBetweenFlashes = 0.6f;
    
    public float minTimeBetweenFlashGroups = 3;
    public float maxTimeBetweenFlashGroups = 7;

    private float _nextFlashTime;
    private float _flashOffTime;
    private float _nextFlashGroupTime;
    private int _numFlashesToDo = 0;
    private void OnEnable()
    {
        lightningLight.intensity = offIntensity;
    }
    
    void FixedUpdate()
    {
        var currentTime = Time.time;
        if (_numFlashesToDo == 0)
        {
            _nextFlashGroupTime = Time.time + Random.Range(minTimeBetweenFlashGroups, maxTimeBetweenFlashGroups);
        }
        else
        {
            if (currentTime > _flashOffTime & currentTime >_nextFlashTime)
            {
                lightningLight.intensity = offIntensity;
                _numFlashesToDo--;
                if (_numFlashesToDo > 0)
                {
                    _nextFlashTime = Time.time + Random.Range(minTimeBetweenFlashes, maxTimeBetweenFlashes);
                }
            }
            if (currentTime > _nextFlashTime)
            {
                lightningLight.intensity = onIntensity;
                _flashOffTime = Time.time + flashOnDuration;
            }
        }

        if (currentTime > _nextFlashGroupTime)
        {
            _numFlashesToDo = Random.Range(minNumFlashes, maxNumFlashes);
        }
    }
}
