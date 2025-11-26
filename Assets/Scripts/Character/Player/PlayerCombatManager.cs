using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    public WeaponItem currentWeaponBeingUsed;

    override protected void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponItemAction, WeaponItem weaponItem)
    {
        if (weaponItemAction != null && weaponItem != null)
        {
            weaponItemAction.PerformAction(weaponItem, player);
        }
    }
}
