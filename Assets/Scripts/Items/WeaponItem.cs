using UnityEngine;


[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon requirements")]
    public int requiredLevel = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;

    [Header("Weapon Base Poise Damage")]
    public float poiseDamage = 10;

    //Weapon Modifiers
    //light attack modifiers
    //heavy attack modifiers

    [Header("Stmaina Costs")]
    public int baseStaminaCost = 10;
    //running attack stamina cost
    //light attack stamina cost
    //heavy attack stamina cost

    //Item based action (r1, r2, l1, l2)



}
