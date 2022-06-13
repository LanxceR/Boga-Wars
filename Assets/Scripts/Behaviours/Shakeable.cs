using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shakeable : MonoBehaviour
{
    [SerializeField] private float magnitude = 0f;

    public void DoShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        this.magnitude = magnitude;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * this.magnitude;
            float y = Random.Range(-1f, 1f) * this.magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            this.magnitude = Mathf.Lerp(magnitude, 0, elapsed / duration);

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
