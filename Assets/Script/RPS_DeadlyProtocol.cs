using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class RPS_DeadlyProtocol : MonoBehaviour
{
    public int scoreToWin = 5;
    public int maxLives = 3;
    public float decisionTimeLimit = 5f;
    public float aiDifficultyRate = 0.1f;

    private int playerScore = 0;
    private int currentLives;
    private float timer;
    private BaseAI currentAI;
    private bool isGameActive = true;

    public TMPro.TextMeshProUGUI statusText;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI livesText;
    public TMPro.TextMeshProUGUI timerText;

    public int PlayerScore => playerScore;
    public int CurrentLives => currentLives;
    public bool IsGameActive => isGameActive;

    public AudioClip selectionSound;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;

    // --------------------------------------------------------------------

    void Start()
    {
        currentAI = new AI_OracleX(aiDifficultyRate);
        InitializeGame();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource Component not found on GameManager!");
        }

        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;

            audioSource.Play();
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString("0");

            if (timer <= 0)
            {
                Move aiMove = currentAI.GetAIMove();
                timer = 0;
                PlayerLostRound(aiMove, true);
            }
        }
    }

    public void InitializeGame()
    {
        currentLives = maxLives;
        playerScore = 0;
        isGameActive = true;

        currentAI.Reset();

        Time.timeScale = 1;
        UpdateUI();
        StartNewRound();
    }

    void StartNewRound()
    {
        if (!isGameActive) return;
        timer = decisionTimeLimit;
        statusText.text = "Choose your power: Rock, Paper, Scissors?";
    }

    public void OnRockChosen() { PlayerChoseMove(Move.Rock); }
    public void OnPaperChosen() { PlayerChoseMove(Move.Paper); }
    public void OnScissorsChosen() { PlayerChoseMove(Move.Scissors); }

    void PlayerChoseMove(Move playerMove)
    {
        if (!isGameActive || timer <= 0) return;

        timer = 0;

        Move aiMove = currentAI.GetAIMove(); 
        currentAI.RecordPlayerMove(playerMove); 

        DetermineWinner(playerMove, aiMove);

        if (isGameActive)
        {
            Invoke("StartNewRound", 2f);
        }

        if (audioSource != null && selectionSound != null)
        {
            audioSource.PlayOneShot(selectionSound);
        }


    }

    void DetermineWinner(Move pMove, Move aMove)
    {
        UpdateStatus(pMove, aMove);

        if (pMove == aMove)
        {
            statusText.text += "\n\nIt's a Tie! ORACLE-X is re-processing";
        }
        else if ((pMove == Move.Rock && aMove == Move.Scissors) ||
                  (pMove == Move.Paper && aMove == Move.Rock) ||
                  (pMove == Move.Scissors && aMove == Move.Paper))
        {
            PlayerWonRound();
        }
        else
        {
            PlayerLostRound(aMove, false);
        }

        UpdateUI();
    }

    void PlayerWonRound()
    {
        playerScore++;
        statusText.text += "\n\nYOU WIN!";

        if (playerScore >= scoreToWin)
        {
            statusText.text = "FINAL PROTOCOL UNLOCKED! Prepare to face ORACLE-X!";
            isGameActive = false;

            Invoke("LoadFinalScene", 3f);
        }
    }

    void PlayerLostRound(Move aMove, bool timeOut)
    {
        currentLives--;

        if (timeOut)
        {
            statusText.text = "TIME OUT! AI chose " + aMove.ToString() + ". Life -1.";
        }
        else
        {
            statusText.text += "\n\nYOU LOSE! AI Chose: " + aMove.ToString() + ". Life -1.";
        }

        if (currentLives <= 0)
        {
            statusText.text = "\n\nGAME OVER! ORACLE-X has defeated you.";
            isGameActive = false;
            Time.timeScale = 0;
        }
    }

    void LoadFinalScene()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("FinalProtocolScene"); 
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + playerScore.ToString() + " / " + scoreToWin.ToString();
        livesText.text = "Lives: " + currentLives.ToString() + " / " + maxLives.ToString();
    }

    void UpdateStatus(Move pMove, Move aMove)
    {
        string result = "You chose:" + pMove.ToString() + "\n\nOracleX chose:" + aMove.ToString() + "";

        if (currentAI.IsOverclocked())
        {
            result += "\n\nOracleX STATUS: OVERCLOCK MODE ACTIVATED!  ";
        }
        statusText.text = result;
    }
}