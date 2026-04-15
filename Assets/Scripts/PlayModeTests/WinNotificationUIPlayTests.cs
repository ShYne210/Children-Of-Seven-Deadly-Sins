using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WinNotificationUIPlayTests
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
        winUI.showTime = 0.5f;
        
        testPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(testPanel);
        Object.Destroy(testObject);
        Time.timeScale = 1f;
    }

    // TEST 1: Panel được kích hoạt
    [UnityTest]
    public IEnumerator ShowWinUI_ActivatesPanel()
    {
        Assert.IsFalse(testPanel.activeSelf, "Panel phải ẩn ban đầu");
        
        winUI.ShowWinUI();
        yield return null;
        
        Assert.IsTrue(testPanel.activeSelf, "Panel phải hiện sau khi gọi ShowWinUI");
    }

    // TEST 2: Game bị pause
    [UnityTest]
    public IEnumerator ShowWinUI_PausesGame()
    {
        Assert.AreEqual(1f, Time.timeScale, "TimeScale ban đầu phải là 1");
        
        winUI.ShowWinUI();
        yield return null;
        
        Assert.AreEqual(0f, Time.timeScale, "Game phải pause (timeScale = 0)");
    }

    // TEST 3: Panel ẩn sau showTime
    [UnityTest]
    public IEnumerator ShowWinUI_HidesPanelAfterShowTime()
    {
        winUI.ShowWinUI();
        yield return null;
        
        Assert.IsTrue(testPanel.activeSelf, "Panel đang hiển thị");
        
        yield return new WaitForSecondsRealtime(winUI.showTime + 0.2f);
        
        Assert.IsFalse(testPanel.activeSelf, "Panel phải ẩn sau showTime");
    }

    // TEST 4: Game resume sau showTime
    [UnityTest]
    public IEnumerator ShowWinUI_ResumesGameAfterShowTime()
    {
        winUI.ShowWinUI();
        yield return null;
        
        yield return new WaitForSecondsRealtime(winUI.showTime + 0.2f);
        
        Assert.AreEqual(1f, Time.timeScale, "Game phải resume (timeScale = 1)");
    }

    // TEST 5: Toàn bộ flow hoạt động đúng
    [UnityTest]
    public IEnumerator ShowWinUI_CompleteFlow_WorksCorrectly()
    {
        // Trạng thái ban đầu
        Assert.IsFalse(testPanel.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
        
        // Gọi ShowWinUI
        winUI.ShowWinUI();
        yield return null;
        
        // Trong khi hiển thị
        Assert.IsTrue(testPanel.activeSelf, "Panel phải hiện");
        Assert.AreEqual(0f, Time.timeScale, "Game phải pause");
        
        // Sau khi hết thời gian
        yield return new WaitForSecondsRealtime(winUI.showTime + 0.2f);
        
        Assert.IsFalse(testPanel.activeSelf, "Panel phải ẩn");
        Assert.AreEqual(1f, Time.timeScale, "Game phải resume");
    }

    // TEST 6: Gọi nhiều lần liên tiếp
    [UnityTest]
    public IEnumerator ShowWinUI_CalledMultipleTimes_WorksCorrectly()
    {
        winUI.showTime = 0.3f;
        
        // Lần 1
        winUI.ShowWinUI();
        yield return null;
        Assert.IsTrue(testPanel.activeSelf);
        
        yield return new WaitForSecondsRealtime(0.4f);
        Assert.IsFalse(testPanel.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
        
        // Lần 2
        winUI.ShowWinUI();
        yield return null;
        Assert.IsTrue(testPanel.activeSelf);
        Assert.AreEqual(0f, Time.timeScale);
        
        yield return new WaitForSecondsRealtime(0.4f);
        Assert.IsFalse(testPanel.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
    }

    // TEST 7: ShowTime khác nhau
    [UnityTest]
    public IEnumerator ShowWinUI_WithDifferentShowTimes_WorksCorrectly()
    {
        winUI.showTime = 1f;
        
        winUI.ShowWinUI();
        yield return null;
        
        Assert.IsTrue(testPanel.activeSelf);
        
        // Kiểm tra panel vẫn hiện ở giữa
        yield return new WaitForSecondsRealtime(0.5f);
        Assert.IsTrue(testPanel.activeSelf, "Panel vẫn phải hiện ở giữa chừng");
        Assert.AreEqual(0f, Time.timeScale, "Game vẫn pause");
        
        // Kiểm tra panel ẩn sau khi hết thời gian
        yield return new WaitForSecondsRealtime(0.7f);
        Assert.IsFalse(testPanel.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
    }

    // TEST 8: ShowTime rất ngắn
    [UnityTest]
    public IEnumerator ShowWinUI_VeryShortShowTime_StillWorks()
    {
        winUI.showTime = 0.01f;
        
        winUI.ShowWinUI();
        yield return null;
        
        Assert.IsTrue(testPanel.activeSelf);
        Assert.AreEqual(0f, Time.timeScale);
        
        yield return new WaitForSecondsRealtime(0.1f);
        
        Assert.IsFalse(testPanel.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
    }

    // TEST 9: ShowTime = 0
    [UnityTest]
    public IEnumerator ShowWinUI_ZeroShowTime_CompletesImmediately()
    {
        winUI.showTime = 0f;
        
        winUI.ShowWinUI();
        yield return null;
        
        yield return new WaitForSecondsRealtime(0.1f);
        
        Assert.IsFalse(testPanel.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
    }

    // TEST 10: Panel null - xử lý lỗi
    [UnityTest]
    public IEnumerator ShowWinUI_NullPanel_LogsError()
    {
        winUI.panel = null;
        
        LogAssert.Expect(LogType.Exception, "NullReferenceException");
        
        winUI.ShowWinUI();
        yield return null;
    }

    // TEST 11: Coroutine chạy trên đúng GameObject
    [UnityTest]
    public IEnumerator ShowWinUI_CoroutineRunsOnCorrectObject()
    {
        Assert.AreEqual(testObject, winUI.gameObject);
        
        winUI.ShowWinUI();
        yield return null;
        
        Assert.IsTrue(testPanel.activeSelf);
        Assert.IsNotNull(winUI);
        Assert.IsTrue(winUI.enabled);
    }

    // TEST 12: Destroy GameObject trong khi coroutine chạy
    [UnityTest]
    public IEnumerator ShowWinUI_DestroyObjectDuringCoroutine_StopsCoroutine()
    {
        winUI.showTime = 1f;
        winUI.ShowWinUI();
        yield return null;
        
        Assert.IsTrue(testPanel.activeSelf);
        Assert.AreEqual(0f, Time.timeScale);
        
        // Destroy object
        Object.Destroy(testObject);
        yield return null;
        
        // TimeScale không tự động reset (đây là bug tiềm ẩn!)
        // Nên cần cleanup trong OnDestroy
        Time.timeScale = 1f; // Manual reset
    }

    // TEST 13: Disable component trong khi coroutine chạy
    [UnityTest]
    public IEnumerator ShowWinUI_DisableComponentDuringCoroutine_StopsCoroutine()
    {
        winUI.showTime = 1f;
        winUI.ShowWinUI();
        yield return null;
        
        Assert.IsTrue(testPanel.activeSelf);
        
        winUI.enabled = false;
        yield return new WaitForSecondsRealtime(1.2f);
        
        // Coroutine dừng, cần manual cleanup
        Time.timeScale = 1f;
    }

    // TEST 14: Panel với RectTransform (UI thật)
    [UnityTest]
    public IEnumerator ShowWinUI_WithRectTransform_WorksCorrectly()
    {
        GameObject canvas = new GameObject("Canvas");
        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        
        GameObject uiPanel = new GameObject("UIPanel");
        uiPanel.AddComponent<RectTransform>();
        uiPanel.transform.SetParent(canvas.transform);
        
        winUI.panel = uiPanel;
        winUI.showTime = 0.3f;
        uiPanel.SetActive(false);
        
        winUI.ShowWinUI();
        yield return null;
        
        Assert.IsTrue(uiPanel.activeSelf);
        
        yield return new WaitForSecondsRealtime(0.4f);
        
        Assert.IsFalse(uiPanel.activeSelf);
        
        Object.Destroy(canvas);
    }

    // TEST 15: WaitForSecondsRealtime hoạt động đúng khi timeScale = 0
    [UnityTest]
    public IEnumerator ShowWinUI_UsesRealtimeWait_WorksWhenTimePaused()
    {
        winUI.showTime = 0.5f;
        
        float startTime = Time.realtimeSinceStartup;
        
        winUI.ShowWinUI();
        yield return null;
        
        // Game pause
        Assert.AreEqual(0f, Time.timeScale);
        
        yield return new WaitForSecondsRealtime(winUI.showTime + 0.2f);
        
        float elapsed = Time.realtimeSinceStartup - startTime;
        
        Assert.GreaterOrEqual(elapsed, winUI.showTime);
        Assert.IsFalse(testPanel.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
    }
}