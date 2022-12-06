using UnityEngine;

[CreateAssetMenu(menuName = "Match-3/Bonus collection")]
public class BonusCollection : ScriptableObject
{
    [SerializeField] private BonusEffect[] _bonuses;

    public int Count => _bonuses.Length;
    public BonusEffect this[int i] => _bonuses[i];
}
