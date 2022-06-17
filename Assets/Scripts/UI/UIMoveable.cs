using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveable : MonoBehaviour
{
    public void SetPosition(Vector2 targetPosition)
    {
        this.transform.localPosition = targetPosition;
    }

    public void MoveTo(Vector2 targetPosition, float duration)
    {
        StartCoroutine(MoveCoroutine(targetPosition, duration, 0f));
    }
    public void MoveTo(Vector2 targetPosition, float duration, float delay)
    {
        StartCoroutine(MoveCoroutine(targetPosition, duration, delay));
    }
    private IEnumerator MoveCoroutine(Vector2 targetPosition, float duration, float delay)
    {
        Vector3 originalPos = transform.localPosition;

        yield return new WaitForSecondsRealtime(delay);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.fixedUnscaledDeltaTime;
            this.transform.localPosition = Vector2.Lerp(originalPos, targetPosition, elapsed / duration);

            yield return null;
        }
    }
}
