using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
    public static WorldCharacterEffectsManager instance;

    [Header("Damage")]
    public TakeHealthDamageEffect takeHealthDamageEffect;

    [SerializeField] List<InstantCharacterEffect> instantEffects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           // DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        GenerateEffectIDs();
    }

    private void GenerateEffectIDs()
    {
        for (int i = 0; i < instantEffects.Count; i++)
        {
            instantEffects[i].instantEffectID = i;
        }
    }
}
