using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager player;
   protected override void Awake()
   {
       base.Awake();

       player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();
        //We calculate these values here until we have a character creation system
        CalculateHealthBasedOnDurabilityLevel(player.durability);
        CalculateOverheatingBasedOnCoolantLevel(player.coolant);
        //player.currentHealth = player.maxHealth;

    }
}
