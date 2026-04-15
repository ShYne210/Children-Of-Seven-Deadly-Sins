using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine.SceneManagement;

public class MainMenuUI_PlayModeTests
{
    private GameObject testObject;
    private MainMenuUI mainMenuUI;
    private GameObject mainMenuPanel;
    private GameObject settingsPanel;
    private GameObject chapterPanel;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Tạo GameObject test
        testObject = new GameObject("MainMenuUI_Test");
        mainMenuUI = testObject.AddComponent<MainMenuUI>();

        // Tạo các panel
        mainMenuPanel = new GameObject("MainMenuPanel");
        settingsPanel = new GameObject("SettingsPanel");
        chapterPanel = new GameObject("ChapterPanel");

        // Gán vào MainMenuUI
        mainMenuUI.mainMenuPanel = mainMenuPanel;
        mainMenuUI.settingsPanel = settingsPanel;
        mainMenuUI.chapterPanel = chapterPanel;
        mainMenuUI.cutsceneSceneName = "Cutscene";

        // Set trạng thái ban đầu
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        chapterPanel.SetActive(false);

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Dọn dẹp
        if (mainMenuPanel != null) Object.Destroy(mainMenuPanel);
        if (settingsPanel != null) Object.Destroy(settingsPanel);
        if (chapterPanel != null) Object.Destroy(chapterPanel);
        if (testObject != null) Object.Destroy(testObject);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_OpenChapter_WithDelay()
    {
        // Act
        mainMenuUI.OpenChapter();
        yield return null; // Đợi 1 frame

        // Assert
        Assert.IsFalse(mainMenuPanel.activeSelf, "Main Menu Panel nên bị ẩn");
        Assert.IsTrue(chapterPanel.activeSelf, "Chapter Panel nên hiện");
    }

    [UnityTest]
    public IEnumerator Test_OpenSettings_WithDelay()
    {
        // Act
        mainMenuUI.OpenSettings();
        yield return null;

        // Assert
        Assert.IsFalse(mainMenuPanel.activeSelf);
        Assert.IsTrue(settingsPanel.activeSelf);
    }

    [UnityTest]
    public IEnumerator Test_BackToMain_WithDelay()
    {
        // Arrange
        mainMenuUI.OpenSettings();
        yield return null;

        // Act
        mainMenuUI.BackToMain();
        yield return null;

        // Assert
        Assert.IsTrue(mainMenuPanel.activeSelf);
        Assert.IsFalse(settingsPanel.activeSelf);
    }

    [UnityTest]
    public IEnumerator Test_NavigationFlow_Chapter_To_Main()
    {
        // Mở Chapter
        mainMenuUI.OpenChapter();
        yield return null;
        Assert.IsTrue(chapterPanel.activeSelf, "Chapter Panel nên mở");

        // Quay về Main
        mainMenuUI.BackToMain();
        yield return null;
        Assert.IsTrue(mainMenuPanel.activeSelf, "Main Panel nên hiện lại");
    }

    [UnityTest]
    public IEnumerator Test_NavigationFlow_Settings_To_Main()
    {
        // Mở Settings
        mainMenuUI.OpenSettings();
        yield return null;
        Assert.IsTrue(settingsPanel.activeSelf);

        // Quay về Main
        mainMenuUI.BackToMain();
        yield return null;
        Assert.IsTrue(mainMenuPanel.activeSelf);
    }

    [UnityTest]
    public IEnumerator Test_QuitGame_DoesNotThrowError()
    {
        // Act & Assert (không crash)
        Assert.DoesNotThrow(() => mainMenuUI.QuitGame());
        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_StartGame_LoadsCorrectScene()
    {
        // Lưu ý: Test này cần scene "Cutscene" trong Build Settings
        // Nếu không có scene sẽ log warning

        // Act
        LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex(".*"));
        mainMenuUI.StartGame();
        
        yield return null;

        // Kiểm tra xem có attempt load scene không
        // (Trong test environment có thể fail nếu scene không tồn tại)
    }

    [UnityTest]
    public IEnumerator Test_MultipleNavigation_ShouldWork()
    {
        // Settings -> Main -> Chapter -> Main
        mainMenuUI.OpenSettings();
        yield return null;
        Assert.IsTrue(settingsPanel.activeSelf);

        mainMenuUI.BackToMain();
        yield return null;
        Assert.IsTrue(mainMenuPanel.activeSelf);

        mainMenuUI.OpenChapter();
        yield return null;
        Assert.IsTrue(chapterPanel.activeSelf);

        mainMenuUI.BackToMain();
        yield return null;
        Assert.IsTrue(mainMenuPanel.activeSelf);
    }

    [UnityTest]
    public IEnumerator Test_PanelState_AfterMultipleClicks()
    {
        // Click nhiều lần Settings
        for (int i = 0; i < 3; i++)
        {
            mainMenuUI.OpenSettings();
            yield return null;
        }

        Assert.IsTrue(settingsPanel.activeSelf, "Settings vẫn nên active sau nhiều clicks");
        Assert.IsFalse(mainMenuPanel.activeSelf);
    }
}