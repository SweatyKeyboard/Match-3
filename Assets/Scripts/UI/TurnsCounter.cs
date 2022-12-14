using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TurnsCounter : MonoBehaviour
{
    private static int _turns;
    private TMP_Text _text;

    public static TurnsCounter Instance { get; private set; }
    public int Turns
    {
        get => _turns;
        set
        {
            _turns = value;
            UpdateText();
            OnTurnsChanged?.Invoke();
        }
    }

    public static event System.Action OnTurnsChanged;

    private void Awake()
    {
        _turns = 0;
        Instance = this;
        _text = GetComponent<TMP_Text>();
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"Turns: {_turns}";
    }

}