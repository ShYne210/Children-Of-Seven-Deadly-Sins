using UnityEngine;
using System.Collections;

public class UINotification : MonoBehaviour
{
    public static UINotification instance;

    public GameObject successUI;
    public GameObject failUI;

    void Awake()
    {
        instance = this;

        successUI.SetActive(false);
        failUI.SetActive(false);
    }

    public void ShowSuccess(float time = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(Show(successUI, time));
    }

    public void ShowFail(float time = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(Show(failUI, time));
    }

    IEnumerator Show(GameObject ui, float time)
    {
        successUI.SetActive(false);
        failUI.SetActive(false);

        ui.SetActive(true);

        yield return new WaitForSeconds(time);

        ui.SetActive(false);
    }
}