using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    public TextMeshProUGUI cloneCountText;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI controlsText;

    [Header("Level Complete")]
    public GameObject levelCompletePanel;
    public TextMeshProUGUI levelCompleteText;
    public TextMeshProUGUI splitsUsedText;
    public TextMeshProUGUI nextLevelHintText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateCloneCount(int used, int max)
    {
        if (cloneCountText != null)
            cloneCountText.text = "Clones Left: " + (max - used);
    }

    public void SetLevelName(string name)
    {
        if (levelNameText != null)
            levelNameText.text = name;
    }

    public void ShowLevelComplete(int splitsUsed, int levelScore, int timeBonus, bool isLastLevel)
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);

        if (isLastLevel)
        {
            if (levelCompleteText != null)
                levelCompleteText.text = "YOU BEAT ALL 5 LEVELS!";
            if (nextLevelHintText != null)
                nextLevelHintText.text = "Press SPACE to restart from Level 1 | ESC to replay";
        }
        else
        {
            if (levelCompleteText != null)
                levelCompleteText.text = "LEVEL COMPLETE!";
            if (nextLevelHintText != null)
                nextLevelHintText.text = "Press SPACE for next level | ESC to retry";
        }

        if (splitsUsedText != null)
            splitsUsedText.text = "Splits used: " + splitsUsed + "\n+10 completion  +" + timeBonus + " time bonus\nLevel Score: " + levelScore;
    }

    public void ShowGameComplete()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);
        if (levelCompleteText != null)
            levelCompleteText.text = "YOU BEAT ALL 5 LEVELS!";
        if (splitsUsedText != null)
            splitsUsedText.text = "";
        if (nextLevelHintText != null)
            nextLevelHintText.text = "Press SPACE to restart from Level 1";
    }

    public void HideLevelComplete()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }
}
