using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public WeaponItem currentWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] weaponQuickSlots = new WeaponItem[3];
    public int currentWeaponQuickSlotIndex = 0;
}
