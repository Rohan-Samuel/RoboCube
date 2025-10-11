using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    public PlayerManager player;
    public SaveDataManager saveDataManager;

    [Header("SAVE/LOAD")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    [Header ("World Scene Index")] 
    [SerializeField] int worldSceneIndex = 1;

    [Header("Save Data Writer")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")]
    public CharacterSlot currentCharacterSlotBeingUsed;
    public CharacterSaveData currentCharacterData;
    private string saveFileName;


    [Header("Character Slots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;

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
        LoadAllCharacterProfiles();
    }

    private void Update()
    {
        if (saveGame)
        {

            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot characterSlot)
    {
        string fileName = "";
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01 :
                fileName = "characterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02 :
                fileName = "characterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "characterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "characterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "characterSlot_05";
                break;
            
        }
        return fileName;
    }

    public void AttemptToCreateNewGame()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;


        //Check to see if we can createa new save file (check for other files existing first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        //If this profile slot is not taken make a new one using this slot
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        //Create a new file with a file name based on the slot being used
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

        //If this profile slot is not taken make a new one using this slot
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        //Create a new file with a file name based on the slot being used
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

        //If this profile slot is not taken make a new one using this slot
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        //Create a new file with a file name based on the slot being used
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

        //If this profile slot is not taken make a new one using this slot
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        //Create a new file with a file name based on the slot being used
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

        //If this profile slot is not taken make a new one using this slot
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        //If there are no free slots, send an error
        TitleScreenManager.Instance.DisplayNoFreeCharacterSlotsPopup();
    }

    private void NewGame()
    {
        //Save the newly created character, we are not
        

        StartCoroutine(LoadWorldScene());

        SaveDataManager.instance.durability = 10;
        SaveDataManager.instance.coolant = 10;
        SaveDataManager.instance.currentHealth = 100;
        SaveDataManager.instance.currentOverheating = 0;


        // SaveGame();
    }

    public void LoadGame()
    {
        // load a previous file, with a file name based on which slot is being used
        saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    {
        //save the current file under a file name depending on wchich slot is being used
        saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        //pass the players info, from game to save file
        PlayerManager.instance.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

        //Write that info onto a json file, saved to the machine
        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }

    public void DeleteGame(CharacterSlot characterSlot)
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        //Choose a file to delete based on name
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(characterSlot);

        saveFileDataWriter.DeleteSaveFile();
    }

    //Load all character profiles on device when starting game
    private void LoadAllCharacterProfiles()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        characterSlot01 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
        characterSlot04 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedonCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
        characterSlot05 = saveFileDataWriter.LoadSaveFile();
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

        SaveDataManager.instance.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);

        yield return null;
    }
}
