using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource musicSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip cutsceneMusic;
    public AudioClip gameplayMusic;

    private void Awake()
    {
        // Singleton - chỉ giữ 1 AudioManager duy nhất
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // QUAN TRỌNG: không bị destroy khi đổi scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Nếu chưa gán AudioSource thì tự lấy
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicByScene(scene.name);
    }

    public void PlayMusicByScene(string sceneName)
    {
        if (musicSource == null)
        {
            Debug.LogError("Music Source is NULL!");
            return;
        }

        if (sceneName == "MainMenu")
        {
            PlayMusic(menuMusic);
        }
        else if (sceneName == "Cutscene")
        {
            PlayMusic(cutsceneMusic);
        }
        else if (sceneName == "Map2")
        {
            PlayMusic(gameplayMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio Clip is NULL!");
            return;
        }

        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}