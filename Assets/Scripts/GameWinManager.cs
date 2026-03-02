using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinManager : MonoBehaviour
{
    public static GameWinManager instance;

    public GameObject winUI;

    void Awake()
    {
        instance = this;
    }

    // ===== HIỆN WIN =====
    public void WinGame()
    {
        winUI.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ===== QUA MÀN =====
    public void NextLevel()
    {
        Time.timeScale = 1f;

        int currentScene =
            SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene + 1);
    }

    // ===== CHƠI LẠI =====
    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }

    // ===== THOÁT GAME =====
    public void QuitGame()
    {
        Application.Quit();
    }
}