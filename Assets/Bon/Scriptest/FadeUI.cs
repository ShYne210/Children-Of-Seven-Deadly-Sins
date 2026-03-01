using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeUI : MonoBehaviour
{
    [Header("===== UI Fade =====")]
    public Image fadePanel;
    public float fadeDuration = 2f;

    public void StartFade() { StartCoroutine(FadeOut()); }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        Color c = fadePanel.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }
    }
}
