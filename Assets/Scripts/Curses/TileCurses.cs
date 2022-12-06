public class TileCurses : System.ICloneable
{
    public event System.Action OnChanged;

    public bool IsIced { get; private set; }
    public void Add(ICurse curse)
    {
        if (curse is IceCurse)
        {
            IsIced = true;
        }

        OnChanged?.Invoke();
    }

    public void ClearCurses()
    {
        IsIced = false;
    }

    public object Clone()
    {
        TileCurses curses = new TileCurses();
        curses.IsIced = IsIced;
        return curses;
    }
}
