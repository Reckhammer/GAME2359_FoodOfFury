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

    float currentVolume;

    Resolution[] resolutions;

    void Start()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);

    }

    //Controls the audio for MainMixer
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    }

    
    public void SetQuatity(int qualityIndex)
    {
        if(qualityIndex != 6) // if the user is not using any of the presets
        //{
            QualitySettings.SetQualityLevel(qualityIndex);
        //}
        switch(qualityIndex)
        {
            case 0: // quality level - very low
                textureDropdown.value = 3;
                break;
            case 1: // quality level - low
                textureDropdown.value = 2;
                break;
            case 2: // quality level - medium
                textureDropdown.value = 1;
                break;
            case 3: // quality level - high
                textureDropdown.value = 0;
                break;
            case 4: // quality level - very high
                textureDropdown.value = 0;
                break;
            case 5: // quality level - ultra
                textureDropdown.value = 0;
                break;
        }

        qualityDropdown.value = qualityIndex;
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 6;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //Needs to be tested in a build
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);

        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);

        PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);

        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));

        PlayerPrefs.SetFloat("VolumePreference", currentVolume);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 3;

        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("TextureQualityPreference"))
            textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
        else
            textureDropdown.value = 0;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;

        if (PlayerPrefs.HasKey("VolumePreference"))
            volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        else
            volumeSlider.value =PlayerPrefs.GetFloat("VolumePreference");
    }


}
