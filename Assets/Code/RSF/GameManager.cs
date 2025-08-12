using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int minBet = 100;
    public int maxBet = 10000;

    [Header("Game State")]
    public bool isGameActive = false;
    public bool isRoundInProgress = false;

    [Header("Events")]
    public UnityEvent<HandRankType> OnHandRankResult;
    public UnityEvent<int> OnPayoutCalculated;
    public UnityEvent OnGameWon;
    public UnityEvent OnGameLost;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        PlayerData.Instance.currentMoney = 15000;
        isGameActive = true;
        isRoundInProgress = false;

        // ��� �ý��� ����
        if (GameStatistics.Instance != null)
        {
            GameStatistics.Instance.OnGameStart();
        }
    }

    public bool PlaceBet(int betAmount)
    {
        if (!isGameActive || isRoundInProgress) return false;
        if (betAmount < minBet || betAmount > maxBet) return false;
        if (!PlayerData.Instance.CanAfford(betAmount)) return false;

        PlayerData.Instance.SetBet(betAmount);
        PlayerData.Instance.SpendMoney(betAmount);
        isRoundInProgress = true;

        return true;
    }

    public void ProcessHandResult(HandRankType handRank)
    {
        if (!isRoundInProgress) return;

        PlayerData playerData = PlayerData.Instance;

        // RSF üũ
        if (handRank == HandRankType.RoyalStraightFlush || PayoutManager.Instance.CheckRSFWin())
        {
            OnGameWon?.Invoke();
            EndGame(true);
            return;
        }

        // �޺� ������Ʈ
        playerData.UpdateCombo(handRank);

        // ���� ���
        int payout = PayoutManager.Instance.CalculatePayout(handRank, playerData.currentBet);
        bool isWin = payout > 0;

        if (isWin)
        {
            playerData.AddMoney(payout);
        }

        // ��� ������Ʈ
        if (GameStatistics.Instance != null)
        {
            GameStatistics.Instance.OnRoundComplete(handRank, payout, isWin);
        }

        // ���� üũ
        if (AchievementSystem.Instance != null)
        {
            AchievementSystem.Instance.CheckAchievements();
        }

        // �ִϸ��̼� (�¸� ��)
        if (isWin && CardAnimationManager.Instance != null)
        {
            // �¸� ī�� �ִϸ��̼� ����
            Debug.Log("Playing victory animation for: " + handRank);
        }

        OnHandRankResult?.Invoke(handRank);
        OnPayoutCalculated?.Invoke(payout);

        isRoundInProgress = false;

        // ���� ���� ���� üũ
        if (playerData.currentMoney < minBet)
        {
            OnGameLost?.Invoke();
            EndGame(false);
        }
    }

    private void EndGame(bool won)
    {
        isGameActive = false;
        isRoundInProgress = false;

        // ��� ������Ʈ
        if (GameStatistics.Instance != null)
        {
            GameStatistics.Instance.OnGameEnd();
        }

        if (won)
        {
            Debug.Log("Congratulations! You achieved Royal Straight Flush!");
        }
        else
        {
            Debug.Log("Game Over! Not enough money to continue.");
        }
    }

    public void RestartGame()
    {
        StartNewGame();
    }
}