using UnityEngine;

public class MovingCurse : ICurse
{
    public Vector2 Direction { get; private set; }

    private Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right,
        Vector2.up + Vector2.left,
        Vector2.up + Vector2.right,
        Vector2.down + Vector2.right,
        Vector2.down + Vector2.left
    };
    public void Initialize(TileView[,] board)
    {
        int size = board.GetLength(0);

        int randX = Random.Range(0, size);
        int randY = Random.Range(0, size);
        int randD = Random.Range(0, directions.Length);

        Direction = directions[randD];

        bool isDone = false;
        do
        {
            if (board[randX, randY].Curses.MovingDirection == Vector2.zero)
            {
                board[randX, randY].Curses.Add(this);
                isDone = true;
            }
        } while (!isDone);
    }
}
