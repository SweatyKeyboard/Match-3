using UnityEngine;

public class IceCurse : ICurse
{
    public void Initialize(TileView[,] board)
    {
        int size = board.GetLength(0);


        for (int c = 0; c < Random.Range(2, 5); c++)
        {
            int randX = Random.Range(0, size);
            int randY = Random.Range(0, size);

            if (!board[randX, randY].Curses.IsIced)
            {
                board[randX, randY].Curses.Add(this);
            }
        }
    }
}
