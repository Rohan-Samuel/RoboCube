using System.Collections;
using Unity.Collections;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [Header("Debug Menu")]
    [SerializeField] bool respawnCharacter = false;

    public static PlayerManager instance;
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;

    

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

        //maxHealth = playerStatsManager.CalculateHealthBasedOnDurabilityLevel(durability);
        //currentHealth = maxHealth;
        //PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth);

        //maxOverheating = playerStatsManager.CalculateOverheatingBasedOnCoolantLevel(coolant);
        //currentOverheating = 0;
        //PlayerUIManager.instance.playerUIHudManager.SetMaxOverheatValue(maxOverheating);



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
        maxHealth = playerStatsManager.CalculateHealthBasedOnDurabilityLevel(newDurability);
        PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth);
       // currentHealth = maxHealth; we are updating this live, which breaks this. It shouldn't be an issue.
    }

    public void SetNewMaxOverheatValue(int newCoolant)
    {
        maxOverheating = playerStatsManager.CalculateOverheatingBasedOnCoolantLevel(newCoolant);
        PlayerUIManager.instance.playerUIHudManager.SetMaxOverheatValue(maxOverheating);
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
    }
}
