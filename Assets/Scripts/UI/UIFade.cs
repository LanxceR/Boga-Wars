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
        StartCoroutine(FadeOut(duration));
    }
    private IEnumerator FadeOut(float duration)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            this.canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / duration);

            yield return null;
        }

        this.canvasGroup.alpha = 0f;
    }
    public void DoFadeOut(float duration, float delay)
    {
        StartCoroutine(FadeOut(duration, delay));
    }
    private IEnumerator FadeOut(float duration, float delay)
    {
        Vector3 originalPos = transform.localPosition;

        yield return new WaitForSeconds(delay);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            this.canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / duration);

            yield return null;
        }

        this.canvasGroup.alpha = 0f;
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
            elapsed += Time.deltaTime;
            this.canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / duration);

            yield return null;
        }

        this.canvasGroup.alpha = 1f;
    }
}
