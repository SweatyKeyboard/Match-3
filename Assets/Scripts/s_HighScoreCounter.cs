using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class s_HighScoreCounter
{
    public static void SaveScore(int score)
    {
        List<int> scores = new List<int>();
        int scoresCount = PlayerPrefs.GetInt("ScoresCount", 0);
        for (int i = 0; i < scoresCount; i++)
        {
            scores.Add(PlayerPrefs.GetInt($"Scores{i}"));
        }
        scores.Add(score);

        scores.Sort();
        scores.Reverse();

        if (scores.Count > 10)
        {
            scores.RemoveAt(10);
        }

        PlayerPrefs.SetInt("ScoresCount", scores.Count);
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt($"Scores{i}", scores[i]);
        }
    }

    public static string GetScores()
    {
        string result = "";
        int scoresCount = PlayerPrefs.GetInt("ScoresCount", 0);

        if (scoresCount == 0)
        {
            return "There are no records";
        }

        for (int i = 0; i < scoresCount; i++)
        {
            result += (i + 1) + ". " + PlayerPrefs.GetInt($"Scores{i}") + "\n";
        }

        return result;
    }
}
