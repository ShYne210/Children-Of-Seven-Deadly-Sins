using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class PlayerInventoriPlayTests
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
        
        keysField = typeof(PlayerInventori).GetField("keys", 
            BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [TearDown]
    public void Teardown()
    {
        PlayerInventori.instance = null;
        Object.Destroy(playerObject);
    }

    // TEST 1: Awake được gọi tự động
    [UnityTest]
    public IEnumerator Awake_CalledAutomatically_SetsSingleton()
    {
        yield return null;
        
        Assert.IsNotNull(PlayerInventori.instance);
        Assert.AreEqual(playerInventori, PlayerInventori.instance);
    }

    // TEST 2: AddKey thêm key mới
    [UnityTest]
    public IEnumerator AddKey_NewKey_AddsToList()
    {
        yield return null;
        
        playerInventori.AddKey("key1");
        
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.AreEqual(1, keysList.Count);
        Assert.Contains("key1", keysList);
    }

    // TEST 3: AddKey nhiều keys khác nhau
    [UnityTest]
    public IEnumerator AddKey_MultipleKeys_AddsAll()
    {
        yield return null;
        
        playerInventori.AddKey("key1");
        playerInventori.AddKey("key2");
        playerInventori.AddKey("key3");
        
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.AreEqual(3, keysList.Count);
        Assert.Contains("key1", keysList);
        Assert.Contains("key2", keysList);
        Assert.Contains("key3", keysList);
    }

    // TEST 4: AddKey duplicate key không thêm
    [UnityTest]
    public IEnumerator AddKey_DuplicateKey_DoesNotAdd()
    {
        yield return null;
        
        playerInventori.AddKey("key1");
        playerInventori.AddKey("key1");
        playerInventori.AddKey("key1");
        
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.AreEqual(1, keysList.Count);
        Assert.Contains("key1", keysList);
    }

    // TEST 5: HasKey trả về true khi có key
    [UnityTest]
    public IEnumerator HasKey_ExistingKey_ReturnsTrue()
    {
        yield return null;
        
        playerInventori.AddKey("redKey");
        
        Assert.IsTrue(playerInventori.HasKey("redKey"));
    }

    // TEST 6: HasKey trả về false khi không có key
    [UnityTest]
    public IEnumerator HasKey_NonExistingKey_ReturnsFalse()
    {
        yield return null;
        
        playerInventori.AddKey("blueKey");
        
        Assert.IsFalse(playerInventori.HasKey("redKey"));
    }

    // TEST 7: HasKey với list rỗng
    [UnityTest]
    public IEnumerator HasKey_EmptyList_ReturnsFalse()
    {
        yield return null;
        
        Assert.IsFalse(playerInventori.HasKey("anyKey"));
    }

    // TEST 8: AddKey với empty string
    [UnityTest]
    public IEnumerator AddKey_EmptyString_Adds()
    {
        yield return null;
        
        playerInventori.AddKey("");
        
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.AreEqual(1, keysList.Count);
        Assert.IsTrue(playerInventori.HasKey(""));
    }

    // TEST 9: AddKey với null
    [UnityTest]
    public IEnumerator AddKey_Null_Adds()
    {
        yield return null;
        
        playerInventori.AddKey(null);
        
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.AreEqual(1, keysList.Count);
        Assert.IsTrue(playerInventori.HasKey(null));
    }

    // TEST 10: Case sensitive keys
    [UnityTest]
    public IEnumerator AddKey_CaseSensitive_TreatsAsDifferent()
    {
        yield return null;
        
        playerInventori.AddKey("Key");
        playerInventori.AddKey("key");
        playerInventori.AddKey("KEY");
        
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.AreEqual(3, keysList.Count);
        Assert.IsTrue(playerInventori.HasKey("Key"));
        Assert.IsTrue(playerInventori.HasKey("key"));
        Assert.IsTrue(playerInventori.HasKey("KEY"));
    }

    // TEST 11: AddKey nhiều lần cùng key
    [UnityTest]
    public IEnumerator AddKey_SameKeyMultipleTimes_CountStaysOne()
    {
        yield return null;
        
        for (int i = 0; i < 10; i++)
        {
            playerInventori.AddKey("masterKey");
        }
        
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.AreEqual(1, keysList.Count);
    }

    // TEST 12: Complete flow - Add then check
    [UnityTest]
    public IEnumerator CompleteFlow_AddThenCheck_Works()
    {
        yield return null;
        
        Assert.IsFalse(playerInventori.HasKey("door1"));
        
        playerInventori.AddKey("door1");
        
        Assert.IsTrue(playerInventori.HasKey("door1"));
    }

    // TEST 13: Multiple different keys check
    [UnityTest]
    public IEnumerator HasKey_MultipleKeys_ChecksCorrectly()
    {
        yield return null;
        
        playerInventori.AddKey("red");
        playerInventori.AddKey("blue");
        
        Assert.IsTrue(playerInventori.HasKey("red"));
        Assert.IsTrue(playerInventori.HasKey("blue"));
        Assert.IsFalse(playerInventori.HasKey("green"));
    }

    // TEST 14: Singleton accessible from other code
    [UnityTest]
    public IEnumerator Singleton_AccessibleGlobally()
    {
        yield return null;
        
        PlayerInventori.instance.AddKey("globalKey");
        
        Assert.IsTrue(PlayerInventori.instance.HasKey("globalKey"));
    }

    // TEST 15: Long key names
    [UnityTest]
    public IEnumerator AddKey_LongKeyName_Works()
    {
        yield return null;
        
        string longKey = new string('A', 1000);
        playerInventori.AddKey(longKey);
        
        Assert.IsTrue(playerInventori.HasKey(longKey));
    }

    // TEST 16: Special characters in key
    [UnityTest]
    public IEnumerator AddKey_SpecialCharacters_Works()
    {
        yield return null;
        
        playerInventori.AddKey("key_123");
        playerInventori.AddKey("key-456");
        playerInventori.AddKey("key@#$");
        
        Assert.IsTrue(playerInventori.HasKey("key_123"));
        Assert.IsTrue(playerInventori.HasKey("key-456"));
        Assert.IsTrue(playerInventori.HasKey("key@#$"));
    }

    // TEST 17: Numeric keys
    [UnityTest]
    public IEnumerator AddKey_NumericStrings_Works()
    {
        yield return null;
        
        playerInventori.AddKey("1");
        playerInventori.AddKey("2");
        playerInventori.AddKey("123");
        
        Assert.IsTrue(playerInventori.HasKey("1"));
        Assert.IsTrue(playerInventori.HasKey("2"));
        Assert.IsTrue(playerInventori.HasKey("123"));
    }

    // TEST 18: Whitespace keys
    [UnityTest]
    public IEnumerator AddKey_Whitespace_Works()
    {
        yield return null;
        
        playerInventori.AddKey(" ");
        playerInventori.AddKey("  ");
        playerInventori.AddKey("\t");
        
        Assert.IsTrue(playerInventori.HasKey(" "));
        Assert.IsTrue(playerInventori.HasKey("  "));
        Assert.IsTrue(playerInventori.HasKey("\t"));
    }

    // TEST 19: Order of keys maintained
    [UnityTest]
    public IEnumerator AddKey_Order_Maintained()
    {
        yield return null;
        
        playerInventori.AddKey("first");
        playerInventori.AddKey("second");
        playerInventori.AddKey("third");
        
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.AreEqual("first", keysList[0]);
        Assert.AreEqual("second", keysList[1]);
        Assert.AreEqual("third", keysList[2]);
    }

    // TEST 20: Large number of keys
    [UnityTest]
    public IEnumerator AddKey_ManyKeys_AllAdded()
    {
        yield return null;
        
        for (int i = 0; i < 100; i++)
        {
            playerInventori.AddKey($"key{i}");
        }
        
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.AreEqual(100, keysList.Count);
        
        for (int i = 0; i < 100; i++)
        {
            Assert.IsTrue(playerInventori.HasKey($"key{i}"));
        }
    }

    // TEST 21: Component disabled vẫn hoạt động
    [UnityTest]
    public IEnumerator AddKey_ComponentDisabled_StillWorks()
    {
        yield return null;
        
        playerInventori.enabled = false;
        
        playerInventori.AddKey("key1");
        
        Assert.IsTrue(playerInventori.HasKey("key1"));
    }

    // TEST 22: AddKey và HasKey liên tiếp
    [UnityTest]
    public IEnumerator AddKeyAndHasKey_Alternating_Works()
    {
        yield return null;
        
        playerInventori.AddKey("a");
        Assert.IsTrue(playerInventori.HasKey("a"));
        
        playerInventori.AddKey("b");
        Assert.IsTrue(playerInventori.HasKey("b"));
        Assert.IsTrue(playerInventori.HasKey("a"));
        
        playerInventori.AddKey("c");
        Assert.IsTrue(playerInventori.HasKey("c"));
        Assert.IsTrue(playerInventori.HasKey("b"));
        Assert.IsTrue(playerInventori.HasKey("a"));
    }

    // TEST 23: Unicode characters
    [UnityTest]
    public IEnumerator AddKey_UnicodeCharacters_Works()
    {
        yield return null;
        
        playerInventori.AddKey("钥匙");
        playerInventori.AddKey("🔑");
        playerInventori.AddKey("клавиша");
        
        Assert.IsTrue(playerInventori.HasKey("钥匙"));
        Assert.IsTrue(playerInventori.HasKey("🔑"));
        Assert.IsTrue(playerInventori.HasKey("клавиша"));
    }

    // TEST 24: Duplicate check với mixed case
    [UnityTest]
    public IEnumerator AddKey_DuplicateMixedCase_AddsMultiple()
    {
        yield return null;
        
        playerInventori.AddKey("Key");
        playerInventori.AddKey("Key"); // Duplicate exact
        playerInventori.AddKey("key"); // Different case
        
        var keysList = keysField.GetValue(playerInventori) as System.Collections.Generic.List<string>;
        Assert.AreEqual(2, keysList.Count); // "Key" và "key"
    }

    // TEST 25: List persistence across frames
    [UnityTest]
    public IEnumerator KeysList_PersistsAcrossFrames()
    {
        yield return null;
        
        playerInventori.AddKey("persistent");
        
        yield return null;
        yield return null;
        yield return null;
        
        Assert.IsTrue(playerInventori.HasKey("persistent"));
    }
}