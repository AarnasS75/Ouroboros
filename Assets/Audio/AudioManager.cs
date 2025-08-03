using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; 
    
    [Header("SFX")]
    [SerializeField] private List<Sound> _sfxClips;

    [Header("Soundtracks")]
    [SerializeField] private List<Sound> _soundtrackClips;

    private Dictionary<AudioTitle, AudioSource> sfxSources = new();
    private Dictionary<AudioTitle, AudioSource> soundtrackSources = new();

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
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
    
    public void CrossfadeSoundtrack(AudioTitle fromTitle, AudioTitle toTitle, float duration)
    {
        if (!soundtrackSources.TryGetValue(fromTitle, out var fromSource))
        {
            Debug.LogWarning($"Soundtrack '{fromTitle}' not found!");
            return;
        }

        if (!soundtrackSources.TryGetValue(toTitle, out var toSource))
        {
            Debug.LogWarning($"Soundtrack '{toTitle}' not found!");
            return;
        }

        var toTargetVolume = GetSoundVolume(toTitle);
        StartCoroutine(Crossfade(fromSource, toSource, duration, toTargetVolume));
    }
    
    private float GetSoundVolume(AudioTitle title)
    {
        var sound = _soundtrackClips.Find(s => s.Title == title);
        return sound != null ? sound.Volume : 1f;
    }
    
    private IEnumerator Crossfade(AudioSource fromSource, AudioSource toSource, float duration, float toTargetVolume)
    {
        float time = 0f;

        float fromStartVolume = fromSource.volume;
        float toStartVolume = 0f;

        toSource.volume = 0f;
        toSource.Play();

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration * 1.3f;

            fromSource.volume = Mathf.Lerp(fromStartVolume, 0f, t);
            toSource.volume = Mathf.Lerp(toStartVolume, toTargetVolume, t);

            yield return null;
        }

        fromSource.Stop();
        fromSource.volume = fromStartVolume;
        toSource.volume = toTargetVolume;
    }

}
