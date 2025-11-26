using UnityEngine;


[CreateAssetMenu(menuName = "Items/Weapon Item Action/ Test Action")]
public class WeaponItemAction : ScriptableObject
{
   public int ActionID;

    public virtual void PerformAction(WeaponItem weaponItem, PlayerManager player)
    {
        //Debug.Log("Weapon being Used: " + weaponItem.itemName + " with ActionID: " + ActionID);

        Vector3 origin = GameObject.Find("Center").transform.position;
        Vector3 target = GameObject.Find("Target Direction").transform.position;

        Vector3 forward = (target - origin).normalized;

        Debug.Log("Origin: " + origin + " Forward: " + forward);
        Debug.DrawRay(origin, forward * 50f, Color.red, 1f);


    }
}
