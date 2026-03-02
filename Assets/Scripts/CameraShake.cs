using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    Vector3 originalPos;

    private void Awake()
    {
        instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    System.Collections.IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float timer = 0;

        while (timer < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}