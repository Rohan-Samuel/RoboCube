using UnityEngine;


[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Health Damage")]

public class TakeHealthDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0;
    //Add other damage types (maybe)

    //Build ups

    [Header("Final Damage Dealt")]
    private int finalDamageDealt = 0;  //Damage taken after all calculations are made;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool wollPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;  //Determine which animation to play
    public Vector3 contactPoint;  //Blood/Oil effect spawn point

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        //Do not process any additional effects if character is dead
        if (character.isDead)
            return;

        //Check for i-frames

        //Calculate damage
        CalculateDamage(character);
        //Check which direction damage came from
        //Play animation
        //check for build ups
        //Play damage sound FX
        //Play Damage VFX

        //IF character is AI, check for new target (if present)


    }

    private void CalculateDamage(CharacterManager character)
    {
        
        if (characterCausingDamage != null)
        {
            //Check for Damage modifiers and modify
        }

        //Check character for flat defenses and subtract

        //Add all damage types, and apply final damage to character

        finalDamageDealt = Mathf.RoundToInt(physicalDamage);  //+ other damage types

        if (finalDamageDealt <= 0)
            finalDamageDealt = 1;

        character.currentHealth -= finalDamageDealt;

        //Calculate Poise damage (maybe)
    }

}
