using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    /* Errors out
    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string options = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(options);
        }

        resolutionDropdown.AddOptions(options);

    }
    */

    //Controls the audio for MainMixer
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    //Dose not work
    public void SetQuatity(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Needs to be tested in a build
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


}
