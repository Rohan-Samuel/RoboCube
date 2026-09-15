using UnityEngine;


[CreateAssetMenu(menuName = "Character Actions/Weapon Item Action/ Test Action")]
public class WeaponItemAction : ScriptableObject
{
  // public int ActionID;

    public virtual void PerformAction(WeaponItem weaponItem, PlayerManager player)
    {
        Debug.Log("Weapon being Used: " + weaponItem.itemName);




    }
}
