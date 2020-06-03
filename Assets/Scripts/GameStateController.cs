using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{
    public GameObject hud;
    public GameObject pausePanel;
    public GameObject swapPanel;
    public Canvas uiCanvas;
    public Animator sceneTransitionAnimator;
    public Animator glitchEffectAnimator;
    public PauseCameraTarget pauseCameraTarget;
    public GameObject gameplayVCAM;
    public GameObject pauseVCAM;
    public PlayerController playerController;

    public int mainMenuSceneIndex;
    
    private Controls _controls;
    private bool _isPaused = false;
    private int _levelToLoadIndex;
    private static readonly int DoGlitch = Animator.StringToHash("doGlitch");

    private void OnEnable()
    {
        _controls = new Controls();
        _controls.Player.pause.performed += HandlePause;
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.pause.performed -= HandlePause;//without this an error is caused by reloading scene and pausing
    }

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    public void Respawn()
    {
        
        _isPaused = true;
        Time.timeScale = 0;
        glitchEffectAnimator.SetTrigger(DoGlitch);
    }

    public void ReloadScene()
    {
        UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DisableHud()
    {
        hud.SetActive(false);
    }

    public void EnableHud()
    {
        hud.SetActive(true);
    }

    public void ScreenSpaceCameraUI()
    {
        uiCanvas.renderMode = RenderMode.ScreenSpaceCamera;
    }

    public void ScreenSpaceOverlayUI()
    {
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    private void HandlePause(InputAction.CallbackContext context)
    {
        if (_isPaused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        pauseCameraTarget.isPaused = true;
        //switch live camera
        pauseVCAM.SetActive(true);
        gameplayVCAM.SetActive(false);
        playerController.isPaused = true;
        playerController.CancelUtility();
    }

    public void UnpauseGame()
    {
        _isPaused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        swapPanel.SetActive(false);
        pauseCameraTarget.isPaused = false;
        //switch live camera
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
