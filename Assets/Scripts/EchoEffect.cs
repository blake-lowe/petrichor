using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    public GameObject prefab;
    private float _nextSpawnTime;
    public float spawnRate;
    public SpriteRenderer spriteSource;
    void OnEnable()
    {
        _nextSpawnTime = Time.time;
    }
    
    void FixedUpdate()
    {
        if (Time.time > _nextSpawnTime)
        {
            var go = Instantiate(prefab);
            go.GetComponent<SpriteRenderer>().sprite = spriteSource.sprite;
            go.transform.position = transform.position;
            _nextSpawnTime = Time.time + (1f / spawnRate);
        }
    }
}
