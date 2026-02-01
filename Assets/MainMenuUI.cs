using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject chapterPanel;

    // ▶ Start Game
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // tên scene gameplay
    }

    // 📚 Chọn chương
    public void OpenChapter()
    {
        mainMenuPanel.SetActive(false);
        chapterPanel.SetActive(true);
    }

    // ⚙ Settings
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // 🔙 Back
    public void BackToMain()
    {
        settingsPanel.SetActive(false);
        chapterPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // ❌ Quit Game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game"); // test trong Editor
    }
}
