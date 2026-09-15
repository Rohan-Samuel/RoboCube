using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase Instance;

    [SerializeField] public WeaponItem unarmedWeapon;

    [Header("Weapon Items")]
    [SerializeField] List<WeaponItem> weaponItems = new List<WeaponItem>();


    [Header("Items")]
    private List <Item> items = new List<Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var weapon in weaponItems)
        {
            items.Add(weapon);
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].itemID = i;
        }
    }

    public WeaponItem GetWeaponItemByID(int id)
    {
        return weaponItems.FirstOrDefault(weapon => weapon.itemID == id);
    }
}
