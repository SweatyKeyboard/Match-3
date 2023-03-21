using UnityEngine;

public class DestroyDiagonal2 : a_DestroyingBonus
{
    public DestroyDiagonal2()
    {
        _isCheckingAll = true;
        _directions = new Vector2[]
        {
            Vector2.left + Vector2.down,
            Vector2.right + Vector2.up
        };
    }
}
