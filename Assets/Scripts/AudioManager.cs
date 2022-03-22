using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    Dictionary<AudioClip, float> audioLatencyMap;

    public List<AudioClip> musicList = new List<AudioClip>();
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioLatencyMap = new Dictionary<AudioClip, float>();
        
        audioSource.clip = musicList[SceneManager.GetActiveScene().buildIndex % musicList.Count];
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            if(SceneManager.GetActiveScene().buildIndex != 0) // not title screen
            audioSource.Play();
        } 

        foreach (var key in audioLatencyMap.Keys.ToList())
        {
            audioLatencyMap[key] -= Time.deltaTime;
            if (audioLatencyMap[key] < 0.0f) audioLatencyMap[key] = 0.0f;
        }
    }

    public void PlayOneShot(AudioClip audioClip, float volume = 1.0f)
    {
        if (!audioLatencyMap.ContainsKey(audioClip))
        {
            audioLatencyMap[audioClip] = 0.0f;
        }
        volume *= (0.0075f) / (audioLatencyMap[audioClip] + 0.0075f);
        StartCoroutine(PlayOneShotCoroutine(audioClip, volume, audioLatencyMap[audioClip]));
        audioLatencyMap[audioClip] += 0.015f;
    }

    IEnumerator PlayOneShotCoroutine(AudioClip audioClip, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(audioClip,volume);
    }
}
