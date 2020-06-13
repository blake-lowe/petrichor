using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gadget", menuName = "Item/Gadget")]
public class Gadget : Item
{
    public Sprite spriteUi;
    public Sprite spriteGround;
    public GameObject gadgetPrefab;
    public bool deployable = true;
    public AudioClip audioClip;
}
