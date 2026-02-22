using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAudioManager : MonoBehaviour {
    public AudioSource voiceSource;
    public AudioClip[] randomClips;
    public float interval = 5f; // thời gian chờ sau mỗi câu
    private List<AudioClip> clipPool = new List<AudioClip>();

    void Start() {
        ResetClipPool();
        StartCoroutine(RandomVoiceRoutine());
    }

    IEnumerator RandomVoiceRoutine() {
        while (true) {
            if (!voiceSource.isPlaying) {
                PlayRandomClip();
                // chờ cho clip phát xong + thêm interval
                yield return new WaitForSeconds(voiceSource.clip.length + interval);
            } else {
                yield return null;
            }
        }
    }

    void PlayRandomClip() {
        if (clipPool.Count == 0) ResetClipPool();

        int i = Random.Range(0, clipPool.Count);
        AudioClip chosen = clipPool[i];
        clipPool.RemoveAt(i);

        voiceSource.clip = chosen;
        voiceSource.Play();
        Debug.Log("Playing clip: " + chosen.name);
    }

    void ResetClipPool() {
        clipPool.Clear();
        clipPool.AddRange(randomClips);
        Debug.Log("Clip pool reset!");
    }
}
