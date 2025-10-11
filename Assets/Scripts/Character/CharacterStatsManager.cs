using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    PlayerManager player;

    [Header("Overheat Regeneration")]
    [SerializeField] float overheatRegenerationAmount = 2;
    private float overheatRegenerationTimer = 0;
    private float overheatTickTimer;
    [SerializeField] float overheatRegenerationDelay = 2;

    private float previousOverheatValue;
    private float currentOverheatValue;

    private float previousHealthValue;
    private float currentHealthValue;

    public float OverheatChange
    {
        get { return currentOverheatValue; }

        set
        {
            previousOverheatValue = currentOverheatValue;
            currentOverheatValue = value;
        }

    }

    protected virtual void Start()
    {

    }


    protected virtual void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public int CalculateHealthBasedOnDurabilityLevel(int durability)
    {
        float health = 0;

        //equation for how we want health to be calculated

        health = durability * 10;

        return Mathf.RoundToInt(health);
    }

    public int CalculateOverheatingBasedOnCoolantLevel(int coolant)
    {
        float overheating = 0;

        //equation for how we want overheating to be calculated

        overheating = coolant * 10;

        return Mathf.RoundToInt(overheating);
    }


    public virtual void HandleStatUpdates()
    {
        //Update total amount of resources when stat changes
        player.SetNewMaxHealthValue(player.durability);
        player.SetNewMaxOverheatValue(player.coolant);
        //PlayerManager.instance.maxHealth = CalculateHealthBasedOnDurabilityLevel(player.durability);
        //PlayerManager.instance.maxOverheating = CalculateOverheatingBasedOnCoolantLevel(player.coolant);

        //Updates UI Stat bars when stat changes
        PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(player.currentHealth);
        PlayerUIManager.instance.playerUIHudManager.SetNewOverheatValue(player.currentOverheating);

        RegenerateOverheating();
        ResetOverheatRegenTimer();
    }

    public virtual void RegenerateOverheating()
    {
        if (player.isSprinting)
            return;

        if (player.isPerformingAction)
            return;

        overheatRegenerationTimer += Time.deltaTime;
        
        if (overheatRegenerationTimer >= overheatRegenerationDelay)
        {
            if (player.currentOverheating > 0)
            {
                overheatTickTimer += Time.deltaTime;

                if (overheatTickTimer >= 0.1)
                {
                    overheatTickTimer = 0;
                    player.currentOverheating -= overheatRegenerationAmount;
                }
            }
        }
    }

    public virtual void ResetOverheatRegenTimer()
    {
        OverheatChange = player.currentOverheating;
        if (currentOverheatValue > previousOverheatValue)
        {
            overheatRegenerationTimer = 0;
        }
    }



   
}
