using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{

    public static TitleScreenManager Instance;
    [Header("Menus")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button deleteCharacterPopUpConfirmButton;
    [SerializeField] Button deleteCharacterPopUpNoButton;

    [Header("Pop Ups")]
    [SerializeField] GameObject noCharacterSlotsPopUp;
    [SerializeField] Button noCharacterSlotsOkayButton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;

    [Header("Character Slots")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;
    

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttemptToCreateNewGame();
        
    }

    public void OpenLoadGameMenu()
    {
        //Close Main Menu
        titleScreenMainMenu.SetActive(false); 

        //Open Load Menu
        titleScreenLoadMenu.SetActive(true);

        //Select the return button first
        loadMenuReturnButton.Select();


    }

    public void CloseLoadGameMenu()
    {
        titleScreenLoadMenu.SetActive(false);

        titleScreenMainMenu.SetActive(true);

        //Select the load game button
        mainMenuLoadGameButton.Select();
    }

    public void DisplayNoFreeCharacterSlotsPopup()
    {
        noCharacterSlotsPopUp.SetActive(true);
        noCharacterSlotsOkayButton.Select();
    }

    public void CloseNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }

    //Character Slots

    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }

    public void SelectNoSlot()
    {
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttemptToDeleteCharacterSlot()
    {
        if (currentSelectedSlot != CharacterSlot.NO_SLOT) 
        {
            deleteCharacterSlotPopUp.SetActive(true);
            deleteCharacterPopUpNoButton.Select();

        }
        
    }

    public void DeleteCharacterSlot()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);

        //Disable and enable the load menu, to refresh the slots
        titleScreenLoadMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);

        loadMenuReturnButton.Select();

    }

    public void CloseDeleteCharacterPopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();
    }
}
