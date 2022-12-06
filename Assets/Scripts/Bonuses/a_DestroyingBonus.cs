using System.Collections.Generic;
using UnityEngine;

public abstract class a_DestroyingBonus : IBonus
{
    protected Vector2[] _directions;
    protected bool _isCheckingAll;
    public void Execute(TileView tileView, TileView[,] field)
    {
        List<TileView> tilesToDestroy = new List<TileView>();

        tileView.gameObject.SetActive(false);
        for (int i = 0; i < _directions.Length; i++)
        {
            if (_isCheckingAll)
            {
                RaycastHit2D[] rays = Physics2D.RaycastAll(tileView.transform.position, _directions[i]);
                foreach (RaycastHit2D ray in rays)
                {
                    if (ray.collider != null)
                    {
                        tilesToDestroy.Add(ray.collider.GetComponent<TileView>());
                    }
                }
            }
            else
            {
                RaycastHit2D ray = Physics2D.Raycast(tileView.transform.position, _directions[i]);
                if (ray.collider != null)
                {
                    tilesToDestroy.Add(ray.collider.GetComponent<TileView>());
                }

            }
        }
        tileView.gameObject.SetActive(true);

        tilesToDestroy.Add(tileView);

        foreach (Tile tile in Board.Instance.PossibleTiles)
        {
            int counter = 0;
            foreach (TileView destroyedTile in tilesToDestroy)
            {
                if (destroyedTile.Tile == tile)
                {
                    counter++;
                }
            }

            if (counter >= 3)
            {
                tile.Mana += counter - 2;
                ScoreCounter.Instance.Score += counter - 2;
                Timer.Instance.SecondsLeft += counter - 2;
            }
        }

        foreach (TileView tile in tilesToDestroy)
        {
            tile.Destroy();
        }
    }
}
