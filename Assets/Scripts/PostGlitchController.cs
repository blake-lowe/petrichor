using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGlitchController : MonoBehaviour
{
    public GameStateController gameStateController;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayGlitchSound()
    {
        _audioSource.Play();
    }
    public void ReloadScene()
    {
        gameStateController.ReloadScene();
    }

    public void ScreenSpaceCameraUI()
    {
        gameStateController.ScreenSpaceCameraUI();
    }

    public void ScreenSpaceOverlayUI()
    {
        gameStateController.ScreenSpaceOverlayUI();
    }
}
