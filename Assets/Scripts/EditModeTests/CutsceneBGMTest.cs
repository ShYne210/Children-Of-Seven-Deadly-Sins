using NUnit.Framework;
using UnityEngine;

public class CutsceneBGMEditTests
{
    private GameObject bgmObject;
    private CutsceneBGM cutsceneBGM;
    private AudioSource audioSource;

    [SetUp]
    public void Setup()
    {
        bgmObject = new GameObject("CutsceneBGM");
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        audioSource = bgmObject.AddComponent<AudioSource>();
        
        cutsceneBGM.audioSource = audioSource;
        cutsceneBGM.volume = 0.5f;
        cutsceneBGM.playOnStart = true;
        cutsceneBGM.loop = true;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(bgmObject);
    }

    // TEST 1: Component tồn tại
    [Test]
    public void CutsceneBGM_ComponentExists()
    {
        Assert.IsNotNull(cutsceneBGM);
        Assert.IsTrue(cutsceneBGM.enabled);
    }

    // TEST 2: Default values
    [Test]
    public void CutsceneBGM_HasDefaultValues()
    {
        GameObject obj = new GameObject();
        CutsceneBGM bgm = obj.AddComponent<CutsceneBGM>();
        
        Assert.AreEqual(0.5f, bgm.volume, 0.01f);
        Assert.IsTrue(bgm.playOnStart);
        Assert.IsTrue(bgm.loop);
        Assert.IsNull(bgm.audioSource);
        
        Object.DestroyImmediate(obj);
    }

    // TEST 3: AudioSource có thể gán
    [Test]
    public void CutsceneBGM_AudioSourceCanBeAssigned()
    {
        GameObject newObj = new GameObject();
        AudioSource newAudioSource = newObj.AddComponent<AudioSource>();
        
        cutsceneBGM.audioSource = newAudioSource;
        
        Assert.AreEqual(newAudioSource, cutsceneBGM.audioSource);
        
        Object.DestroyImmediate(newObj);
    }

    // TEST 4: Volume có thể thay đổi
    [Test]
    public void CutsceneBGM_VolumeCanBeChanged()
    {
        cutsceneBGM.volume = 0.0f;
        Assert.AreEqual(0.0f, cutsceneBGM.volume, 0.01f);
        
        cutsceneBGM.volume = 1.0f;
        Assert.AreEqual(1.0f, cutsceneBGM.volume, 0.01f);
        
        cutsceneBGM.volume = 0.75f;
        Assert.AreEqual(0.75f, cutsceneBGM.volume, 0.01f);
    }

    // TEST 5: PlayOnStart có thể toggle
    [Test]
    public void CutsceneBGM_PlayOnStartCanBeToggled()
    {
        cutsceneBGM.playOnStart = false;
        Assert.IsFalse(cutsceneBGM.playOnStart);
        
        cutsceneBGM.playOnStart = true;
        Assert.IsTrue(cutsceneBGM.playOnStart);
    }

    // TEST 6: Loop có thể toggle
    [Test]
    public void CutsceneBGM_LoopCanBeToggled()
    {
        cutsceneBGM.loop = false;
        Assert.IsFalse(cutsceneBGM.loop);
        
        cutsceneBGM.loop = true;
        Assert.IsTrue(cutsceneBGM.loop);
    }

    // TEST 7: AudioSource có thể null
    [Test]
    public void CutsceneBGM_AudioSourceCanBeNull()
    {
        cutsceneBGM.audioSource = null;
        Assert.IsNull(cutsceneBGM.audioSource);
    }

    // TEST 8: MonoBehaviour check
    [Test]
    public void CutsceneBGM_IsMonoBehaviour()
    {
        Assert.IsInstanceOf<MonoBehaviour>(cutsceneBGM);
    }

    // TEST 9: Component type name
    [Test]
    public void CutsceneBGM_HasCorrectTypeName()
    {
        Assert.AreEqual("CutsceneBGM", cutsceneBGM.GetType().Name);
    }

