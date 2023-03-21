using UnityEngine;

public class TripleTimeMark : IBonus
{
    public void Execute(TileView tileView, TileView[,] field)
    {
        tileView.Effects.Add(new TripleTimeEffect());
    }
}