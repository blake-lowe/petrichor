using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGlitchController : MonoBehaviour
{
    public GameStateController gameStateController;

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
