using UnityEngine;

public class CutsceneBGM : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    [Range(0f, 1f)] public float volume = 0.5f;
    public bool playOnStart = true;
    public bool loop = true;

    void Start()
    {
        // Nếu chưa gán AudioSource thì tự lấy
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("Chưa gán AudioSource cho CutsceneBGM!");
            return;
        }

        audioSource.volume = volume;
        audioSource.loop = loop;

        if (playOnStart)
        {
            audioSource.Play();
        }
    }

    public void StopBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}