using System.Collections;
using Unity.Collections;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [Header("Debug Menu")]
    [SerializeField] bool respawnCharacter = false;
    [SerializeField] bool switchWeapon = false;

    public static PlayerManager instance;
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;




    public FixedString64Bytes characterName = "Character";

    protected override void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        base.Awake();

        //Do MORE STUFF, ONLY FOR THE PLAYER
        playerManager = GetComponent<PlayerManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();






        // Do not load save data here - other managers (SaveDataManager, UI managers etc.) may not have
        // run their Awake yet. Defer loading to Start which runs after all Awake calls.



    }

    protected override void Start()
    {
        base.Start();

        // Load save data after all Awake() methods have executed so singletons are initialized.
        LoadGameDataFromSaveDataManager();
    }

    

    protected override void Update()
    {
        base.Update();

        //Handle character movement
        playerLocomotionManager.HandleAllMovement();


        playerStatsManager.HandleStatUpdates();
        //playerStatsManager.RegenerateOverheating();

        CheckHP();

        DebugMenu();

        
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
       


    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        PlayerUIManager.instance.playerUIPopUpManager.SendSignalLostPopUp();

        return base.ProcessDeathEvent(manuallySelectDeathAnimation);


    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.characterName = characterName.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.currentHealth = currentHealth;
        currentCharacterData.currentOverheating = currentOverheating;

        currentCharacterData.durability = durability;
        currentCharacterData.coolant = coolant;
    }

    public void LoadGameDataFromSaveDataManager()
    {
        characterName = SaveDataManager.instance.characterName;
        Vector3 myPosition = SaveDataManager.instance.characterPosition;
        transform.position = myPosition;

        durability = SaveDataManager.instance.durability;
        coolant = SaveDataManager.instance.coolant;


        maxHealth = playerStatsManager.CalculateHealthBasedOnDurabilityLevel(durability);
        PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth);
        currentHealth = SaveDataManager.instance.currentHealth;


        maxOverheating = playerStatsManager.CalculateOverheatingBasedOnCoolantLevel(coolant);
        PlayerUIManager.instance.playerUIHudManager.SetMaxOverheatValue(maxOverheating);
        currentOverheating = SaveDataManager.instance.currentOverheating;

    }

    public void CheckHP()
    {
        if (currentHealth <= 0 && !isDead)
        {
            
            StartCoroutine(ProcessDeathEvent());
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void SetNewMaxHealthValue(int newDurability)
    {
        if (playerStatsManager == null)
        {
            Debug.LogWarning("PlayerStatsManager is null in SetNewMaxHealthValue");
            return;
        }

        maxHealth = playerStatsManager.CalculateHealthBasedOnDurabilityLevel(newDurability);

        if (PlayerUIManager.instance != null && PlayerUIManager.instance.playerUIHudManager != null)
        {
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth);
        }
        else
        {
            Debug.LogWarning("PlayerUIHudManager not available when setting max health");
        }
       // currentHealth = maxHealth; we are updating this live, which breaks this. It shouldn't be an issue.
    }

    public void SetNewMaxOverheatValue(int newCoolant)
    {
        if (playerStatsManager == null)
        {
            Debug.LogWarning("PlayerStatsManager is null in SetNewMaxOverheatValue");
            return;
        }

        maxOverheating = playerStatsManager.CalculateOverheatingBasedOnCoolantLevel(newCoolant);

        if (PlayerUIManager.instance != null && PlayerUIManager.instance.playerUIHudManager != null)
        {
            PlayerUIManager.instance.playerUIHudManager.SetMaxOverheatValue(maxOverheating);
        }
        else
        {
            Debug.LogWarning("PlayerUIHudManager not available when setting max overheat");
        }
        //currentOverheating = 0; we are updating this live, which breaks this. It shouldn't be an issue.
    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        currentHealth = maxHealth;
        currentOverheating = 0;
        isDead = false;

        //Revive Effects

        playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
    }

    private void DebugMenu()
    {
        if (respawnCharacter)
        {
            respawnCharacter = false;
            ReviveCharacter();
        }

        if (switchWeapon)
        {
            switchWeapon = false;
            playerEquipmentManager.SwitchWeapon();
        }
    }

    public void OnWeaponIDChange(int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponItemByID(newID));
        playerInventoryManager.currentWeapon = newWeapon;
        playerEquipmentManager.LoadWeapon();
    }


}
