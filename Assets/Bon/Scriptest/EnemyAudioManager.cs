using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAudioManager : MonoBehaviour
{
    [Header("===== Âm thanh =====")]
    public AudioSource voiceSource;
    public AudioClip[] randomClips;
    public float interval = 5f;

    private List<AudioClip> clipPool = new List<AudioClip>();

    void Start()
    {
        ResetClipPool();
        StartCoroutine(RandomVoiceRoutine());
    }

    IEnumerator RandomVoiceRoutine()
    {
        while (true)
        {
            if (!voiceSource.isPlaying)
            {
                PlayRandomClip();
                yield return new WaitForSeconds(voiceSource.clip.length + interval);
            }
            else yield return null;
        }
    }

    void PlayRandomClip()
    {
        if (clipPool.Count == 0) ResetClipPool();
        int i = Random.Range(0, clipPool.Count);
        AudioClip chosen = clipPool[i];
        clipPool.RemoveAt(i);
        voiceSource.clip = chosen;
        voiceSource.Play();
    }

    void ResetClipPool()
    {
        clipPool.Clear();
        clipPool.AddRange(randomClips);
    }
    public void SwitchToQuest2Clips()
    {
        // Reset pool với clip mới (hung dữ hơn)
        clipPool.Clear();
        clipPool.AddRange(randomClips); // ở đây cậu có thể gán mảng clip khác cho Quest 2
        interval = Mathf.Max(1f, interval - 2f); // giảm khoảng cách giữa các tiếng
    }

}
