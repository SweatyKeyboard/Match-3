using UnityEngine;

public class DestroyCrossDiagonal : a_DestroyingBonus
{
    public DestroyCrossDiagonal()
    {
        _isCheckingAll = true;
        _directions = new Vector2[]
        {
            Vector2.up + Vector2.right,
            Vector2.down + Vector2.left,
            Vector2.left + Vector2.up,
            Vector2.right + Vector2.down
        };
    }
}
