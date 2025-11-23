using UnityEngine;

public class AI_FinalBoss : BaseAI
{
    private int[] playerMoveHistory = new int[3];
    private int roundsPassed = 0;

    private const float BOSS_WIN_CHANCE = 0.9f;

    public AI_FinalBoss()
    {
        // บอสไม่จำเป็นต้องใช้ difficultyRate เพราะมันเก่งอยู่แล้ว
    }

    public override void RecordPlayerMove(Move playerMove)
    {
        playerMoveHistory[(int)playerMove]++;
        roundsPassed++;
    }

    public override Move GetAIMove()
    {
        //หาการเคลื่อนไหวที่ผู้เล่นใช้บ่อยที่สุด
        int predictedMoveIndex = 0;
        for (int i = 1; i < playerMoveHistory.Length; i++)
        {
            if (playerMoveHistory[i] > playerMoveHistory[predictedMoveIndex])
            {
                predictedMoveIndex = i;
            }
        }
        Move winningMove = GetWinningMove((Move)predictedMoveIndex);

        //ตัดสินใจ
        if (Random.value < BOSS_WIN_CHANCE) // โอกาส 90% ที่จะชนะ
        {
            return winningMove;
        }
        else
        {
            // โอกาส 10% ที่จะสุ่มพลาด
            return (Move)Random.Range(0, 3);
        }
    }

    public override bool IsOverclocked()
    {
        // บอสอยู่ในโหมด Overclock เสมอ
        return true;
    }

    public override void Reset()
    {
        playerMoveHistory = new int[3];
        roundsPassed = 0;
    }
}
