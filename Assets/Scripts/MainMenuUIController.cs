using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    public int startGameSceneIndex;
    public GameObject MainMenuCanvas;
    public GameObject OptionsCanvas;
    public GameObject CreditsCanvas;
    public void doExitGame()//called by ui
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void doStartGame() //called by ui
    {
        SceneManager.LoadScene(startGameSceneIndex);
    }

    public void doOpenCredits()//called by ui
    {
        MainMenuCanvas.SetActive(false);
        CreditsCanvas.SetActive(true);
    }

    public void doOpenOptions()//called by ui
    {
        MainMenuCanvas.SetActive(false);
        OptionsCanvas.SetActive(true);
    }

    public void doOpenMainMenu()//called by ui
    {
        OptionsCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }
}
