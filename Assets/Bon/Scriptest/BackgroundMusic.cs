using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip quest1Music;
    [SerializeField] private AudioClip quest2Music;

    void Start()
    {
        PlayQuest1Music();
    }

    public void PlayQuest1Music()
    {
        audioSource.clip = quest1Music;
        audioSource.Play();
    }

    public void PlayQuest2Music()
    {
        audioSource.clip = quest2Music;
        audioSource.Play();
    }
}
