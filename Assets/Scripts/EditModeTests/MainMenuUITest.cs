using NUnit.Framework;
using UnityEngine;

public class MainMenuUIEditTests
{
    private GameObject menuObject;
    private MainMenuUI mainMenuUI;
    private GameObject mainMenuPanel;
    private GameObject settingsPanel;
    private GameObject chapterPanel;

    [SetUp]
    public void Setup()
    {
        // Tạo MainMenuUI
        menuObject = new GameObject("MainMenuUI");
        mainMenuUI = menuObject.AddComponent<MainMenuUI>();
        
        // Tạo panels
        mainMenuPanel = new GameObject("MainMenuPanel");
        settingsPanel = new GameObject("SettingsPanel");
        chapterPanel = new GameObject("ChapterPanel");
        
        // Gán references
        mainMenuUI.mainMenuPanel = mainMenuPanel;
        mainMenuUI.settingsPanel = settingsPanel;
        mainMenuUI.chapterPanel = chapterPanel;
        mainMenuUI.cutsceneSceneName = "Cutscene";
        
        // Set initial states
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        chapterPanel.SetActive(false);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(chapterPanel);
        Object.DestroyImmediate(settingsPanel);
        Object.DestroyImmediate(mainMenuPanel);
        Object.DestroyImmediate(menuObject);
    }

    // TEST 1: Component tồn tại
    [Test]
    public void MainMenuUI_ComponentExists()
    {
        Assert.IsNotNull(mainMenuUI);
        Assert.IsTrue(mainMenuUI.enabled);
    }

    // TEST 2: Default values
    [Test]
    public void MainMenuUI_HasDefaultValues()
    {
        GameObject obj = new GameObject();
        MainMenuUI menu = obj.AddComponent<MainMenuUI>();
        
        Assert.AreEqual("Cutscene", menu.cutsceneSceneName);
        Assert.IsNull(menu.mainMenuPanel);
        Assert.IsNull(menu.settingsPanel);
        Assert.IsNull(menu.chapterPanel);
        
        Object.DestroyImmediate(obj);
    }

    // TEST 3: Panels có thể gán
    [Test]
    public void MainMenuUI_PanelsCanBeAssigned()
    {
        GameObject newMain = new GameObject("NewMain");
        GameObject newSettings = new GameObject("NewSettings");
        GameObject newChapter = new GameObject("NewChapter");
        
        mainMenuUI.mainMenuPanel = newMain;
        mainMenuUI.settingsPanel = newSettings;
        mainMenuUI.chapterPanel = newChapter;
        
        Assert.AreEqual(newMain, mainMenuUI.mainMenuPanel);
        Assert.AreEqual(newSettings, mainMenuUI.settingsPanel);
        Assert.AreEqual(newChapter, mainMenuUI.chapterPanel);
        
        Object.DestroyImmediate(newChapter);
        Object.DestroyImmediate(newSettings);
        Object.DestroyImmediate(newMain);
    }

    // TEST 4: Scene names có thể thay đổi
    [Test]
    public void MainMenuUI_SceneNamesCanBeChanged()
    {
        mainMenuUI.cutsceneSceneName = "Intro";
        Assert.AreEqual("Intro", mainMenuUI.cutsceneSceneName);
        
        mainMenuUI.cutsceneSceneName = "";
        Assert.AreEqual("", mainMenuUI.cutsceneSceneName);
    }

    // TEST 5: Panels có thể null
    [Test]
    public void MainMenuUI_PanelsCanBeNull()
    {
        mainMenuUI.mainMenuPanel = null;
        mainMenuUI.settingsPanel = null;
        mainMenuUI.chapterPanel = null;
        
        Assert.IsNull(mainMenuUI.mainMenuPanel);
        Assert.IsNull(mainMenuUI.settingsPanel);
        Assert.IsNull(mainMenuUI.chapterPanel);
    }

    // TEST 6: MonoBehaviour check
    [Test]
    public void MainMenuUI_IsMonoBehaviour()
    {
        Assert.IsInstanceOf<MonoBehaviour>(mainMenuUI);
    }

    // TEST 7: Component type name
    [Test]
    public void MainMenuUI_HasCorrectTypeName()
    {
        Assert.AreEqual("MainMenuUI", mainMenuUI.GetType().Name);
    }

    // TEST 8: GameObject reference
    [Test]
    public void MainMenuUI_HasCorrectGameObject()
    {
        Assert.AreEqual(menuObject, mainMenuUI.gameObject);
    }

