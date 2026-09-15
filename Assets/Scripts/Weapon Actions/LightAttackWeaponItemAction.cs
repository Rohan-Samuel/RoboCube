using UnityEngine;


[CreateAssetMenu(menuName = "Character Actions/Weapon Item Action/ Light Attack Action")]
public class LightAttackWeaponItemAction : WeaponItemAction
{

    public override void PerformAction(WeaponItem weaponItem, PlayerManager player)
    {
        base.PerformAction(weaponItem, player);

        //Check for stops

        if (player.currentOverheating >= player.maxOverheating)
            return;

        PerformLightAttack(weaponItem, player);
    }

    private void PerformLightAttack(WeaponItem weaponItem, PlayerManager player)
    {
        Vector3 origin = player.playerEquipmentManager.weaponManager.muzzle.position;
        Vector3 target = player.playerEquipmentManager.weaponManager.firingDirectionTarget.position;

        Vector3 forward = (target - origin).normalized;

        //Debug.Log("Origin: " + origin + " Forward: " + forward);
        //Debug.DrawRay(origin, forward * 50f, Color.red, 1f);

        player.playerEquipmentManager.weaponManager.Shoot(Quaternion.LookRotation(forward));

        player.playerManager.currentOverheating += weaponItem.baseStaminaCost;
    }
}
