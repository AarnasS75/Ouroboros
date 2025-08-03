using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; 
    
    [Header("SFX")]
    [SerializeField] private List<Sfx> _sfxClips;

    [Header("Soundtracks")]
    [SerializeField] private List<Song> _soundtrackClips;

    private Dictionary<SfxTitle, AudioSource> sfxSources = new();
    private Dictionary<SongTitle, AudioSource> soundtrackSources = new();

    private bool _isMuted;
    
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
        InitializeSfxSources(_sfxClips, sfxSources);
        InitializeSongSources(_soundtrackClips, soundtrackSources);
    }

    public void ToggleMute()
    {
        _isMuted = !_isMuted;
        
        foreach (var source in sfxSources.Values)
        {
            source.mute = _isMuted;
        }

        foreach (var source in soundtrackSources.Values)
        {
            source.mute = _isMuted;
        }
        
        PlayerPrefs.SetInt("Muted", _isMuted ? 1 : 0);
    }

    public void AdjustSfxVolume(Image fillImage)
    {
        foreach (var source in sfxSources.Values)
        {
            source.volume = fillImage.fillAmount;
        }
        
        PlayerPrefs.SetFloat("Sfx", fillImage.fillAmount);
    }
    
    public void AdjustMasterVolume(Image fillImage)
    {
        foreach (var source in soundtrackSources.Values)
        {
            source.volume = fillImage.fillAmount;
        }
        
        PlayerPrefs.SetFloat("Master", fillImage.fillAmount);
    }
    
    private void InitializeSfxSources(List<Sfx> clips, Dictionary<SfxTitle, AudioSource> dict)
    {
        foreach (var data in clips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            
            source.clip = data.Clip;
            source.volume = PlayerPrefs.GetFloat("Sfx", 1f);
            source.loop = data.IsLooping;
            dict[data.Title] = source;
            source.mute = PlayerPrefs.GetInt("Muted", 0) == 1;
        }
    }
    
    private void InitializeSongSources(List<Song> clips, Dictionary<SongTitle, AudioSource> dict)
    {
        foreach (var data in clips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            
            source.clip = data.Clip;
            source.volume = PlayerPrefs.GetFloat("Master", 1f);
            source.loop = data.IsLooping;
            dict[data.Title] = source;
            source.mute = PlayerPrefs.GetInt("Muted", 0) == 1;
        }
    }

    public void PlaySFX(SfxTitle title)
    {
        if (sfxSources.TryGetValue(title, out var source)) {
            source.Play();
        } else {
            Debug.LogWarning($"SFX '{title}' not found!");
        }
    }

    public void PlaySoundtrack(SongTitle title)
    {
        if (soundtrackSources.TryGetValue(title, out var source)) {
            source.Play();
        } else {
            Debug.LogWarning($"Soundtrack '{title}' not found!");
        }
    }
    
    public void CrossfadeSoundtrack(SongTitle fromTitle, SongTitle toTitle, float duration)
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

        StartCoroutine(Crossfade(fromSource, toSource, duration));
    }
    
    private IEnumerator Crossfade(AudioSource fromSource, AudioSource toSource, float duration)
    {
        float fadeOutDuration = duration * 0.8f;
        float fadeInDuration = duration - fadeOutDuration;

        float fromStartVolume = fromSource.volume;
        float toTargetVolume = PlayerPrefs.GetFloat("Master", 1f);

        // Start fade-out
        float time = 0f;
        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeOutDuration;
            fromSource.volume = Mathf.Lerp(fromStartVolume, 0f, t);
            yield return null;
        }

        fromSource.Stop();
        fromSource.volume = fromStartVolume;

        // Start fade-in
        toSource.volume = 0f;
        toSource.Play();

        time = 0f;
        while (time < fadeInDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeInDuration;
            toSource.volume = Mathf.Lerp(0f, toTargetVolume, t);
            yield return null;
        }

        toSource.volume = toTargetVolume;
    }
}
