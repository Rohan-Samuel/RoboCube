using UnityEngine;
using Unity.Collections;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager instance;

    public FixedString64Bytes characterName = "Character";
    public Vector3 characterPosition;
    public int durability;
    public int coolant;
    public float currentHealth;
    public float currentOverheating;

    private void Awake()
    {
        // There can be only one instance of theis script active at one time, if another exists, destroy it
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }



    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        characterName = currentCharacterData.characterName;
        characterPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        //transform.position = myPosition;

        durability = currentCharacterData.durability;
        coolant = currentCharacterData.coolant;

        currentHealth = currentCharacterData.currentHealth;
        currentOverheating = currentCharacterData.currentOverheating;


    }

}
