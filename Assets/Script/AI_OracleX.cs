using UnityEngine;

public class AI_OracleX : BaseAI
{
    private int[] playerMoveHistory = new int[3];
    private int roundsPassed = 0;

    private const float BASE_WIN_CHANCE = 0.3f;
    private const float MAX_WIN_CHANCE = 0.7f;
    private float difficultyIncreaseRate;

    public AI_OracleX(float difficultyRate)
    {
        this.difficultyIncreaseRate = difficultyRate;
    }

    public override void RecordPlayerMove(Move playerMove)
    {
        playerMoveHistory[(int)playerMove]++;
        roundsPassed++;
    }

    public override Move GetAIMove()
    {
        // คาดเดาหาตัวเลือกที่ผู้เล่นเลือกบ่อยที่สุด
        int predictedMoveIndex = 0;
        for (int i = 1; i < playerMoveHistory.Length; i++)
        {
            if (playerMoveHistory[i] > playerMoveHistory[predictedMoveIndex])
            {
                predictedMoveIndex = i;
            }
        }
        // ตัวเลือกที่ชนะทางตัวเลือกที่ผู้เล่นเลือกบ่อยที่สุด
        Move winningMove = GetWinningMove((Move)predictedMoveIndex); // ใช้ Helper จาก BaseAI

        // คำนวณโอกาสที่ AI จะเลือก Winning Move
        float currentWinChance = Mathf.Min(MAX_WIN_CHANCE, BASE_WIN_CHANCE + (roundsPassed * difficultyIncreaseRate));

        if (Random.value < currentWinChance)
        {
            return winningMove;
        }
        else
        {
            return (Move)Random.Range(0, 3);
        }
    }

    public override bool IsOverclocked()
    {
        // เงื่อนไข Overclock: โอกาสชนะทางเกิน 50%
        float currentWinChance = Mathf.Min(MAX_WIN_CHANCE, BASE_WIN_CHANCE + (roundsPassed * difficultyIncreaseRate));
        return roundsPassed >= 3 && currentWinChance > 0.5f;
    }

    public override void Reset()
    {
        playerMoveHistory = new int[3];
        roundsPassed = 0;
    }
}