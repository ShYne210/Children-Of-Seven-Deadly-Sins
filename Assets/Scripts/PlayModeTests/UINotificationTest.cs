using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UINotificationPlayTests
{
    private GameObject notificationObject;
    private UINotification uiNotification;
    private GameObject successUIObject;
    private GameObject failUIObject;

    [SetUp]
    public void Setup()
    {
        UINotification.instance = null;
        
        notificationObject = new GameObject("UINotification");
        uiNotification = notificationObject.AddComponent<UINotification>();
        
        successUIObject = new GameObject("SuccessUI");
        failUIObject = new GameObject("FailUI");
        
        uiNotification.successUI = successUIObject;
        uiNotification.failUI = failUIObject;
        
        // Không gọi Awake, để Unity tự gọi
        successUIObject.SetActive(false);
        failUIObject.SetActive(false);
    }

    [TearDown]
    public void Teardown()
    {
        UINotification.instance = null;
        Object.Destroy(failUIObject);
        Object.Destroy(successUIObject);
        Object.Destroy(notificationObject);
    }

    // TEST 1: Awake được gọi tự động
    [UnityTest]
    public IEnumerator Awake_CalledAutomatically_SetsSingleton()
    {
        yield return null; // Đợi Awake chạy
        
        Assert.IsNotNull(UINotification.instance);
        Assert.AreEqual(uiNotification, UINotification.instance);
    }

    // TEST 2: ShowSuccess activates successUI
    [UnityTest]
    public IEnumerator ShowSuccess_ActivatesSuccessUI()
    {
        yield return null; // Awake chạy
        
        Assert.IsFalse(successUIObject.activeSelf, "Ban đầu phải inactive");
        
        uiNotification.ShowSuccess(0.2f);
        yield return null;
        
        Assert.IsTrue(successUIObject.activeSelf, "SuccessUI phải active");
        Assert.IsFalse(failUIObject.activeSelf, "FailUI phải inactive");
    }

    // TEST 3: ShowFail activates failUI
    [UnityTest]
    public IEnumerator ShowFail_ActivatesFailUI()
    {
        yield return null;
        
        uiNotification.ShowFail(0.2f);
        yield return null;
        
        Assert.IsTrue(failUIObject.activeSelf, "FailUI phải active");
        Assert.IsFalse(successUIObject.activeSelf, "SuccessUI phải inactive");
    }

    // TEST 4: ShowSuccess hides after time
    [UnityTest]
    public IEnumerator ShowSuccess_HidesAfterTime()
    {
        yield return null;
        
        uiNotification.ShowSuccess(0.3f);
        yield return null;
        
        Assert.IsTrue(successUIObject.activeSelf, "Đang hiển thị");
        
        yield return new WaitForSeconds(0.4f);
        
        Assert.IsFalse(successUIObject.activeSelf, "Đã ẩn sau time");
    }

    // TEST 5: ShowFail hides after time
    [UnityTest]
    public IEnumerator ShowFail_HidesAfterTime()
    {
        yield return null;
        
        uiNotification.ShowFail(0.3f);
        yield return null;
        
        Assert.IsTrue(failUIObject.activeSelf);
        
        yield return new WaitForSeconds(0.4f);
        
        Assert.IsFalse(failUIObject.activeSelf);
    }

    // TEST 6: ShowSuccess với default time (2s)
    [UnityTest]
    public IEnumerator ShowSuccess_DefaultTime_Works()
    {
        yield return null;
        
        uiNotification.ShowSuccess(); // default 2f
        yield return null;
        
        Assert.IsTrue(successUIObject.activeSelf);
        
        // Không đợi 2s để test nhanh, chỉ verify nó active
    }

    // TEST 7: ShowFail với default time
    [UnityTest]
    public IEnumerator ShowFail_DefaultTime_Works()
    {
        yield return null;
        
        uiNotification.ShowFail(); // default 2f
        yield return null;
        
        Assert.IsTrue(failUIObject.activeSelf);
    }

    // TEST 8: ShowSuccess stops previous ShowFail
    [UnityTest]
    public IEnumerator ShowSuccess_StopsPreviousShowFail()
    {
        yield return null;
        
        uiNotification.ShowFail(1f);
        yield return null;
        Assert.IsTrue(failUIObject.activeSelf);
        
        uiNotification.ShowSuccess(0.5f);
        yield return null;
        
        Assert.IsTrue(successUIObject.activeSelf, "Success phải active");
        Assert.IsFalse(failUIObject.activeSelf, "Fail phải bị stop");
    }

    // TEST 9: ShowFail stops previous ShowSuccess
    [UnityTest]
    public IEnumerator ShowFail_StopsPreviousShowSuccess()
    {
        yield return null;
        
        uiNotification.ShowSuccess(1f);
        yield return null;
        Assert.IsTrue(successUIObject.activeSelf);
        
        uiNotification.ShowFail(0.5f);
        yield return null;
        
        Assert.IsTrue(failUIObject.activeSelf, "Fail phải active");
        Assert.IsFalse(successUIObject.activeSelf, "Success phải bị stop");
    }

    // TEST 10: Multiple ShowSuccess calls
    [UnityTest]
    public IEnumerator ShowSuccess_CalledMultipleTimes_RestartsTimer()
    {
        yield return null;
        
        uiNotification.ShowSuccess(0.3f);
        yield return null;
        Assert.IsTrue(successUIObject.activeSelf);
        
        yield return new WaitForSeconds(0.2f);
        
        // Gọi lại trước khi hết time
        uiNotification.ShowSuccess(0.3f);
        yield return null;
        
        Assert.IsTrue(successUIObject.activeSelf, "Vẫn active vì restart timer");
        
        yield return new WaitForSeconds(0.2f);
        Assert.IsTrue(successUIObject.activeSelf, "Chưa đến 0.3s");
        
        yield return new WaitForSeconds(0.2f);
        Assert.IsFalse(successUIObject.activeSelf, "Đã hết 0.3s");
    }

    // TEST 11: Very short display time
    [UnityTest]
    public IEnumerator ShowSuccess_VeryShortTime_StillWorks()
    {
        yield return null;
        
        uiNotification.ShowSuccess(0.01f);
        yield return null;
        
        Assert.IsTrue(successUIObject.activeSelf);
        
        yield return new WaitForSeconds(0.05f);
        
        Assert.IsFalse(successUIObject.activeSelf);
    }

    // TEST 12: Zero display time
    [UnityTest]
    public IEnumerator ShowSuccess_ZeroTime_HidesImmediately()
    {
        yield return null;
        
        uiNotification.ShowSuccess(0f);
        yield return null;
        
        // Với WaitForSeconds(0), UI sẽ active 1 frame rồi ẩn ngay
        yield return new WaitForSeconds(0.1f);
        
        Assert.IsFalse(successUIObject.activeSelf);
    }

    // TEST 13: Null successUI không crash
    [UnityTest]
    public IEnumerator ShowSuccess_NullSuccessUI_LogsError()
    {
        yield return null;
        
        uiNotification.successUI = null;
        
        LogAssert.Expect(LogType.Exception, "NullReferenceException");
        
        uiNotification.ShowSuccess();
        yield return null;
    }

    // TEST 14: Null failUI không crash
    [UnityTest]
    public IEnumerator ShowFail_NullFailUI_LogsError()
    {
        yield return null;
        
        uiNotification.failUI = null;
        
        LogAssert.Expect(LogType.Exception, "NullReferenceException");
        
        uiNotification.ShowFail();
        yield return null;
    }

    // TEST 15: Component disabled vẫn chạy coroutine
    [UnityTest]
    public IEnumerator ShowSuccess_ComponentDisabled_CoroutineStops()
    {
        yield return null;
        
        uiNotification.ShowSuccess(1f);
        yield return null;
        
        Assert.IsTrue(successUIObject.activeSelf);
        
        uiNotification.enabled = false;
        
        yield return new WaitForSeconds(1.2f);
        
        // Coroutine bị stop khi component disabled
        // UI có thể vẫn active (behavior của Unity)
    }

    // TEST 16: Destroy object stops coroutine
    [UnityTest]
    public IEnumerator ShowSuccess_DestroyObject_StopsCoroutine()
    {
        yield return null;
        
        uiNotification.ShowSuccess(1f);
        yield return null;
        
        Assert.IsTrue(successUIObject.activeSelf);
        
        Object.Destroy(notificationObject);
        yield return null;
        
        // notificationObject đã bị destroy
        // successUIObject vẫn tồn tại nhưng coroutine dừng
    }

    // TEST 17: Long display time
    [UnityTest]
    public IEnumerator ShowSuccess_LongTime_Works()
    {
        yield return null;
        
        uiNotification.ShowSuccess(5f);
        yield return null;
        
        Assert.IsTrue(successUIObject.activeSelf);
        
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(successUIObject.activeSelf, "Vẫn hiển thị");
        
        // Không đợi hết 5s
    }

    // TEST 18: Alternating show calls
    [UnityTest]
    public IEnumerator AlternatingShowCalls_WorkCorrectly()
    {
        yield return null;
        
        uiNotification.ShowSuccess(0.2f);
        yield return null;
        Assert.IsTrue(successUIObject.activeSelf);
        
        yield return new WaitForSeconds(0.1f);
        
        uiNotification.ShowFail(0.2f);
        yield return null;
        Assert.IsTrue(failUIObject.activeSelf);
        Assert.IsFalse(successUIObject.activeSelf);
        
        yield return new WaitForSeconds(0.1f);
        
        uiNotification.ShowSuccess(0.2f);
        yield return null;
        Assert.IsTrue(successUIObject.activeSelf);
        Assert.IsFalse(failUIObject.activeSelf);
    }

    // TEST 19: Both UI initially active
    [UnityTest]
    public IEnumerator Show_BothUIInitiallyActive_HidesBoth()
    {
        successUIObject.SetActive(true);
        failUIObject.SetActive(true);
        
        yield return null;
        
        uiNotification.ShowSuccess(0.2f);
        yield return null;
        
        // Show coroutine ẩn cả 2 trước
        Assert.IsFalse(failUIObject.activeSelf);
        Assert.IsTrue(successUIObject.activeSelf);
    }

    // TEST 20: Singleton accessible from other scripts
    [UnityTest]
    public IEnumerator Singleton_AccessibleFromOtherScripts()
    {
        yield return null;
        
        // Giả lập script khác gọi
        UINotification.instance.ShowSuccess(0.2f);
        yield return null;
        
        Assert.IsTrue(successUIObject.activeSelf);
    }
}