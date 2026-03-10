using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    [Header("Settings")]
    public int completionPoints = 10;

    private float levelTime;
    private float timeRemaining;
    private int totalScore;
    private bool timerRunning;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
    }

    void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerRunning = false;
            // Time ran out — reset the level
            TimeManager.Instance.FullReset();
        }

        UpdateTimerUI();
    }

    public void StartTimer(float time)
    {
        levelTime = time;
        timeRemaining = time;
        timerRunning = true;
        UpdateTimerUI();
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ResetTimer()
    {
        timeRemaining = levelTime;
        timerRunning = true;
        UpdateTimerUI();
    }

    public int CompleteLevel()
    {
        timerRunning = false;
        int timeBonus = Mathf.FloorToInt(timeRemaining);
        int levelScore = completionPoints + timeBonus;
        totalScore += levelScore;
        UpdateScoreUI();
        return levelScore;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public int GetTimeBonus()
    {
        return Mathf.FloorToInt(timeRemaining);
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = "Time Left: " + Mathf.CeilToInt(timeRemaining);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + totalScore;
    }
}
