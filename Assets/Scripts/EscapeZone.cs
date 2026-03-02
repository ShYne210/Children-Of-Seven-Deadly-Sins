using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeZone : MonoBehaviour
{
    public string nextSceneName;

    bool playerInside = false;
    bool unlocked = false;

    // ======================
    // UPDATE
    // ======================
    void Update()
    {
        if (!playerInside) return;

        // ✅ HIỆN UI
        if (InteractUIManager.instance != null)
        {
            if (unlocked)
                InteractUIManager.instance.Show("Press E to Exit");
            else
                InteractUIManager.instance.Show("Door Locked");
        }

        // ✅ CHUYỂN SCENE
        if (unlocked && Input.GetKeyDown(KeyCode.E))
        {
            ExitGame();
        }
    }

    // ======================
    // EXIT
    // ======================
    void ExitGame()
    {
        Debug.Log("EXIT LEVEL");

        if (InteractUIManager.instance != null)
            InteractUIManager.instance.Hide();

        SceneManager.LoadScene(nextSceneName);
    }

    // ======================
    // TRIGGER
    // ======================
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (InteractUIManager.instance != null)
            InteractUIManager.instance.Hide();
    }

    // ======================
    // LOCK / UNLOCK
    // ======================
    public void UnlockEscape()
    {
        unlocked = true;
        Debug.Log("Escape Unlocked!");
    }

    public void LockEscape()
    {
        unlocked = false;
    }
}