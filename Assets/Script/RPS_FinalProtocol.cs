using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;


public class RPS_FinalProtocol : MonoBehaviour
{
    public int finalBossWinsToEscape = 3;
    public float decisionTimeLimit = 4f;

    public TMPro.TextMeshProUGUI statusText;
    public TMPro.TextMeshProUGUI scoreText; 
    public TMPro.TextMeshProUGUI livesText;
    public TMPro.TextMeshProUGUI timerText;


    public Sprite[] moveSprites; 
    public AudioClip selectionSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip gameOverSound;
    public AudioClip backgroundMusic;
    public AudioClip finalVictorySound;

    private int finalBossScore = 0;
    private int currentLives;
    private float timer;
    private BaseAI currentAI;
    private bool isGameActive = true;
    private AudioSource audioSource;
    private int maxLives = 3;

    // --------------------------------------------------------------------

    void Start()
    {
        currentAI = new AI_FinalBoss();
        InitializeAudio();
        InitializeGame();
    }

    void InitializeAudio()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource Component not found on GameManager!");
        }

        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void InitializeGame()
    {
        currentLives = maxLives;
        finalBossScore = 0;
        isGameActive = true;
        currentAI.Reset();
        Time.timeScale = 1;
        UpdateUI();
        StartNewRound();
    }

    void Update()
    {
        if (!isGameActive) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timerText != null) timerText.text = "Time: " + Mathf.Ceil(timer).ToString("0");

            if (timer <= 0)
            {
                Move aiMove = currentAI.GetAIMove();
                timer = 0;


                PlayerLostRound(aiMove, true);
            }
        }
    }

    void StartNewRound()
    {
        if (!isGameActive) return;
        timer = decisionTimeLimit;

        if (statusText != null) statusText.text = "Final Protocol: ORACLE-X is awaiting your move!";

    }

    public void OnRockChosen() { PlayerChoseMove(Move.Rock); }
    public void OnPaperChosen() { PlayerChoseMove(Move.Paper); }
    public void OnScissorsChosen() { PlayerChoseMove(Move.Scissors); }

    void PlayerChoseMove(Move playerMove)
    {
        if (!isGameActive || timer <= 0) return;

        if (audioSource != null && selectionSound != null) audioSource.PlayOneShot(selectionSound);

        timer = 0;

        Move aiMove = currentAI.GetAIMove();
        currentAI.RecordPlayerMove(playerMove);


        DetermineWinner(playerMove, aiMove);

        if (isGameActive)
        {
            Invoke("StartNewRound", 2f);
        }
    }

    void DetermineWinner(Move pMove, Move aMove)
    {
        UpdateStatus(pMove, aMove);

        if (pMove == aMove)
        {
            if (statusText != null) statusText.text += "\n\n STANDOFF! ORACLE-X is re-processing...";
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
        finalBossScore++;
        if (audioSource != null && winSound != null) audioSource.PlayOneShot(winSound);

        if (statusText != null) statusText.text += "\n\n VICTORY! Boss Score +1.";

        if (finalBossScore >= finalBossWinsToEscape)
        {
            if (statusText != null) statusText.text = "\n\nESCAPED!PROTOCOL OVERRIDE SUCCESSFUL! YOU ARE FREE.";
            isGameActive = false;
            Time.timeScale = 0;

            if (audioSource != null)
            {
                audioSource.Stop();
            }

            if (audioSource != null && finalVictorySound != null)
            {
                audioSource.PlayOneShot(finalVictorySound);
            }
            else if (audioSource != null && winSound != null)
            {
                audioSource.PlayOneShot(winSound);
            }
        }
    }

    void PlayerLostRound(Move aMove, bool timeOut)
    {
        currentLives--;

        if (audioSource != null && loseSound != null) audioSource.PlayOneShot(loseSound);

        if (timeOut)
        {
            if (statusText != null) statusText.text = "TIME OUT! Life -1. ORACLE-X chose: " + aMove.ToString();
        }
        else
        {
            if (statusText != null) statusText.text += "\n\nDEFEAT! Life -1. ORACLE-X chose: " + aMove.ToString();
        }

        if (currentLives <= 0)
        {
            if (statusText != null) statusText.text = "**SYSTEM FAILURE!** ORACLE-X has terminated your connection.";
            isGameActive = false;
            Time.timeScale = 0;
            if (audioSource != null)
            {
                audioSource.Stop();
            }

            if (audioSource != null && gameOverSound != null)
            {
                audioSource.PlayOneShot(gameOverSound);
            }

        }
        else if (timeOut)
        {
            Invoke("StartNewRound", 2f);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Boss Wins: " + finalBossScore.ToString() + " / " + finalBossWinsToEscape.ToString();
        if (livesText != null) livesText.text = "Lives: " + currentLives.ToString() + " / " + maxLives.ToString();
    }

    void UpdateStatus(Move pMove, Move aMove)
    {
        if (statusText == null) return;

        string result = "YOU chose: " + pMove.ToString() + "\n";
        result += "\n\nORACLE-X chose: " + aMove.ToString() + "";

        result += "\n\n=================================";
        result += "\n\n AI STATUS: FINAL PROTOCOL MODE ";
        result += "\n\n=================================";

        statusText.text = result;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainGameScene");
    }
}