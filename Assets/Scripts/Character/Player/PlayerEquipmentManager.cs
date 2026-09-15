using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    public WeaponModelInstantiationSlot topSlot;

    [SerializeField] public WeaponManager weaponManager;

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
       // firingCenter = GameObject.Find("Firing Center").transform;

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
            //Remove old weapon
            topSlot.UnloadWeapon();

            //Bring in new weapon
            weaponModel = Instantiate(player.playerInventoryManager.currentWeapon.weaponModel);
            weaponManager = weaponModel.GetComponent<WeaponManager>();
            topSlot.LoadWeapon(weaponModel);
           
        }
    }

    public void SwitchWeapon()
    {
        WeaponItem selectedWeapon = null;

        player.playerInventoryManager.currentWeaponQuickSlotIndex += 1;

        if (player.playerInventoryManager.currentWeaponQuickSlotIndex < 0 || player.playerInventoryManager.currentWeaponQuickSlotIndex > 2)
        {
               player.playerInventoryManager.currentWeaponQuickSlotIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponQuickSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponQuickSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponQuickSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.currentWeaponQuickSlotIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                player.playerInventoryManager.currentWeapon = selectedWeapon;
            }
            else
            {
                player.playerInventoryManager.currentWeaponQuickSlotIndex = firstWeaponPosition;
                player.playerInventoryManager.currentWeapon = firstWeapon;
            }
            LoadWeapon();
            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponQuickSlots)
        {
            //IF the next potential weapon is not unarmed, select it
            if (player.playerInventoryManager.weaponQuickSlots[player.playerInventoryManager.currentWeaponQuickSlotIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponQuickSlots[player.playerInventoryManager.currentWeaponQuickSlotIndex];


                player.playerInventoryManager.currentWeapon = player.playerInventoryManager.weaponQuickSlots[player.playerInventoryManager.currentWeaponQuickSlotIndex];
                LoadWeapon();
                return;
            }

            if (selectedWeapon == null && player.playerInventoryManager.currentWeaponQuickSlotIndex <= 2)
            {
                SwitchWeapon();
            }
        }
    } 
}
