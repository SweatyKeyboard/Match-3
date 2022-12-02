public class RandomizingBonus : Bonus
{
    public override void Execute(TileView tileView, TileView[] field)
    {
        tileView.RandomizeType();
    }
}
