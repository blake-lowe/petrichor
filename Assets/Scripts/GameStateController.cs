﻿using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{

    public GameObject pausePanel;
    public Animator sceneTransitionAnimator;
    public PauseCameraTarget pauseCameraTarget;
    public GameObject gameplayVCAM;
    public GameObject pauseVCAM;
    public PlayerController playerController;

    public int mainMenuSceneIndex;
    
    private Controls _controls;
    private bool _isPaused = false;
    private int _levelToLoadIndex;

    private void Awake()
    {
        _controls = new Controls();

        _controls.Player.pause.performed += HandlePause;
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
    }

    void Start()
    {
        pausePanel.SetActive(false);
    }
    
    void Update()
    {
        
    }

    private void HandlePause(InputAction.CallbackContext context)
    {
        if (_isPaused)
        {
            _isPaused = false;
            pauseCameraTarget.isPaused = false;
            UnpauseGame();
        }
        else
        {
            _isPaused = true;
            pauseCameraTarget.isPaused = true;
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //disable and reenable camera to set switch to it
        pauseVCAM.SetActive(true);
        gameplayVCAM.SetActive(false);
        playerController.isPaused = true;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        //disable and reenable camera to set switch to it
        gameplayVCAM.SetActive(true);
        pauseVCAM.SetActive(false);
        playerController.isPaused = false;
    }

    public void FadeToLevel(int levelIndex)//called by other classes
    {
        sceneTransitionAnimator.SetTrigger("FadeOut");
        _levelToLoadIndex = levelIndex;
    }

    public void OnFadeComplete()//called by animation event
    {
        UnpauseGame();
        SceneManager.LoadScene(_levelToLoadIndex);
    }

    public void DoLoadMainMenu()
    {
        FadeToLevel(mainMenuSceneIndex);
    }
    
    
}
