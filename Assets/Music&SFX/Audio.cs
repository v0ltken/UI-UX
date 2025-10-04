using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    // Temporary values (not saved until Apply)
    private float tempMusic;
    private float tempSfx;

    private void Start()
    {
        // Load saved values or default 0.75
        float musicVol = PlayerPrefs.HasKey("MusicVol") ? PlayerPrefs.GetFloat("MusicVol") : 0.75f;
        float sfxVol   = PlayerPrefs.HasKey("SFXVol")   ? PlayerPrefs.GetFloat("SFXVol")   : 0.75f;

        // Set sliders visually (without triggering events)
        musicSlider.SetValueWithoutNotify(musicVol);
        sfxSlider.SetValueWithoutNotify(sfxVol);

        // Store in temp
        tempMusic = musicVol;
        tempSfx = sfxVol;

        // Apply to mixer
        SetMusicVolume(tempMusic);
        SetSFXVolume(tempSfx);

        // Add listeners to update live when dragging
        musicSlider.onValueChanged.AddListener(OnMusicSlider);
        sfxSlider.onValueChanged.AddListener(OnSFXSlider);
    }

    public void OnMusicSlider(float value)
    {
        tempMusic = value;
        SetMusicVolume(tempMusic); // update live, but don’t save yet
    }

    public void OnSFXSlider(float value)
    {
        tempSfx = value;
        SetSFXVolume(tempSfx); // update live, but don’t save yet
    }

    private void SetMusicVolume(float value)
    {
        float v = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("MusicVol", Mathf.Log10(v) * 20);
    }

    private void SetSFXVolume(float value)
    {
        float v = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("SFXVol", Mathf.Log10(v) * 20);
    }

    public void ApplyVolumes()
    {
        // Save only when Apply button is pressed
        PlayerPrefs.SetFloat("MusicVol", tempMusic);
        PlayerPrefs.SetFloat("SFXVol", tempSfx);
    }

    public void ResetToDefaults()
    {
        float defaultVal = 0.75f;

        musicSlider.SetValueWithoutNotify(defaultVal);
        sfxSlider.SetValueWithoutNotify(defaultVal);

        tempMusic = defaultVal;
        tempSfx = defaultVal;

        SetMusicVolume(tempMusic);
        SetSFXVolume(tempSfx);
    }
}
