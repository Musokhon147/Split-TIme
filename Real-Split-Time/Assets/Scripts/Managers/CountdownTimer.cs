using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining = 45f;

    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            SceneManager.LoadScene("Game");
        }

        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = "Starts in " + seconds + " second" + (seconds != 1 ? "s" : "");
    }
}
