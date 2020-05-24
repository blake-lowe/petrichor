using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Weapon")]
public class Weapon : Item
{
    public Sprite spriteUi;
    public Sprite spriteGround;
    public Sprite spriteSide;
    public Sprite spriteTop;
    public GameObject weaponPrefab;
    public WeaponInfo enemyWeaponInfo;
}
