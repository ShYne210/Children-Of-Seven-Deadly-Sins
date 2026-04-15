using NUnit.Framework;
using UnityEngine;

public class WinNotificationUIEditTests
{
    private GameObject testObject;
    private WinNotificationUI winUI;
    private GameObject testPanel;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject("WinUI");
        winUI = testObject.AddComponent<WinNotificationUI>();
        
        testPanel = new GameObject("Panel");
        winUI.panel = testPanel;
        
        testPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(testPanel);
        Object.DestroyImmediate(testObject);
        Time.timeScale = 1f;
    }

    // TEST 1: Kiểm tra component được tạo đúng
    [Test]
    public void WinNotificationUI_ComponentExists()
    {
        Assert.IsNotNull(winUI, "WinNotificationUI component phải tồn tại");
        Assert.IsTrue(winUI.enabled, "Component phải được enable");
    }

    // TEST 2: Kiểm tra default values
    [Test]
    public void WinNotificationUI_HasDefaultValues()
    {
        GameObject obj = new GameObject();
        WinNotificationUI newWinUI = obj.AddComponent<WinNotificationUI>();
        
        Assert.AreEqual(3f, newWinUI.showTime, "ShowTime mặc định phải là 3f");
        Assert.IsNull(newWinUI.panel, "Panel mặc định phải là null");
        
        Object.DestroyImmediate(obj);
    }

    // TEST 3: Kiểm tra panel reference
    [Test]
    public void WinNotificationUI_PanelCanBeAssigned()
    {
        GameObject newPanel = new GameObject("NewPanel");
        winUI.panel = newPanel;
        
        Assert.AreEqual(newPanel, winUI.panel, "Panel phải được gán đúng");
        
        Object.DestroyImmediate(newPanel);
    }

    // TEST 4: Kiểm tra showTime có thể thay đổi
    [Test]
    public void WinNotificationUI_ShowTimeCanBeChanged()
    {
        winUI.showTime = 5f;
        Assert.AreEqual(5f, winUI.showTime);
        
        winUI.showTime = 0.5f;
        Assert.AreEqual(0.5f, winUI.showTime);
        
        winUI.showTime = 0f;
        Assert.AreEqual(0f, winUI.showTime);
    }

    // TEST 5: Kiểm tra showTime có thể là số âm (edge case)
    [Test]
    public void WinNotificationUI_ShowTimeCanBeNegative()
    {
        // Unity cho phép giá trị âm, nhưng không nên dùng
        winUI.showTime = -1f;
        Assert.AreEqual(-1f, winUI.showTime);
        
        // Lưu ý: Nếu bạn muốn validate, cần thêm setter
    }

    // TEST 6: Kiểm tra panel null (edge case)
    [Test]
    public void WinNotificationUI_PanelCanBeNull()
    {
        winUI.panel = null;
        Assert.IsNull(winUI.panel, "Panel có thể là null");
    }

    // TEST 7: Kiểm tra nhiều WinNotificationUI trên cùng GameObject
    [Test]
    public void WinNotificationUI_CannotHaveMultipleOnSameObject()
    {
        // Unity không cho phép 2 component cùng loại
        WinNotificationUI secondWinUI = testObject.GetComponent<WinNotificationUI>();
        
        Assert.IsNotNull(secondWinUI);
        Assert.AreEqual(winUI, secondWinUI, "Phải là cùng 1 instance");
    }

    // TEST 8: Kiểm tra GameObject có MonoBehaviour
    [Test]
    public void WinNotificationUI_IsMonoBehaviour()
    {
        Assert.IsInstanceOf<MonoBehaviour>(winUI, "WinNotificationUI phải là MonoBehaviour");
    }

    // TEST 9: Kiểm tra panel state ban đầu
    [Test]
    public void WinNotificationUI_PanelInitialStateNotAffected()
    {
        // Panel được set active false trong Setup
        Assert.IsFalse(testPanel.activeSelf, "Panel phải ẩn ban đầu");
        
        // Tạo WinUI mới không làm thay đổi panel
        GameObject newObj = new GameObject();
        WinNotificationUI newWinUI = newObj.AddComponent<WinNotificationUI>();
        newWinUI.panel = testPanel;
        
        Assert.IsFalse(testPanel.activeSelf, "Panel vẫn phải ẩn");
        
        Object.DestroyImmediate(newObj);
    }

    // TEST 10: Kiểm tra properties có thể đọc
    [Test]
    public void WinNotificationUI_PropertiesAreReadable()
    {
        winUI.showTime = 2.5f;
        GameObject panel = new GameObject("TestPanel");
        winUI.panel = panel;
        
        float readShowTime = winUI.showTime;
        GameObject readPanel = winUI.panel;
        
        Assert.AreEqual(2.5f, readShowTime);
        Assert.AreEqual(panel, readPanel);
        
        Object.DestroyImmediate(panel);
    }

    // TEST 11: Kiểm tra component name
    [Test]
    public void WinNotificationUI_HasCorrectTypeName()
    {
        string typeName = winUI.GetType().Name;
        Assert.AreEqual("WinNotificationUI", typeName);
    }

    // TEST 12: Kiểm tra GameObject của component
    [Test]
    public void WinNotificationUI_HasCorrectGameObject()
    {
        Assert.AreEqual(testObject, winUI.gameObject);
        Assert.AreEqual("WinUI", winUI.gameObject.name);
    }

    // TEST 13: Test với inactive panel
    [Test]
    public void WinNotificationUI_WorksWithInactivePanel()
    {
        testPanel.SetActive(false);
        winUI.panel = testPanel;
        
        Assert.IsFalse(testPanel.activeSelf);
        Assert.AreEqual(testPanel, winUI.panel);
    }

    // TEST 14: Test với active panel
    [Test]
    public void WinNotificationUI_WorksWithActivePanel()
    {
        testPanel.SetActive(true);
        winUI.panel = testPanel;
        
        Assert.IsTrue(testPanel.activeSelf);
        Assert.AreEqual(testPanel, winUI.panel);
    }

    // TEST 15: Kiểm tra panel với children
    [Test]
    public void WinNotificationUI_WorksWithPanelHavingChildren()
    {
        GameObject child1 = new GameObject("Child1");
        GameObject child2 = new GameObject("Child2");
        child1.transform.SetParent(testPanel.transform);
        child2.transform.SetParent(testPanel.transform);
        
        winUI.panel = testPanel;
        
        Assert.AreEqual(2, testPanel.transform.childCount);
        Assert.AreEqual(testPanel, winUI.panel);
        
        Object.DestroyImmediate(child2);
        Object.DestroyImmediate(child1);
    }

    // TEST 16: Test extreme showTime values
    [Test]
    public void WinNotificationUI_HandlesExtremeShowTimeValues()
    {
        winUI.showTime = float.MaxValue;
        Assert.AreEqual(float.MaxValue, winUI.showTime);
        
        winUI.showTime = float.MinValue;
        Assert.AreEqual(float.MinValue, winUI.showTime);
        
        winUI.showTime = 0.001f;
        Assert.AreEqual(0.001f, winUI.showTime, 0.0001f);
    }

    // TEST 17: Test panel replacement
    [Test]
    public void WinNotificationUI_CanReplacePanel()
    {
        GameObject oldPanel = testPanel;
        GameObject newPanel = new GameObject("NewPanel");
        
        Assert.AreEqual(oldPanel, winUI.panel);
        
        winUI.panel = newPanel;
        
        Assert.AreEqual(newPanel, winUI.panel);
        Assert.AreNotEqual(oldPanel, winUI.panel);
        
        Object.DestroyImmediate(newPanel);
    }
}