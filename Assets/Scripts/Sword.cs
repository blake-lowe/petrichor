using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sword", menuName = "Item/Sword")]
public class Sword : Item
{
    public Sprite spriteUi;
    public Sprite spriteGround;
    public Sprite spriteSide;
    public Sprite spriteTop;
    
    public float reach;
    public float arc;
    public float swingTime;
}
