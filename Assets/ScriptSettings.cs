using UnityEngine;
using UnityEngine.Audio;

public class SettingsUI : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
}
