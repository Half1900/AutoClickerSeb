using UnityEngine;

public enum SoundType
{
    Music,
    SoundEffect,
    Dialogue
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    public bool playOnAwake;
    public bool loop;
    public SoundType type;
    [HideInInspector]
    public AudioSource source;
}
