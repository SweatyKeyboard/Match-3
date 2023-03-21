using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTile : IBonus
{
    public void Execute(TileView tileView, TileView[,] field)
    {
        tileView.Destroy();
    }

}
