using UnityEngine;

public class DestroyHorizontal : a_DestroyingBonus
{
    public DestroyHorizontal()
    {
        _isCheckingAll = true;
        _directions = new Vector2[]
        {
            Vector2.left,
            Vector2.right
        };
    }
}
