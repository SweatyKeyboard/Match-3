using UnityEngine;

public class TileCurses
{
    public event System.Action OnChanged;
    public bool IsIced { get; private set; }
    public Vector2 MovingDirection { get; private set; }
    public void Add(ICurse curse)
    {
        if (curse is IceCurse)
        {
            IsIced = true;
        }
        if (curse is MovingCurse)
        {
            MovingDirection = ((MovingCurse)curse).Direction;
        }

        OnChanged?.Invoke();
    }

    public void ClearCurses()
    {
        IsIced = false;
        MovingDirection = Vector2.zero;
    }

    public void ReverseMovingDirection()
    {
        MovingDirection = -MovingDirection;
        OnChanged?.Invoke();
    }
}
