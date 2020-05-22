using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Weapon")]
public class Weapon : Item
{
    public Sprite spriteUI;
    public Sprite spriteGround;
    public Sprite spriteSide;
    public Sprite spriteTop;
    public ParticleSystem bullets;
    public WeaponInfo weaponInfo;
    public WeaponInfo enemyWeaponInfo;
}
