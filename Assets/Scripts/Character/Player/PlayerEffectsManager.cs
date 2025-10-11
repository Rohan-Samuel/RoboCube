using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("debug Tester")]
    [SerializeField] InstantCharacterEffect testEffect;
    [SerializeField] bool processEffect = false;

    private void Update()
    {
        {
            if (processEffect)
            {
                processEffect = false;
                InstantCharacterEffect effect = Instantiate(testEffect);
                ProcessInstantEffect(effect);
            }
        }
    }
}
