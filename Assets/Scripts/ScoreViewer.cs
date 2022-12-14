using UnityEngine;
using TMPro;

public class ScoreViewer : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _text.text = s_HighScoreCounter.GetScores();
    }
}
