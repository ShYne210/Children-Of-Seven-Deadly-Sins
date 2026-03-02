using UnityEngine;
using System.Collections;

public class MissionZoneTrigger : MonoBehaviour
{
    public GameObject missionUI;
    public float showTime = 4f;

    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggered) return;

        triggered = true;

        StartCoroutine(ShowMission());
    }

    IEnumerator ShowMission()
    {
        missionUI.SetActive(true);

        yield return new WaitForSeconds(showTime);

        missionUI.SetActive(false);
    }
}