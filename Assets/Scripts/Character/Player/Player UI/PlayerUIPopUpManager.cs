using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("SIGNAL LOST Pop Up")]
    [SerializeField] GameObject signalLostPopUpGameObject;
    [SerializeField] TextMeshProUGUI signalLostPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI signalLostPopUpMainText;
    [SerializeField] CanvasGroup signalLostPopUpCanvasGroup;

    public void SendSignalLostPopUp()
    {
        //Activate post processing effects

        signalLostPopUpGameObject.SetActive(true);
        signalLostPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUptextOverTime(signalLostPopUpBackgroundText, 8, 8.32f));
        StartCoroutine(FadeInPopUpOverTime(signalLostPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutOverTime(signalLostPopUpCanvasGroup, 2, 5));
    }

    private IEnumerator StretchPopUptextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if (duration > 0f)
        {
            text.characterSpacing = 0;
            float timer = 0;

            yield return null;
            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
    {
        if (duration > 0)
        {
            canvas.alpha = 0;
            float timer = 0;
             
            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 1;

        yield return null;
    }

    private IEnumerator WaitThenFadeOutOverTime(CanvasGroup canvas, float duration, float delay)
    {
        if (duration > 0)
        {
            while (delay > 0)
            {
                delay = delay - Time.deltaTime;
                yield return null;
            }
            canvas.alpha = 1;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 0;

        yield return null;
    }
}
