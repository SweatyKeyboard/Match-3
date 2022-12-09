using UnityEngine;

public class DestroyCrossOrthogonal : a_DestroyingBonus
{
    public DestroyCrossOrthogonal()
    {
        _isCheckingAll = true;
        _directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };
    }
}
