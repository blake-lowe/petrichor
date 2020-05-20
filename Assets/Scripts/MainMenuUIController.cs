using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    public string startGameSceneName;
    public GameObject MainMenuCanvas;
    public GameObject CreditsCanvas;
    public void doExitGame()//called by ui
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void doStartGame() //called by ui
    {
        Debug.Log("Starting Game");
        SceneManager.LoadScene(startGameSceneName);
    }

    public void doOpenCredits()
    {
        MainMenuCanvas.SetActive(false);
        CreditsCanvas.SetActive(true);
    }

    public void doOpenMainMenu()
    {
        CreditsCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }
}
