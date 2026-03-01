using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyCatchCutscene : MonoBehaviour
{
    [Header("===== Công cụ =====")]
    public Transform enemy;
    public Camera playerCamera;
    public Image fadeImage;

    [Header("===== Hiệu ứng =====")]
    public float rotateDuration = 2f;
    public float shakeIntensity = 0.1f;
    public float fadeDuration = 2f;

    public void StartCutscene() { StartCoroutine(CutsceneRoutine()); }

    IEnumerator CutsceneRoutine()
    {
        playerCamera.GetComponent<FirstPersonCamera>().enabled = false;
        Quaternion startRot = playerCamera.transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(enemy.position - playerCamera.transform.position);

        float timer = 0f;
        while (timer < rotateDuration)
        {
            timer += Time.deltaTime;
            playerCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, timer / rotateDuration);
            playerCamera.transform.position += Random.insideUnitSphere * shakeIntensity * Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        Color c = fadeImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }
}
