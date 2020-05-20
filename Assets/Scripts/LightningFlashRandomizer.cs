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

    public float minFlashOnDuration = 0.05f;
    public float maxflashOnDuration = 0.1f;

    public float minTimeBetweenFlashes = 0.2f;
    public float maxTimeBetweenFlashes = 0.6f;
    
    public float minTimeBetweenFlashGroups = 3;
    public float maxTimeBetweenFlashGroups = 7;

    private bool _isFlashGroupOccurring = false;
    private float _nextFlashTime;
    private float _flashOffTime;
    private bool _isOn = false;
    private float _nextFlashGroupTime;
    private int _numFlashesToDo = 0;
    private void OnEnable()
    {
        lightningLight.intensity = offIntensity;
    }
    
    public void FixedUpdate()
    {
        var currentTime = Time.time;
        if (_isFlashGroupOccurring)
        {
            if (_numFlashesToDo > 0)
            {
                if (!_isOn & currentTime > _nextFlashTime)
                {
                    lightningLight.intensity = onIntensity;
                    _isOn = true;
                    _flashOffTime = Time.time + Random.Range(minFlashOnDuration, maxflashOnDuration);
                } else if (_isOn & currentTime > _flashOffTime)
                {
                    lightningLight.intensity = offIntensity;
                    _isOn = false;
                    _numFlashesToDo--;
                    _nextFlashTime = Time.time + Random.Range(minTimeBetweenFlashes, maxTimeBetweenFlashes);
                }
            }
            else
            {
                _isFlashGroupOccurring = false;
            }
        }
        else
        {
            if (_numFlashesToDo == 0)
            {
                _numFlashesToDo = Random.Range(minNumFlashes, maxNumFlashes);
                _nextFlashGroupTime = Time.time + Random.Range(minTimeBetweenFlashGroups, maxTimeBetweenFlashGroups);
            }
            else
            {
                if (currentTime > _nextFlashGroupTime)
                {
                    _isFlashGroupOccurring = true;
                    _nextFlashTime = Time.time + Random.Range(minTimeBetweenFlashes, maxTimeBetweenFlashes);
                }
            }
        }
        
    }
}
