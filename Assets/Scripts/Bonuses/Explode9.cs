using System.Collections.Generic;
using UnityEngine;

public class Explode9 : a_DestroyingBonus
{
    public Explode9()
    {
        _isCheckingAll = false;
        _directions = new Vector2[]
            {
                Vector2.up,
                Vector2.down,
                Vector2.left,
                Vector2.right,
                Vector2.up + Vector2.right,
                Vector2.up + Vector2.left,
                Vector2.down + Vector2.right,
                Vector2.down + Vector2.left,
            };

    }
}
