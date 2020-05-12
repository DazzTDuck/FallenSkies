using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject settingsMenu;

    public AudioMixer masterMixer;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    float animationSpeed;

    private void Start()
    {
        animationSpeed = settingsMenu.GetComponentInChildren<AnimationUI>().AnimationSpeed;

        GetResolutions();
    }

    public void GetResolutions()
    {
        //get and set the resolutions for the dropdown
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    //control the different volume sliders
    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterValue", Mathf.Log10(volume) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicValue", Mathf.Log10(volume) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFXValue", Mathf.Log10(volume) * 20);
    }

    //set the quality of the game
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //set the fullscreen of the game
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }  

    public void SettingsToMenuTrigger()
    {
        StartCoroutine("SettingsToMenu");
    }

    public void MenuToSettingsTrigger()
    {
        StartCoroutine("MenuToSettings");
    }

    public IEnumerator SettingsToMenu()
    {
        foreach (var uiChild in settingsMenu.GetComponentsInChildren<AnimationUI>())
        {
            uiChild.OnCloseAnimation();
        }

        yield return new WaitForSeconds(animationSpeed);

        if (settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
        }

        foreach (var uiChild in startMenu.GetComponentsInChildren<AnimationUI>())
        {
            uiChild.OnButtonPressPlayAnimation();
        }

    }
    public IEnumerator MenuToSettings()
    {

        if (!settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(true);
        }

        foreach (var uiChild in startMenu.GetComponentsInChildren<AnimationUI>())
        {
            uiChild.OnCloseAnimation();
        }

        yield return new WaitForSeconds(animationSpeed);

        foreach (var uiChild in settingsMenu.GetComponentsInChildren<AnimationUI>())
        {
            uiChild.OnButtonPressPlayAnimation();
        }     
    }
}
