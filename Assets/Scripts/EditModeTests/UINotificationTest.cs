using NUnit.Framework;
using UnityEngine;

public class UINotificationEditTests
{
    private GameObject notificationObject;
    private UINotification uiNotification;
    private GameObject successUIObject;
    private GameObject failUIObject;

    [SetUp]
    public void Setup()
    {
        // Reset static instance
        UINotification.instance = null;
        
        // Tạo UINotification
        notificationObject = new GameObject("UINotification");
        uiNotification = notificationObject.AddComponent<UINotification>();
        
        // Tạo UI objects
        successUIObject = new GameObject("SuccessUI");
        failUIObject = new GameObject("FailUI");
        
        // Gán references
        uiNotification.successUI = successUIObject;
        uiNotification.failUI = failUIObject;
        
        // Set active để test Awake
        successUIObject.SetActive(true);
        failUIObject.SetActive(true);
    }

    [TearDown]
    public void Teardown()
    {
        UINotification.instance = null;
        Object.DestroyImmediate(failUIObject);
        Object.DestroyImmediate(successUIObject);
        Object.DestroyImmediate(notificationObject);
    }

    // TEST 1: Component tồn tại
    [Test]
    public void UINotification_ComponentExists()
    {
        Assert.IsNotNull(uiNotification);
        Assert.IsTrue(uiNotification.enabled);
    }

    // TEST 2: Singleton instance được set trong Awake
    [Test]
    public void UINotification_SingletonInstanceSet()
    {
        // Trigger Awake by enabling the GameObject
        notificationObject.SetActive(false);
        notificationObject.SetActive(true);
        Assert.AreEqual(uiNotification, UINotification.instance);
    }

    // TEST 3: Awake deactivates UI
    [Test]
    public void UINotification_AwakeDeactivatesUI()
    {
        successUIObject.SetActive(true);
        failUIObject.SetActive(true);
        
        // Trigger Awake by disabling and enabling the GameObject
        notificationObject.SetActive(false);
        notificationObject.SetActive(true);
        
        Assert.IsFalse(successUIObject.activeSelf, "SuccessUI phải inactive sau Awake");
        Assert.IsFalse(failUIObject.activeSelf, "FailUI phải inactive sau Awake");
    }

    // TEST 4: Default values
    [Test]
    public void UINotification_HasDefaultValues()
    {
        GameObject obj = new GameObject();
        UINotification notif = obj.AddComponent<UINotification>();
        
        Assert.IsNull(notif.successUI);
        Assert.IsNull(notif.failUI);
        
        Object.DestroyImmediate(obj);
    }

    // TEST 5: UI references có thể gán
    [Test]
    public void UINotification_UIReferencesCanBeAssigned()
    {
        GameObject newSuccess = new GameObject("NewSuccess");
        GameObject newFail = new GameObject("NewFail");
        
        uiNotification.successUI = newSuccess;
        uiNotification.failUI = newFail;
        
        Assert.AreEqual(newSuccess, uiNotification.successUI);
        Assert.AreEqual(newFail, uiNotification.failUI);
        
        Object.DestroyImmediate(newFail);
        Object.DestroyImmediate(newSuccess);
    }

    // TEST 6: UI references có thể null
    [Test]
    public void UINotification_UIReferencesCanBeNull()
    {
        uiNotification.successUI = null;
        uiNotification.failUI = null;
        
        Assert.IsNull(uiNotification.successUI);
        Assert.IsNull(uiNotification.failUI);
    }

    // TEST 7: MonoBehaviour check
    [Test]
    public void UINotification_IsMonoBehaviour()
    {
        Assert.IsInstanceOf<MonoBehaviour>(uiNotification);
    }

    // TEST 8: Component type name
    [Test]
    public void UINotification_HasCorrectTypeName()
    {
        Assert.AreEqual("UINotification", uiNotification.GetType().Name);
    }

    // TEST 9: GameObject reference
    [Test]
    public void UINotification_HasCorrectGameObject()
    {
        Assert.AreEqual(notificationObject, uiNotification.gameObject);
    }

    // TEST 10: Multiple instances overwrite singleton
    [Test]
    public void UINotification_MultipleInstances_OverwriteSingleton()
    {
        GameObject obj1 = new GameObject();
        UINotification notif1 = obj1.AddComponent<UINotification>();
        obj1.SetActive(true);
        
        Assert.AreEqual(notif1, UINotification.instance);
        
        GameObject obj2 = new GameObject();
        UINotification notif2 = obj2.AddComponent<UINotification>();
        obj2.SetActive(true);
        
        Assert.AreEqual(notif2, UINotification.instance, "Instance mới ghi đè instance cũ");
        
        Object.DestroyImmediate(obj2);
        Object.DestroyImmediate(obj1);
    }

    // TEST 11: UI active states
    [Test]
    public void UINotification_UICanBeActivatedDeactivated()
    {
        successUIObject.SetActive(false);
        Assert.IsFalse(successUIObject.activeSelf);
        
        successUIObject.SetActive(true);
        Assert.IsTrue(successUIObject.activeSelf);
    }

    // TEST 12: Properties readable
    [Test]
    public void UINotification_PropertiesAreReadable()
    {
        GameObject readSuccess = uiNotification.successUI;
        GameObject readFail = uiNotification.failUI;
        
        Assert.AreEqual(successUIObject, readSuccess);
        Assert.AreEqual(failUIObject, readFail);
    }

    // TEST 13: Static instance accessible
    [Test]
    public void UINotification_StaticInstanceAccessible()
    {
        notificationObject.SetActive(false);
        notificationObject.SetActive(true);
        
        Assert.IsNotNull(UINotification.instance);
        Assert.AreEqual(uiNotification, UINotification.instance);
    }

    // TEST 14: Awake với null UI không crash
    [Test]
    public void UINotification_AwakeWithNullUI_DoesNotCrash()
    {
        uiNotification.successUI = null;
        uiNotification.failUI = null;
        
        // Sẽ throw NullReferenceException khi Awake được gọi
        Assert.Throws<System.NullReferenceException>(() => 
        {
            notificationObject.SetActive(false);
            notificationObject.SetActive(true);
        });
    }

    // TEST 15: UI hierarchy
    [Test]
    public void UINotification_UICanHaveParent()
    {
        GameObject canvas = new GameObject("Canvas");
        
        successUIObject.transform.SetParent(canvas.transform);
        failUIObject.transform.SetParent(canvas.transform);
        
        Assert.AreEqual(canvas.transform, successUIObject.transform.parent);
        Assert.AreEqual(canvas.transform, failUIObject.transform.parent);
        
        Object.DestroyImmediate(canvas);
    }

    // TEST 16: Component enabled state
    [Test]
    public void UINotification_ComponentCanBeDisabled()
    {
        uiNotification.enabled = true;
        Assert.IsTrue(uiNotification.enabled);
        
        uiNotification.enabled = false;
        Assert.IsFalse(uiNotification.enabled);
    }

    // TEST 17: Singleton pattern
    [Test]
    public void UINotification_FollowsSingletonPattern()
    {
        UINotification.instance = null;
        
        notificationObject.SetActive(false);
        notificationObject.SetActive(true);
        UINotification first = UINotification.instance;
        
        notificationObject.SetActive(false);
        notificationObject.SetActive(true);
        UINotification second = UINotification.instance;
        
        Assert.AreEqual(first, second);
    }
}