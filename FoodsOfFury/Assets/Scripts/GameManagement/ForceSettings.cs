using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSettings : MonoBehaviour
{
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
        }
    }
}