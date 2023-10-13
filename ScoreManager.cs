using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private int currentScore;

    private void Start()
    {
        LoadScore();
        UpdateScoreUI();
    }

    public void IncreaseScore(int points)
    {
        currentScore += points;
        SetScore(currentScore);
    }

    private void SetScore(int score)
    {
        currentScore = score;
        SaveScore();
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        SetScore(0);
    }
    public void ResetBestScore()
    {
        PlayerPrefs.SetInt("bestScore", 0);
    }

    private void SaveScore()
    {
        int bestScore = GetScore();
        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("bestScore", currentScore);
        }
    }

    private int GetScore()
    {
        return PlayerPrefs.GetInt("bestScore", 0);
    }

    public void UpdateScoreUI()
    {
        currentScoreText.text = currentScore.ToString();
        bestScoreText.text = GetScore().ToString();
    }

    private void LoadScore()
    {
        currentScore = PlayerPrefs.GetInt("currentScore", 0);
    }

    private void OnDestroy()
    {
        SaveScore();
    }
}
