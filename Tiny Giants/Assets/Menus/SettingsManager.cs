using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Text volumeValueText;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private void Start()
    {
        RefreshResolutions();
        LoadSettings();
    }

    public void SetVolume(float volume)
    {
        float volumeValue = Mathf.Log(volume) * 20;
        audioMixer.SetFloat("Volume", volumeValue);
        PlayerPrefs.SetFloat("volume", volumeValue);
        volumeValueText.text = $"{Mathf.FloorToInt(volume * 100)} %";
    }

    public void LoadSettings()
    {
        // Loads Volume Data
        float volumeValue = PlayerPrefs.GetFloat("volume", Mathf.Log(1) * 20);
        audioMixer.SetFloat("Volume", volumeValue);
        volumeValueText.text = $"{Mathf.FloorToInt(1 * 100)} %";

        // Loads Quality Data
        int qualityIndex = PlayerPrefs.GetInt("quality", 2);
        QualitySettings.SetQualityLevel(qualityIndex);

        // Loads Fullscreen Data
        int isFullscreenValue = PlayerPrefs.GetInt("fullscreen", 1);
        bool isFullscreen = isFullscreenValue == 1;
        Screen.fullScreen = isFullscreen;

        // Loads Fullscreen Data
        int resolutionIndex = PlayerPrefs.GetInt("resolutionIndex", resolutions.Length - 1);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
    }

    private void RefreshResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width}x{resolutions[i].height}@{resolutions[i].refreshRate}hz";
            options.Add(option);
            if (resolutions[i].width.Equals(Screen.currentResolution.width) && resolutions[i].height.Equals(Screen.currentResolution.height))
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
}