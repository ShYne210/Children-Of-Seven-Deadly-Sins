using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CutsceneBGMPlayTests
{
    private GameObject bgmObject;
    private CutsceneBGM cutsceneBGM;
    private AudioSource audioSource;

    [SetUp]
    public void Setup()
    {
        bgmObject = new GameObject("CutsceneBGM");
        audioSource = bgmObject.AddComponent<AudioSource>();
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        
        // Tạo AudioClip giả để test
        audioSource.clip = CreateDummyAudioClip();
        
        cutsceneBGM.audioSource = audioSource;
        cutsceneBGM.volume = 0.5f;
        cutsceneBGM.playOnStart = true;
        cutsceneBGM.loop = true;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(bgmObject);
    }

    // Helper: Tạo AudioClip dummy
    private AudioClip CreateDummyAudioClip()
    {
        // Tạo AudioClip 1 giây, 44100Hz, mono
        int sampleRate = 44100;
        int length = sampleRate * 1; // 1 second
        AudioClip clip = AudioClip.Create("DummyClip", length, 1, sampleRate, false);
        
        float[] samples = new float[length];
        // Tạo sine wave đơn giản
        for (int i = 0; i < length; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * 440 * i / sampleRate); // 440 Hz
        }
        clip.SetData(samples, 0);
        
        return clip;
    }

    // TEST 1: Start được gọi tự động
    [UnityTest]
    public IEnumerator Start_CalledAutomatically_ConfiguresAudioSource()
    {
        yield return null; // Đợi Start() chạy
        
        Assert.AreEqual(0.5f, audioSource.volume, 0.01f);
        Assert.IsTrue(audioSource.loop);
    }

    // TEST 2: PlayOnStart = true plays audio
    [UnityTest]
    public IEnumerator Start_PlayOnStartTrue_PlaysAudio()
    {
        cutsceneBGM.playOnStart = true;
        
        yield return null;
        
        Assert.IsTrue(audioSource.isPlaying, "Audio phải đang play");
    }

    // TEST 3: PlayOnStart = false không play
    [UnityTest]
    public IEnumerator Start_PlayOnStartFalse_DoesNotPlay()
    {
        // Tạo object mới để test Start từ đầu
        Object.Destroy(bgmObject);
        
        GameObject newObj = new GameObject("BGM");
        AudioSource newAudio = newObj.AddComponent<AudioSource>();
        newAudio.clip = CreateDummyAudioClip();
        CutsceneBGM newBGM = newObj.AddComponent<CutsceneBGM>();
        
        newBGM.audioSource = newAudio;
        newBGM.playOnStart = false;
        
        yield return null;
        
        Assert.IsFalse(newAudio.isPlaying, "Audio không được play");
        
        Object.Destroy(newObj);
    }

    // TEST 4: Volume được set đúng
    [UnityTest]
    public IEnumerator Start_SetsCorrectVolume()
    {
        cutsceneBGM.volume = 0.75f;
        
        // Recreate để test Start
        Object.Destroy(bgmObject);
        bgmObject = new GameObject("BGM");
        audioSource = bgmObject.AddComponent<AudioSource>();
        audioSource.clip = CreateDummyAudioClip();
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        cutsceneBGM.audioSource = audioSource;
        cutsceneBGM.volume = 0.75f;
        
        yield return null;
        
        Assert.AreEqual(0.75f, audioSource.volume, 0.01f);
    }

    // TEST 5: Loop được set đúng
    [UnityTest]
    public IEnumerator Start_SetsCorrectLoop()
    {
        Object.Destroy(bgmObject);
        bgmObject = new GameObject("BGM");
        audioSource = bgmObject.AddComponent<AudioSource>();
        audioSource.clip = CreateDummyAudioClip();
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        cutsceneBGM.audioSource = audioSource;
        cutsceneBGM.loop = false;
        
        yield return null;
        
        Assert.IsFalse(audioSource.loop);
    }

    // TEST 6: StopBGM stops playing audio
    [UnityTest]
    public IEnumerator StopBGM_StopsPlayingAudio()
    {
        yield return null;
        
        Assert.IsTrue(audioSource.isPlaying, "Audio đang play");
        
        cutsceneBGM.StopBGM();
        
        Assert.IsFalse(audioSource.isPlaying, "Audio đã stop");
    }

    // TEST 7: StopBGM khi audio không play
    [UnityTest]
    public IEnumerator StopBGM_WhenNotPlaying_DoesNothing()
    {
        cutsceneBGM.playOnStart = false;
        
        Object.Destroy(bgmObject);
        bgmObject = new GameObject("BGM");
        audioSource = bgmObject.AddComponent<AudioSource>();
        audioSource.clip = CreateDummyAudioClip();
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        cutsceneBGM.audioSource = audioSource;
        cutsceneBGM.playOnStart = false;
        
        yield return null;
        
        Assert.IsFalse(audioSource.isPlaying);
        
        cutsceneBGM.StopBGM(); // Không crash
        
        Assert.IsFalse(audioSource.isPlaying);
    }

    // TEST 8: StopBGM với null AudioSource
    [UnityTest]
    public IEnumerator StopBGM_NullAudioSource_DoesNotCrash()
    {
        yield return null;
        
        cutsceneBGM.audioSource = null;
        
        cutsceneBGM.StopBGM(); // Không crash
        
        Assert.Pass();
    }

    // TEST 9: AudioSource tự động lấy từ GetComponent
    [UnityTest]
    public IEnumerator Start_NullAudioSource_GetsFromComponent()
    {
        Object.Destroy(bgmObject);
        
        bgmObject = new GameObject("BGM");
        AudioSource autoAudioSource = bgmObject.AddComponent<AudioSource>();
        autoAudioSource.clip = CreateDummyAudioClip();
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        
        // Không gán audioSource
        cutsceneBGM.audioSource = null;
        
        yield return null;
        
        Assert.IsNotNull(cutsceneBGM.audioSource, "AudioSource phải được tự động lấy");
        Assert.AreEqual(autoAudioSource, cutsceneBGM.audioSource);
    }

    // TEST 10: Không có AudioSource logs error
    [UnityTest]
    public IEnumerator Start_NoAudioSource_LogsError()
    {
        Object.Destroy(bgmObject);
        
        bgmObject = new GameObject("BGM");
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        
        LogAssert.Expect(LogType.Error, "Chưa gán AudioSource cho CutsceneBGM!");
        
        yield return null;
    }

    // TEST 11: Multiple Start calls
    [UnityTest]
    public IEnumerator Start_CalledMultipleTimes_StillWorks()
    {
        yield return null;
        
        // Start đã chạy 1 lần
        Assert.IsTrue(audioSource.isPlaying);
        
        // Disable and re-enable to trigger Start again
        cutsceneBGM.enabled = false;
        yield return null;
        cutsceneBGM.enabled = true;
        yield return null;
        
        Assert.IsTrue(audioSource.isPlaying);
    }

    // TEST 12: StopBGM nhiều lần
    [UnityTest]
    public IEnumerator StopBGM_CalledMultipleTimes_DoesNotCrash()
    {
        yield return null;
        
        cutsceneBGM.StopBGM();
        Assert.IsFalse(audioSource.isPlaying);
        
        cutsceneBGM.StopBGM();
        cutsceneBGM.StopBGM();
        
        Assert.IsFalse(audioSource.isPlaying);
    }

    // TEST 13: Volume = 0 vẫn play
    [UnityTest]
    public IEnumerator Start_VolumeZero_StillPlays()
    {
        Object.Destroy(bgmObject);
        
        bgmObject = new GameObject("BGM");
        audioSource = bgmObject.AddComponent<AudioSource>();
        audioSource.clip = CreateDummyAudioClip();
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        cutsceneBGM.audioSource = audioSource;
        cutsceneBGM.volume = 0f;
        cutsceneBGM.playOnStart = true;
        
        yield return null;
        
        Assert.IsTrue(audioSource.isPlaying);
        Assert.AreEqual(0f, audioSource.volume, 0.01f);
    }

    // TEST 14: Component disabled không chạy Start
    [UnityTest]
    public IEnumerator Start_ComponentDisabled_DoesNotRun()
    {
        Object.Destroy(bgmObject);
        
        bgmObject = new GameObject("BGM");
        audioSource = bgmObject.AddComponent<AudioSource>();
        audioSource.clip = CreateDummyAudioClip();
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        cutsceneBGM.audioSource = audioSource;
        cutsceneBGM.enabled = false;
        
        yield return null;
        
        Assert.IsFalse(audioSource.isPlaying, "Start không chạy khi disabled");
    }

    // TEST 15: Enable component sau đó
    [UnityTest]
    public IEnumerator Component_EnabledAfterStart_DoesNotPlayAgain()
    {
        Object.Destroy(bgmObject);
        
        bgmObject = new GameObject("BGM");
        audioSource = bgmObject.AddComponent<AudioSource>();
        audioSource.clip = CreateDummyAudioClip();
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        cutsceneBGM.audioSource = audioSource;
        cutsceneBGM.enabled = false;
        
        yield return null;
        
        cutsceneBGM.enabled = true;
        yield return null;
        
        // Start chỉ chạy 1 lần
        Assert.IsFalse(audioSource.isPlaying);
    }

    // TEST 16: Null AudioClip vẫn không crash
    [UnityTest]
    public IEnumerator Start_NullAudioClip_DoesNotCrash()
    {
        Object.Destroy(bgmObject);
        
        bgmObject = new GameObject("BGM");
        audioSource = bgmObject.AddComponent<AudioSource>();
        audioSource.clip = null; // No clip
        cutsceneBGM = bgmObject.AddComponent<CutsceneBGM>();
        cutsceneBGM.audioSource = audioSource;
        
        yield return null;
        
        // Audio vẫn "play" nhưng không có sound
        Assert.Pass();
    }

    // TEST 17: AudioSource isPlaying state
    [UnityTest]
    public IEnumerator AudioSource_IsPlayingState_CorrectlyTracked()
    {
        yield return null;
        
        Assert.IsTrue(audioSource.isPlaying);
        
        audioSource.Pause();
        Assert.IsFalse(audioSource.isPlaying);
        
        audioSource.UnPause();
        Assert.IsTrue(audioSource.isPlaying);
        
        cutsceneBGM.StopBGM();
        Assert.IsFalse(audioSource.isPlaying);
    }

    // TEST 18: Loop setting works
    [UnityTest]
    public IEnumerator Loop_Setting_WorksCorrectly()
    {
        yield return null;
        
        Assert.IsTrue(audioSource.loop, "Loop phải true");
        
        // Đợi clip chạy hết
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);
        
        // Vẫn play vì loop = true
        Assert.IsTrue(audioSource.isPlaying, "Vẫn play vì loop");
    }
}