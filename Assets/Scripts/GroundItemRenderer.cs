using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItemRenderer : MonoBehaviour
{
    public SpriteRenderer sr;
    public Weapon weapon;
    public AmmoCounter ammoCounter;
    public Gadget gadget;
    public Sword sword;

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
        if (weapon)
        {
            sr.sprite = weapon.spriteGround;
        } 
        else if (gadget)
        {
            sr.sprite = gadget.spriteGround;
        }
        else if (sword)
        {
            sr.sprite = sword.spriteGround;
        }
        
    }

    
}
