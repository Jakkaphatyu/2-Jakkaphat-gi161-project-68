using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string mainGameSceneName = "MainGameScene"; 
    public string creditsSceneName = "CreditsScene";   
    public string rulesSceneName = "RulesScene";       
    public string mainMenuSceneName = "MainMenuScene";

    void Start()
    {
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        Debug.Log("START: Loading Rules and Synopsis...");
        SceneManager.LoadScene(rulesSceneName);
    }

    public void LoadCredits()
    {
        Debug.Log("Loading Credits...");
        SceneManager.LoadScene(creditsSceneName);
    }

    public void LoadMenu()
    {
        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void LoadMainGame()
    {
        Debug.Log("Launching Main Gameplay: " + mainGameSceneName);
        SceneManager.LoadScene(mainGameSceneName);
    }
}