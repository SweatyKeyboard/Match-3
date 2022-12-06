using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaCounter : MonoBehaviour
{
    [SerializeField] private Image _colorIndicator;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Tile _tileType;

    private void Awake()
    {
        _colorIndicator.color = _tileType.Color;
        _tileType.Mana = 0;
        _tileType.OnManaUpdated += UpdateCounter;
        UpdateCounter();
    }

    private void OnDestroy()
    {
        _tileType.OnManaUpdated -= UpdateCounter;
    }
    public void UpdateCounter()
    {
        _text.text = _tileType.Mana.ToString();
    }
}
