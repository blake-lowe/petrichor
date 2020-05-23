using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItemRenderer : MonoBehaviour
{
    public SpriteRenderer sr;
    public Weapon weapon;
    
    private void OnEnable()
    {
        if (sr == null)
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
        }

        if (sr == null)
        {
            Debug.Log("Sprite renderer not set", gameObject);
        }
    }

    private void Start()
    {
        sr.sprite = weapon.spriteGround;
    }

    
}
