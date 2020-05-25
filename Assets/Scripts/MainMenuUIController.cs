using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    public int startGameSceneIndex;
    public GameObject mainMenuCanvas;
    public GameObject optionsCanvas;
    public GameObject creditsCanvas;
    public GameObject buildInfoCanvas;
    public GameObject bindingsCanvas;
    public AudioMixer audioMixer;
    public TextMeshProUGUI volumeText;
    public Slider volumeSlider;
    public Dropdown resolutionDropdown;
    
    private Resolution[] _resolutions;
    
    private void Start()
    {
        volumeText.text = Mathf.RoundToInt(((volumeSlider.value + 80) / 80) * 100).ToString() + "%";
        _resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        var currentResolutionIndex = 0;
        for(int i = 0; i < _resolutions.Length; i++)
        {
            var option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.width && 
                _resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
            
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void DoExitGame()//called by ui
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void DoStartGame() //called by ui
    {
        SceneManager.LoadScene(startGameSceneIndex);
    }

    public void DoOpenCredits()//called by ui
    {
        mainMenuCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void DoOpenOptions()//called by ui
    {
        mainMenuCanvas.SetActive(false);
        bindingsCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }
    
    public void DoOpenBuildInfo()
    {
        mainMenuCanvas.SetActive(false);
        buildInfoCanvas.SetActive(true);
    }

    public void DoOpenBindings()
    {
        optionsCanvas.SetActive(false);
        bindingsCanvas.SetActive(true);
    }

    public void DoOpenMainMenu()//called by ui
    {
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        buildInfoCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
    
    //options setters
    public void SetVolume(float volume)
    {
        volumeText.text = Mathf.RoundToInt(((volume + 80) / 80) * 100).ToString() + "%";
        audioMixer.SetFloat("Master Volume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
