using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private CanvasGroup canvasGroup;

    public void SetAlpha(float alpha)
    {
        this.canvasGroup.alpha = alpha;
    }

    public void DoFadeOut(float duration)
    {
        StartCoroutine(FadeOut(duration, 0f));
    }
    public void DoFadeOut(float duration, float delay)
    {
        StartCoroutine(FadeOut(duration, delay));
    }
    private IEnumerator FadeOut(float duration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.fixedUnscaledDeltaTime;
            SetAlpha(Mathf.Lerp(1, 0, elapsed / duration));

            yield return null;
        }

        SetAlpha(0f);
    }

    public void DoFadeIn(float duration)
    {
        StartCoroutine(FadeIn(duration));
    }
    private IEnumerator FadeIn(float duration)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.fixedUnscaledDeltaTime;
            SetAlpha(Mathf.Lerp(0, 1, elapsed / duration));

            yield return null;
        }

        SetAlpha(1f);
    }

    public void DoBlink(int blinkAmount, float blinkInterval, float delay)
    {
        StartCoroutine(Blink(blinkAmount, blinkInterval, delay));
    }
    private IEnumerator Blink(int blinkAmount, float blinkInterval, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        for (int i = 0; i < blinkAmount; i++)
        {
            SetAlpha(0f);
            yield return new WaitForSecondsRealtime(blinkInterval/2);
            SetAlpha(1f);
            yield return new WaitForSecondsRealtime(blinkInterval/2);
        }
    }
}
