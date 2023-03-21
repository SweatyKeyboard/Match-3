using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    public void EndThisGame()
    {
        int score = ScoreCounter.Instance.Score;
        _scoreText.text = "Score: " + score;
        s_HighScoreCounter.SaveScore(score);
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
