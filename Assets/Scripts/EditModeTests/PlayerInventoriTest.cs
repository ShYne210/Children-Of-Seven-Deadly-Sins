using NUnit.Framework;
using UnityEngine;
using System.Reflection;

public class PlayerInventoriEditTests
{
    private GameObject playerObject;
    private PlayerInventori playerInventori;
    private FieldInfo keysField;

    [SetUp]
    public void Setup()
    {
        PlayerInventori.instance = null;
        
        playerObject = new GameObject("Player");
        playerInventori = playerObject.AddComponent<PlayerInventori>();
        
        // Lấy private field keys
        keysField = typeof(PlayerInventori).GetField("keys", 
            BindingFlags.NonPublic | BindingFlags.Instance);
    }

    private void CallAwake(PlayerInventori inventory)
    {
        typeof(PlayerInventori).GetMethod("Awake", 
            BindingFlags.NonPublic | BindingFlags.Instance).Invoke(inventory, null);
    }

    [TearDown]
    public void Teardown()
    {
        PlayerInventori.instance = null;
        Object.DestroyImmediate(playerObject);
    }

    // TEST 1: Component tồn tại
    [Test]
    public void PlayerInventori_ComponentExists()
    {
        Assert.IsNotNull(playerInventori);
        Assert.IsTrue(playerInventori.enabled);
    }

    // TEST 2: Singleton instance được set trong Awake
    [Test]
    public void PlayerInventori_SingletonInstanceSet()
    {
        CallAwake(playerInventori);
        Assert.AreEqual(playerInventori, PlayerInventori.instance);
    }

    // TEST 3: Keys list được khởi tạo
    [Test]
    public void PlayerInventori_KeysListInitialized()
    {
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.IsNotNull(keysList);
        Assert.AreEqual(0, keysList.Count);
    }

    // TEST 4: MonoBehaviour check
    [Test]
    public void PlayerInventori_IsMonoBehaviour()
    {
        Assert.IsInstanceOf<MonoBehaviour>(playerInventori);
    }

    // TEST 5: Component type name
    [Test]
    public void PlayerInventori_HasCorrectTypeName()
    {
        Assert.AreEqual("PlayerInventori", playerInventori.GetType().Name);
    }

    // TEST 6: GameObject reference
    [Test]
    public void PlayerInventori_HasCorrectGameObject()
    {
        Assert.AreEqual(playerObject, playerInventori.gameObject);
    }

    // TEST 7: Multiple instances overwrite singleton
    [Test]
    public void PlayerInventori_MultipleInstances_OverwriteSingleton()
    {
        GameObject obj1 = new GameObject();
        PlayerInventori inv1 = obj1.AddComponent<PlayerInventori>();
        CallAwake(inv1);
        
        Assert.AreEqual(inv1, PlayerInventori.instance);
        
        GameObject obj2 = new GameObject();
        PlayerInventori inv2 = obj2.AddComponent<PlayerInventori>();
        CallAwake(inv2);
        
        Assert.AreEqual(inv2, PlayerInventori.instance);
        
        Object.DestroyImmediate(obj2);
        Object.DestroyImmediate(obj1);
    }

    // TEST 8: Static instance accessible
    [Test]
    public void PlayerInventori_StaticInstanceAccessible()
    {
        CallAwake(playerInventori);
        
        Assert.IsNotNull(PlayerInventori.instance);
        Assert.AreEqual(playerInventori, PlayerInventori.instance);
    }

    // TEST 9: Keys list là List<string>
    [Test]
    public void PlayerInventori_KeysListIsStringList()
    {
        var keysList = keysField.GetValue(playerInventori);
        Assert.IsInstanceOf<System.Collections.Generic.List<string>>(keysList);
    }

    // TEST 10: Component enabled state
    [Test]
    public void PlayerInventori_ComponentCanBeDisabled()
    {
        playerInventori.enabled = true;
        Assert.IsTrue(playerInventori.enabled);
        
        playerInventori.enabled = false;
        Assert.IsFalse(playerInventori.enabled);
    }
}