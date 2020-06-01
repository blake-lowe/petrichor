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
}
