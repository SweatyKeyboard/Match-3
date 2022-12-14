using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class ScoreCounter : MonoBehaviour
{
    private static int _score;
    private TMP_Text _text;

    public event System.Action OnScoreChanged;
    public static ScoreCounter Instance { get; private set; }
    public int Score 
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreChanged?.Invoke();
            UpdateText();
        }
    }

    private void Awake()
    {
        _score = 0;
        Instance = this;
        _text = GetComponent<TMP_Text>();
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"Score: {_score}";
    }
 
}
