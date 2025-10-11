using UnityEngine;


[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Overheat Damage")]
public class TakeOverheatDamageCharacterEffect : InstantCharacterEffect
{
    
    public float overheatDamage;

    public override void ProcessEffect(CharacterManager character)
    {
        CalculateOverheatDamage(character);
    }

    private void CalculateOverheatDamage(CharacterManager character)
    {
      //Compare the base overheat damage against other player effects
      //change the value before subtracting/adding it

        character.currentOverheating += overheatDamage;
    }
}
