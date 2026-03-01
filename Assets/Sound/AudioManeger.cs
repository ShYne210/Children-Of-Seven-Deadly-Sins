using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip cutsceneMusic;
    public AudioClip gameplayMusic; // optional

    private string currentScene;

    void Awake()
    {
        // Singleton để không bị tạo nhiều nhạc
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ nhạc khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        PlayMusicByScene(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicByScene(scene.name);
    }

    void PlayMusicByScene(string sceneName)
    {
        if (currentScene == sceneName) return;

        currentScene = sceneName;

        if (sceneName == "MainMenu")
        {
            PlayMusic(menuMusic);
        }
        else if (sceneName == "cutscene")
        {
            PlayMusic(cutsceneMusic);
        }
        else if (sceneName == "GameScene")
        {
            StopMusic(); // hoặc đổi sang gameplayMusic nếu muốn
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}