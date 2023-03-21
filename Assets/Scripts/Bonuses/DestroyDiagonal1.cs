using UnityEngine;

public class DestroyDiagonal1 : a_DestroyingBonus
{
    public DestroyDiagonal1()
    {
        _isCheckingAll = true;
        _directions = new Vector2[]
        {
            Vector2.left + Vector2.up,
            Vector2.right + Vector2.down
        };
    }
}