    // TEST 10: GameObject reference
    [Test]
    public void CutsceneBGM_HasCorrectGameObject()
    {
        Assert.AreEqual(bgmObject, cutsceneBGM.gameObject);
    }

    // TEST 11: Volume range validation
    [Test]
    public void CutsceneBGM_VolumeAcceptsValidRange()
    {
        // Unity Range attribute chỉ validate trong Inspector
        // Code có thể set giá trị ngoài range
        cutsceneBGM.volume = -0.5f;
        Assert.AreEqual(-0.5f, cutsceneBGM.volume, 0.01f);
        
        cutsceneBGM.volume = 1.5f;
        Assert.AreEqual(1.5f, cutsceneBGM.volume, 0.01f);
    }

    // TEST 12: AudioSource component on same GameObject
    [Test]
    public void CutsceneBGM_AudioSourceOnSameGameObject()
    {
        AudioSource sameAudioSource = bgmObject.GetComponent<AudioSource>();
        Assert.IsNotNull(sameAudioSource);
        Assert.AreEqual(audioSource, sameAudioSource);
    }

    // TEST 13: Multiple CutsceneBGM instances
    [Test]
    public void CutsceneBGM_CanHaveMultipleInstances()
    {
        GameObject obj1 = new GameObject();
        CutsceneBGM bgm1 = obj1.AddComponent<CutsceneBGM>();
        bgm1.volume = 0.3f;
        
        GameObject obj2 = new GameObject();
        CutsceneBGM bgm2 = obj2.AddComponent<CutsceneBGM>();
        bgm2.volume = 0.7f;
        
        Assert.AreNotEqual(bgm1.volume, bgm2.volume);
        
        Object.DestroyImmediate(obj2);
        Object.DestroyImmediate(obj1);
    }

    // TEST 14: Properties readable
    [Test]
    public void CutsceneBGM_PropertiesAreReadable()
    {
        float readVolume = cutsceneBGM.volume;
        bool readPlayOnStart = cutsceneBGM.playOnStart;
        bool readLoop = cutsceneBGM.loop;
        AudioSource readAudioSource = cutsceneBGM.audioSource;
        
        Assert.AreEqual(0.5f, readVolume, 0.01f);
        Assert.IsTrue(readPlayOnStart);
        Assert.IsTrue(readLoop);
        Assert.AreEqual(audioSource, readAudioSource);
    }

    // TEST 15: AudioSource settings readable
    [Test]
    public void CutsceneBGM_AudioSourceHasDefaultSettings()
    {
        Assert.AreEqual(1.0f, audioSource.volume, 0.01f); // Default Unity AudioSource volume
        Assert.IsFalse(audioSource.loop); // Default loop
        Assert.IsFalse(audioSource.playOnAwake); // Default playOnAwake
    }

    // TEST 16: Component enabled state
    [Test]
    public void CutsceneBGM_ComponentCanBeDisabled()
    {
        cutsceneBGM.enabled = true;
        Assert.IsTrue(cutsceneBGM.enabled);
        
        cutsceneBGM.enabled = false;
        Assert.IsFalse(cutsceneBGM.enabled);
    }

    // TEST 17: GameObject with multiple AudioSources
    [Test]
    public void CutsceneBGM_GameObjectCanHaveMultipleAudioSources()
    {
        AudioSource secondAudioSource = bgmObject.AddComponent<AudioSource>();
        
        AudioSource[] sources = bgmObject.GetComponents<AudioSource>();
        Assert.AreEqual(2, sources.Length);
        
        Object.DestroyImmediate(secondAudioSource);
    }

    // TEST 18: Volume precision
    [Test]
    public void CutsceneBGM_VolumePrecision()
    {
        cutsceneBGM.volume = 0.123456789f;
        Assert.AreEqual(0.123456789f, cutsceneBGM.volume, 0.0001f);
    }
}