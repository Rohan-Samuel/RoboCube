using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    // Process instant effects like damage, healing, buffs, debuffs, etc.

    // Process over time effects like poison, regeneration, etc.

    // Process Static effects like armor, resistances, etc.
    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        // Take in an effect
        effect.ProcessEffect(character);

        //process the effect
    }
}
