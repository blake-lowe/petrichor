using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{

    public GameObject pausePanel;
    public Animator sceneTransitionAnimator;
    
    private Controls _controls;
    private bool _isPaused = false;

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
            UnpauseGame();
        }
        else
        {
            _isPaused = true;
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
