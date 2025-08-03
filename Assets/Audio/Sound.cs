using System;
using UnityEngine;

[Serializable]
public abstract class Sound
{
    public AudioClip Clip;
    public bool IsLooping;
}

[Serializable]
public class Sfx : Sound
{
    public SfxTitle Title;
}

[Serializable]
public class Song : Sound
{
    public SongTitle Title;
}

public enum SfxTitle
{
    MenuSelect,
    WallHit,
    FoodConsumed
}

public enum SongTitle
{
    OstMainMenu,
    OstGameplay
}