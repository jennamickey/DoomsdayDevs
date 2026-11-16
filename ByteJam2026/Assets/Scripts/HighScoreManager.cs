using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;

    public int totalPoints;
    public int zombiesKilled;
    public int currentRound;

    public int highScore;
    public int highestRound;
    public int mostKills;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highestRound = PlayerPrefs.GetInt("HighestRound", 0);
        mostKills = PlayerPrefs.GetInt("MostKills", 0);
    }

    public void AddPoints(int points)
    {
        totalPoints += points;
    }

    public void ZombieKilled()
    {
        zombiesKilled++;
    }

    public void UpdateRound(int round)
    {
        currentRound = round;
    }

    public void SaveHighScore()
    {
        if (totalPoints > highScore)
        {
            highScore = totalPoints;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        if (currentRound > highestRound)
        {
            highestRound = currentRound;
            PlayerPrefs.SetInt("HighestRound", highestRound);
        }

        if (zombiesKilled > mostKills)
        {
            mostKills = zombiesKilled;
            PlayerPrefs.SetInt("MostKills", mostKills);
        }

        PlayerPrefs.Save();
    }

    public void ResetCurrentGame()
    {
        totalPoints = 0;
        zombiesKilled = 0;
        currentRound = 0;
    }
}
