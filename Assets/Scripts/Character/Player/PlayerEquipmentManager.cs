using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    public WeaponModelInstantiationSlot topSlot;

    public GameObject weaponModel;

    override protected void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();

        InitializeWeaponSlot();
    }

    protected override void Start()
    {
        base.Start();
        
        LoadWeapon();
    }

    private void InitializeWeaponSlot()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.Top)
            {
                topSlot = weaponSlot;
            }
        }
    }

    public void LoadWeapon()
    {
        if (player.playerInventoryManager.currentWeapon != null)
        {
            weaponModel = Instantiate(player.playerInventoryManager.currentWeapon.weaponModel);
            topSlot.LoadWeapon(weaponModel);
        }
    }
}
