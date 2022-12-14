using System.Collections.Generic;
using UnityEngine;

public class a_MoveToBonus : IBonus
{
    protected Vector2 _direction;
    public void Execute(TileView tileView, TileView[,] field)
    {
        List<TileView> movingTiles = new List<TileView>();

        RaycastHit2D[] hits1 = Physics2D.RaycastAll(tileView.transform.position, _direction);
        RaycastHit2D[] hits2 = Physics2D.RaycastAll(tileView.transform.position, -_direction);
        
        foreach (RaycastHit2D hit in hits1)
        {
            TileView tempTile;
            if (hit.collider != null &&
                hit.collider.TryGetComponent(out tempTile))
            {
                movingTiles.Add(tempTile);
            }
        }

        for (int i = hits2.Length - 1; i > 0; i--)
        {
            TileView tempTile;
            if (hits2[i].collider != null &&
                hits2[i].collider.TryGetComponent(out tempTile))
            {
                movingTiles.Add(tempTile);
            }
        }


        for (int i = movingTiles.Count - 2; i >= 0; i--)
        {
            Board.Instance.Swap(movingTiles[i], movingTiles[i + 1], false);
        }

    }
}
