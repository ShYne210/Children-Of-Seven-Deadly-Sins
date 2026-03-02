using UnityEngine;
using System.Collections;

public class WinNotificationUI : MonoBehaviour
{
    public GameObject panel;
    public float showTime = 3f;

    public void ShowWinUI()
    {
        StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        panel.SetActive(true);

        Time.timeScale = 0f; // pause game

        yield return new WaitForSecondsRealtime(showTime);

        panel.SetActive(false);

        Time.timeScale = 1f; // resume
    }
}