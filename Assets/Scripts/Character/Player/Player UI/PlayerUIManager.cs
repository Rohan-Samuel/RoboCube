using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [HideInInspector] public PlayerUIHudManager playerUIHudManager;
    [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
