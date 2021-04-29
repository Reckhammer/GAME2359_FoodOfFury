using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    public Dropdown qualityDropdown;

    public Dropdown textureDropdown;

    public Slider volumeSlider;

    public Slider musicSlider;

    public Slider effectsSlider;

    float currentVolume;
    float currentMusic;
    float currentEffects;

    Resolution[] resolutions;

    void Start()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " : " + resolutions[i].refreshRate + " hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && Screen.currentResolution.refreshRate == resolutions[i].refreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    //Controls the audio for Master volume
    public void SetVolume(float volume)
    {
        print("setting volume: " + volume);
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        currentVolume = volume;
        PlayerPrefs.SetFloat("VolumePreference", currentVolume);
    }

    public void SetMusic(float musicV)
    {
        print("setting volume: " + musicV);
        audioMixer.SetFloat("mVolume", Mathf.Log10(musicV) * 20);
        currentMusic = musicV;
        PlayerPrefs.SetFloat("MusicPreference", currentMusic);
    }

    public void SetEffects(float effectsV)
    {
        print("setting volume: " + effectsV);
        audioMixer.SetFloat("eVolume", Mathf.Log10(effectsV) * 20);
        currentEffects = effectsV;
        PlayerPrefs.SetFloat("EffectsPreference", currentEffects);
    }

    public void SetQuatity(int qualityIndex)
    {
        print("setting overall quality: " + qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex, true);
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
    }

    public void SetTextureQuality(int textureIndex)
    {
        print("setting texture quality: " + textureIndex);
        QualitySettings.masterTextureLimit = textureIndex;
        PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);
    }

    public void SetResolution(int resolutionIndex)
    {
        print("setting resolution: " + resolutionIndex);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Application.targetFrameRate = resolution.refreshRate;
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
    }

    //Needs to be tested in a build
    public void SetFullscreen(bool isFullscreen)
    {
        print("setting fullscreen: " + isFullscreen);
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));
    }

    //public void SaveSettings()
    //{
    //    AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

    //    PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);

    //    PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);

    //    PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);

    //    PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));

    //    PlayerPrefs.SetFloat("VolumePreference", currentVolume);

    //    PlayerPrefs.SetFloat("MusicPreference", currentMusic);

    //    PlayerPrefs.SetFloat("EffectsPreference", currentEffects);
    //}

    public void LoadSettings(int currentResolutionIndex)
    {
        print("loading settings: " + currentResolutionIndex);

        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 0;

        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("TextureQualityPreference"))
            textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
        else
            textureDropdown.value = 5;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
        {
            print("Fullscren preference: " + Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference")));
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        }

        if (PlayerPrefs.HasKey("VolumePreference"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        }
        else
        {
            volumeSlider.value = 1.0f;
            audioMixer.SetFloat("Volume", volumeSlider.value);
        }

        if (PlayerPrefs.HasKey("MusicPreference"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicPreference");
        }
        else
        {
            musicSlider.value = 1.0f;
            audioMixer.SetFloat("mVolume", musicSlider.value);
        }

        if (PlayerPrefs.HasKey("EffectsPreference"))
        {
            effectsSlider.value = PlayerPrefs.GetFloat("EffectsPreference");
        }
        else
        {
            effectsSlider.value = 1.0f;
            audioMixer.SetFloat("eVolume", effectsSlider.value);
        }

    }
}