    // TEST 9: Initial panel states
    [Test]
    public void MainMenuUI_InitialPanelStates()
    {
        Assert.IsTrue(mainMenuPanel.activeSelf, "Main menu phải active");
        Assert.IsFalse(settingsPanel.activeSelf, "Settings phải inactive");
        Assert.IsFalse(chapterPanel.activeSelf, "Chapter phải inactive");
    }

    // TEST 10: Panels có thể toggle
    [Test]
    public void MainMenuUI_PanelsCanBeToggled()
    {
        mainMenuPanel.SetActive(false);
        Assert.IsFalse(mainMenuPanel.activeSelf);
        
        settingsPanel.SetActive(true);
        Assert.IsTrue(settingsPanel.activeSelf);
        
        chapterPanel.SetActive(true);
        Assert.IsTrue(chapterPanel.activeSelf);
    }

    // TEST 11: Multiple instances
    [Test]
    public void MainMenuUI_CanHaveMultipleInstances()
    {
        GameObject obj1 = new GameObject();
        MainMenuUI menu1 = obj1.AddComponent<MainMenuUI>();
        menu1.cutsceneSceneName = "Scene1";
        
        GameObject obj2 = new GameObject();
        MainMenuUI menu2 = obj2.AddComponent<MainMenuUI>();
        menu2.cutsceneSceneName = "Scene2";
        
        Assert.AreNotEqual(menu1.cutsceneSceneName, menu2.cutsceneSceneName);
        
        Object.DestroyImmediate(obj2);
        Object.DestroyImmediate(obj1);
    }

    // TEST 12: Scene name empty string
    [Test]
    public void MainMenuUI_SceneNameCanBeEmpty()
    {
        mainMenuUI.cutsceneSceneName = "";
        Assert.AreEqual("", mainMenuUI.cutsceneSceneName);
    }

    // TEST 13: Scene name với spaces
    [Test]
    public void MainMenuUI_SceneNameWithSpaces()
    {
        mainMenuUI.cutsceneSceneName = "Cut Scene Name";
        Assert.AreEqual("Cut Scene Name", mainMenuUI.cutsceneSceneName);
    }

    // TEST 14: Scene name với special characters
    [Test]
    public void MainMenuUI_SceneNameWithSpecialCharacters()
    {
        mainMenuUI.cutsceneSceneName = "Scene_01-Test";
        Assert.AreEqual("Scene_01-Test", mainMenuUI.cutsceneSceneName);
    }

    // TEST 15: Properties readable
    [Test]
    public void MainMenuUI_PropertiesAreReadable()
    {
        mainMenuUI.cutsceneSceneName = "TestScene";
        
        string sceneName = mainMenuUI.cutsceneSceneName;
        GameObject mainPanel = mainMenuUI.mainMenuPanel;
        GameObject settPanel = mainMenuUI.settingsPanel;
        GameObject chapPanel = mainMenuUI.chapterPanel;
        
        Assert.AreEqual("TestScene", sceneName);
        Assert.AreEqual(mainMenuPanel, mainPanel);
        Assert.AreEqual(settingsPanel, settPanel);
        Assert.AreEqual(chapterPanel, chapPanel);
    }

    // TEST 16: Panels có thể là cùng GameObject (edge case)
    [Test]
    public void MainMenuUI_PanelsCanBeTheSameObject()
    {
        GameObject samePanel = new GameObject("SamePanel");
        
        mainMenuUI.mainMenuPanel = samePanel;
        mainMenuUI.settingsPanel = samePanel;
        mainMenuUI.chapterPanel = samePanel;
        
        Assert.AreEqual(mainMenuUI.mainMenuPanel, mainMenuUI.settingsPanel);
        Assert.AreEqual(mainMenuUI.settingsPanel, mainMenuUI.chapterPanel);
        
        Object.DestroyImmediate(samePanel);
    }

    // TEST 17: Canvas hierarchy
    [Test]
    public void MainMenuUI_CanWorkWithCanvasHierarchy()
    {
        GameObject canvas = new GameObject("Canvas");
        canvas.AddComponent<Canvas>();
        
        mainMenuPanel.transform.SetParent(canvas.transform);
        settingsPanel.transform.SetParent(canvas.transform);
        chapterPanel.transform.SetParent(canvas.transform);
        
        Assert.AreEqual(canvas.transform, mainMenuPanel.transform.parent);
        Assert.AreEqual(canvas.transform, settingsPanel.transform.parent);
        Assert.AreEqual(canvas.transform, chapterPanel.transform.parent);
        
        Object.DestroyImmediate(canvas);
    }
}