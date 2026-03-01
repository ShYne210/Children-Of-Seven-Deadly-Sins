using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private string deathSceneName = "GameOverScene";
    [SerializeField] private string endingSceneName = "EndingScene";
    [SerializeField] private float cameraDelay = 2f; // thời gian xoay camera trước khi chuyển scene

    public void TriggerDeathCutscene()
    {
        StartCoroutine(CameraRotateAndScene(deathSceneName));
    }

    public void TriggerEndingCutscene()
    {
        StartCoroutine(CameraRotateAndScene(endingSceneName));
    }

    private IEnumerator CameraRotateAndScene(string sceneName)
    {
        Transform cam = Camera.main.transform;
        EnemyVision enemy = FindObjectOfType<EnemyVision>();
        if (enemy == null) yield break;

        Transform enemyTransform = enemy.transform;

        float elapsed = 0f;
        Quaternion startRot = cam.rotation;
        Quaternion targetRot = Quaternion.LookRotation((enemyTransform.position - cam.position).normalized);

        // Xoay camera trong cameraDelay giây
        while (elapsed < cameraDelay)
        {
            elapsed += Time.deltaTime;
            cam.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / cameraDelay);
            yield return null;
        }

        // Sau khi xoay xong → chuyển scene
        SceneManager.LoadScene(sceneName);
    }
}
