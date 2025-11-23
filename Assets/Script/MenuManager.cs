using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuManager : MonoBehaviour
{
    public string mainGameSceneName = "MainGameScene";
    public string creditsSceneName = "CreditsScene";
    public string rulesSceneName = "RulesScene";
    public string mainMenuSceneName = "MainMenuScene";

    public AudioSource audioSource;
    public AudioClip menuBGM;

    void Start()
    {
        Time.timeScale = 1;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null && menuBGM != null && !audioSource.isPlaying)
        {
            audioSource.clip = menuBGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StartGame()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        Debug.Log("START: Loading Rules and Synopsis...");
        SceneManager.LoadScene(rulesSceneName);
    }

    public void LoadCredits()
    {
        Debug.Log("Loading Credits...");
        SceneManager.LoadScene(creditsSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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

    public void ToggleMute()
    {

        if (AudioListener.volume > 0f)
        {
            AudioListener.volume = 0f;
            Debug.Log("Game Muted.");
        }
        else
        {
            AudioListener.volume = 1f;
            Debug.Log("Game Unmuted.");
        }
    }
}