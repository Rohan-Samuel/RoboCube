using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{

    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar overheatBar;

    public void RefreshHUD()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        overheatBar.gameObject.SetActive(false);
        overheatBar.gameObject.SetActive(true);

    }

    public void SetNewHealthValue(float newValue)
    {
        healthBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }

    public void SetNewOverheatValue(float newValue)
    {
        overheatBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxOverheatValue(int maxOverheating)
    {
        overheatBar.SetMaxStat(maxOverheating);
    }

    
}
