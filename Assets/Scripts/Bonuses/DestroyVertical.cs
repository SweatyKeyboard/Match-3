using UnityEngine;

public class DestroyVertical : a_DestroyingBonus
{
    public DestroyVertical()
    {
        _isCheckingAll = true;
        _directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down
        };
    }
}
