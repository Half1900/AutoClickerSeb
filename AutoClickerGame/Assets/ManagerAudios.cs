using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ManagerAudios : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SfxSlider;
    public bool Music = true;
    public bool SFX = true;
    public Image SoundImage;
    public Image MusicImage;
    public Sprite MutedSound;
    public Sprite MutedMusic;
    public Sprite UnMutedMusic;
    public Sprite UnMutedSfx;
    private void Start()
    {
        SetMusicVolume();
        SetSFXVolume();
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 1);
    }
    public void SetSFXVolume()
    {
        float volume = SfxSlider.value;
        audioMixer.SetFloat("Sound", Mathf.Log10(volume) * 1);
    }

    public void MusicMuted()
    {
        if (Music)
        {
            audioMixer.SetFloat("Music", -80f);
            MusicImage.sprite = MutedMusic;
            Music = false;
        }
        else
        {
            float volume = SfxSlider.value;
            audioMixer.SetFloat("Music", Mathf.Log10(volume) * 1);
            MusicImage.sprite = UnMutedMusic;
            Music = true;
        }
    }
    public void SFXMuted()
    {
        if (SFX)
        {
            audioMixer.SetFloat("Sound", -80f);
            SoundImage.sprite = MutedSound;
            SFX = false;
        }
        else
        {
            float volume = SfxSlider.value;
            audioMixer.SetFloat("Sound", Mathf.Log10(volume) * 1);
            SoundImage.sprite = UnMutedSfx;
            SFX = true;
        }
    }
}
