using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        if (GameManager.Instance != null && scoreText != null)
        {
            Debug.Log("Updating score UI: " + GameManager.Instance.score);
            scoreText.text = GameManager.Instance.score.ToString();
        }
    }
}