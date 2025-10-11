using UnityEngine;
using UnityEngine.UI;
public class UI_StatBar : MonoBehaviour
{
    private Slider slider;
    private RectTransform rectTransform;

    [Header("Bar Options")]
    [SerializeField] protected bool scaleBarLengthWtihStat = false;
    [SerializeField] protected float widthScaleMultiplier = 1;
    // Variable to scale bar size depending on stat (expanding bar)
    //secondary bar behind the bar to show how much is used

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;
    }

    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        // slider.value = 0;

        if (scaleBarLengthWtihStat)
        {
            rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
            PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
        }
    }
}
