using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int currentScore = 0;
    public TMPro.TextMeshProUGUI scoreText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scoreText == null)
        {
            GameObject scoreGO = GameObject.FindWithTag("ScoreText");
            if (scoreGO != null)
            {
                scoreText = scoreGO.GetComponent<TextMeshProUGUI>();
            }
        }

         //ResetScore();
        UpdateScoreUI();
    }
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    public int GetScore()
    {
        return currentScore;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }
}
