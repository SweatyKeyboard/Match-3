public class TileEffects
{
    public event System.Action OnChanged;

    public bool IsTriplingTime { get; private set; }
    public void Add(ITileEffect effect)
    {
        if (effect is TripleTimeEffect)
        {
            IsTriplingTime = true;
        }

        OnChanged?.Invoke();
    }

    public void Clear()
    {
        IsTriplingTime = false;
    }
}
