using UnityEngine;

public abstract class BaseAI
{
    public abstract void RecordPlayerMove(Move playerMove);
    public abstract Move GetAIMove();
    public abstract bool IsOverclocked();
    public abstract void Reset();

    protected Move GetWinningMove(Move predictedMove)
    {
        return (Move)(((int)predictedMove + 1) % 3);
    }
}