using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatisticsUIManager : MonoBehaviour
{
    [Header("Statistics Panel")]
    public GameObject statisticsPanel;
    public Button closeStatsButton;

    [Header("General Stats")]
    public TextMeshProUGUI totalGamesText;
    public TextMeshProUGUI totalRoundsText;
    public TextMeshProUGUI winRateText;
    public TextMeshProUGUI bestWinStreakText;

    private void Start()
    {
        if (closeStatsButton != null)
            closeStatsButton.onClick.AddListener(CloseStatistics);

        if (statisticsPanel != null)
            statisticsPanel.SetActive(false);
    }

    public void OpenStatistics()
    {
        UpdateStatisticsDisplay();
        if (statisticsPanel != null)
            statisticsPanel.SetActive(true);
    }

    public void CloseStatistics()
    {
        if (statisticsPanel != null)
            statisticsPanel.SetActive(false);
    }

    private void UpdateStatisticsDisplay()
    {
        if (GameStatistics.Instance == null) return;

        GameStatistics stats = GameStatistics.Instance;

        if (totalGamesText != null)
            totalGamesText.text = "Total Games: " + stats.totalGamesPlayed;

        if (totalRoundsText != null)
            totalRoundsText.text = "Total Rounds: " + stats.totalRoundsPlayed;

        if (winRateText != null)
            winRateText.text = "Win Rate: " + stats.GetWinRate().ToString("F1") + "%";

        if (bestWinStreakText != null)
            bestWinStreakText.text = "Best Win Streak: " + stats.bestWinStreak;
    }
}