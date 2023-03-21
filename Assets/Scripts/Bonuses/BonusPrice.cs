using UnityEngine;

[CreateAssetMenu(menuName = "Match-3/Bonus price")]
public class BonusPrice : ScriptableObject
{
    [SerializeField] private int _xValue;
    [SerializeField] private int _yValue;
    [SerializeField] private int _zValue;

    public int[] Prices => new[] { _xValue, _yValue, _zValue };

    public int GetZeroesCount()
    {
        int zeroes = 0;

        zeroes += (_xValue == 0) ? 1 : 0;
        zeroes += (_yValue == 0) ? 1 : 0;
        zeroes += (_zValue == 0) ? 1 : 0;

        return zeroes;
    }
}
