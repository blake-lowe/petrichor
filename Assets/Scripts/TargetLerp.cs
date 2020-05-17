using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLerp : MonoBehaviour
{
    public Transform transform1;
    public Transform transform2;
    public float weight;
    void Update()
    {
        transform.SetPositionAndRotation(Vector3.Lerp(transform1.position, transform2.position, weight), 
            Quaternion.identity);
    }
}
