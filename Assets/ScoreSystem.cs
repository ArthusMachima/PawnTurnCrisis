using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    // Method 1: Save score using PlayerPrefs (original)
    public void SaveScore(string playerName, int score)
    {
        int scoreCount = PlayerPrefs.GetInt("ScoreCount", 0);
        PlayerPrefs.SetString($"PlayerName_{scoreCount}", playerName);
        PlayerPrefs.SetInt($"PlayerScore_{scoreCount}", score);
        PlayerPrefs.SetInt("ScoreCount", scoreCount + 1);
        PlayerPrefs.Save();
    }

    // Method 1 Overload: Save score with waves and difficulty
    public void SaveScore(string playerName, int score, string waves, string difficulty)
    {
        int scoreCount = PlayerPrefs.GetInt("ScoreCount", 0);
        PlayerPrefs.SetString($"PlayerName_{scoreCount}", playerName);
        PlayerPrefs.SetInt($"PlayerScore_{scoreCount}", score);
        PlayerPrefs.SetString($"PlayerWaves_{scoreCount}", waves);
        PlayerPrefs.SetString($"PlayerDifficulty_{scoreCount}", difficulty);
        PlayerPrefs.SetInt("ScoreCount", scoreCount + 1);
        PlayerPrefs.Save();
    }

    // Method 2: Display top 10 scores on TextMeshProUGUI (leaderboard style)
    public void DisplayLeaderboard(TextMeshProUGUI leaderboardTextMesh)
    {
        List<PlayerScore> allScores = LoadAllScores();
        allScores = SortScores(allScores);

        string leaderboardText = "";
        int displayCount = Mathf.Min(allScores.Count, 10);

        for (int i = 0; i < displayCount; i++)
        {
            leaderboardText += $"{i + 1}. {allScores[i].playerName}: {allScores[i].score}";

            // Only show waves and difficulty if they exist
            bool hasWaves = !string.IsNullOrEmpty(allScores[i].waves);
            bool hasDifficulty = !string.IsNullOrEmpty(allScores[i].difficulty);

            if (hasWaves || hasDifficulty)
            {
                leaderboardText += " (";
                if (hasWaves)
                {
                    leaderboardText += $"{allScores[i].waves}";
                    if (hasDifficulty) leaderboardText += " - ";
                }
                if (hasDifficulty)
                {
                    leaderboardText += $"{allScores[i].difficulty}";
                }
                leaderboardText += ")";
            }
            leaderboardText += "\n";
        }

        leaderboardTextMesh.text = leaderboardText;
    }

    // Method 3: Sort scores from highest to lowest
    public List<PlayerScore> SortScores(List<PlayerScore> scores)
    {
        return scores.OrderByDescending(score => score.score).ToList();
    }

    // Method 4: Returns a string with only the highest score
    public string GetHighestScoreString()
    {
        List<PlayerScore> allScores = LoadAllScores();

        if (allScores.Count == 0)
        {
            return "No scores yet!";
        }

        PlayerScore highestScore = SortScores(allScores).First();

        string highestScoreText = $"{highestScore.playerName}: {highestScore.score}";

        // Add waves and difficulty if they exist
        bool hasWaves = !string.IsNullOrEmpty(highestScore.waves);
        bool hasDifficulty = !string.IsNullOrEmpty(highestScore.difficulty);

        if (hasWaves || hasDifficulty)
        {
            highestScoreText += " (";
            if (hasWaves)
            {
                highestScoreText += $"{highestScore.waves}";
                if (hasDifficulty) highestScoreText += " - ";
            }
            if (hasDifficulty)
            {
                highestScoreText += $"{highestScore.difficulty}";
            }
            highestScoreText += ")";
        }

        return highestScoreText;
    }

    // Helper method to load all scores (not counted in the 3 methods)
    private List<PlayerScore> LoadAllScores()
    {
        List<PlayerScore> scores = new List<PlayerScore>();
        int scoreCount = PlayerPrefs.GetInt("ScoreCount", 0);

        for (int i = 0; i < scoreCount; i++)
        {
            string name = PlayerPrefs.GetString($"PlayerName_{i}", "");
            int score = PlayerPrefs.GetInt($"PlayerScore_{i}", 0);
            string waves = PlayerPrefs.GetString($"PlayerWaves_{i}", "");
            string difficulty = PlayerPrefs.GetString($"PlayerDifficulty_{i}", "");

            scores.Add(new PlayerScore(name, score, waves, difficulty));
        }

        return scores;
    }
}

// Separate class for score data
[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public int score;
    public string waves;
    public string difficulty;

    // Original constructor
    public PlayerScore(string name, int scoreValue)
    {
        playerName = name;
        score = scoreValue;
        waves = "";
        difficulty = "";
    }

    // New constructor with waves and difficulty
    public PlayerScore(string name, int scoreValue, string wavesCompleted, string difficultyLevel)
    {
        playerName = name;
        score = scoreValue;
        waves = wavesCompleted;
        difficulty = difficultyLevel;
    }

    // Additional constructor with just waves
    public PlayerScore(string name, int scoreValue, string wavesCompleted)
    {
        playerName = name;
        score = scoreValue;
        waves = wavesCompleted;
        difficulty = "";
    }
}