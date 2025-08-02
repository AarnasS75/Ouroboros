using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private List<Sound> _sfxClips;

    [Header("Soundtracks")]
    [SerializeField] private List<Sound> _soundtrackClips;

    private Dictionary<AudioTitle, AudioSource> sfxSources = new();
    private Dictionary<AudioTitle, AudioSource> soundtrackSources = new();

    private void Awake()
    {
        InitializeAudioSources(_sfxClips, sfxSources);
        InitializeAudioSources(_soundtrackClips, soundtrackSources);
    }

    private void InitializeAudioSources(List<Sound> clips, Dictionary<AudioTitle, AudioSource> dict)
    {
        foreach (var data in clips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            
            source.clip = data.Clip;
            source.volume = data.Volume;
            source.loop = data.IsLooping;
            dict[data.Title] = source;
        }
    }

    public void PlaySFX(AudioTitle title)
    {
        if (sfxSources.TryGetValue(title, out var source)) {
            source.Play();
        } else {
            Debug.LogWarning($"SFX '{title}' not found!");
        }
    }

    public void PlaySoundtrack(AudioTitle title)
    {
        if (soundtrackSources.TryGetValue(title, out var source)) {
            source.Play();
        } else {
            Debug.LogWarning($"Soundtrack '{title}' not found!");
        }
    }

    public void StopSoundtrack(AudioTitle title)
    {
        if (soundtrackSources.TryGetValue(title, out var source)) {
            source.Stop();
        }
    }

    public void StopSFX(AudioTitle title)
    {
        if (sfxSources.TryGetValue(title, out var source)) {
            source.Stop();
        }
    }
}
