using UnityEngine;

[CreateAssetMenu(menuName = "Match-3/Tile")]
public class Tile : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [SerializeField] private Color _color;
    private int _mana;

    public event System.Action OnManaUpdated;

    public Sprite Sprite => _sprite;
    public string Name => _name;
    public Color Color => _color;
    public int Mana
    {
        get => _mana;
        set
        {
            if (value >= 0 && value <= 10)
            {
                _mana = value;
            }
            else if (value > 10)
            {
                _mana = 10;
            }
            else if (_mana < 0)
            {
                _mana = 0;
            }

            OnManaUpdated?.Invoke();
        }
    }
}