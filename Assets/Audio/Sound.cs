using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public AudioTitle Title;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume = 1f;
    public bool IsLooping;
}

public enum AudioTitle
{
    MenuSelect,
    WallHit,
    OstMainMenu,
    OstGameplay
}