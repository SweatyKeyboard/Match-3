public class TimeForTileType : IBonus
{
    public void Execute(TileView tileView, TileView[,] field)
    {
        int counter = 0;
        Tile type = tileView.Tile;
        foreach (TileView tile in field)
        {
            if (tile.Tile == type)
            {
                counter++;
            }
        }

        Timer.Instance.SecondsLeft += counter;
    }
}