public class DestroyTileType : IBonus
{
    public void Execute(TileView tileView, TileView[,] field)
    {
        Tile type = tileView.Tile;
        foreach (TileView tile in field)
        {
            if (tile.Tile == type)
            {
                tile.Destroy();
            }
        }
    }
}