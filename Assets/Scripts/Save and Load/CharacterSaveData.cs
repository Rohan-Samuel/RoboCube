using UnityEngine;

[System.Serializable]
//we want to reference this data for every save file, not monobehaviour and serializable
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName = "Character";

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Stats")]
    public int durability = 10;
    public int coolant = 10;

    [Header("Resources")]
    public float currentHealth;
    public float currentOverheating;
}
