using System.Collections.Generic;

public class RandomizingBonus : Bonus
{
    public override void Execute(TileView tileView, TileView[,] field)
    {
        List<Tile> tiles = Board.Instance.PossibleTiles;
        tiles.Remove(tileView.Tile);
        tileView.RandomizeType(tiles);
    }
}
