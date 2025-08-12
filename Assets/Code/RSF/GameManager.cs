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

        // 통계 시스템 연동
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

        // RSF 체크
        if (handRank == HandRankType.RoyalStraightFlush || PayoutManager.Instance.CheckRSFWin())
        {
            OnGameWon?.Invoke();
            EndGame(true);
            return;
        }

        // 콤보 업데이트
        playerData.UpdateCombo(handRank);

        // 배당금 계산
        int payout = PayoutManager.Instance.CalculatePayout(handRank, playerData.currentBet);
        bool isWin = payout > 0;

        if (isWin)
        {
            playerData.AddMoney(payout);
        }

        // 통계 업데이트
        if (GameStatistics.Instance != null)
        {
            GameStatistics.Instance.OnRoundComplete(handRank, payout, isWin);
        }

        // 업적 체크
        if (AchievementSystem.Instance != null)
        {
            AchievementSystem.Instance.CheckAchievements();
        }

        // 애니메이션 (승리 시)
        if (isWin && CardAnimationManager.Instance != null)
        {
            // 승리 카드 애니메이션 실행
            Debug.Log("Playing victory animation for: " + handRank);
        }

        OnHandRankResult?.Invoke(handRank);
        OnPayoutCalculated?.Invoke(payout);

        isRoundInProgress = false;

        // 게임 종료 조건 체크
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

        // 통계 업데이트
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