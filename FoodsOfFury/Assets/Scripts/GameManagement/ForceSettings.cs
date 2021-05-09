using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ForceSettings : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("SettingsIsForced"))
        {
            PlayerPrefs.SetString("SettingsIsForced", "True");
            PlayerPrefs.SetFloat("VolumePreference", 1.0f);
            PlayerPrefs.SetFloat("MusicPreference", 1.0f);
            PlayerPrefs.SetFloat("EffectsPreference", 1.0f);
            PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(true));
        }
        else
        {
            print("settings have already been forced");
            audioMixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("VolumePreference")) * 20);
            audioMixer.SetFloat("mVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicPreference")) * 20);
            audioMixer.SetFloat("eVolume", Mathf.Log10(PlayerPrefs.GetFloat("EffectsPreference")) * 20);
        }

        // unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}