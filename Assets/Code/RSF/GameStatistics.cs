using UnityEngine;
using System;

[Serializable]
public class GameStatistics : MonoBehaviour
{
    [Header("Game Statistics")]
    public int totalGamesPlayed = 0;
    public int totalRoundsPlayed = 0;
    public int totalMoneyWon = 0;
    public int totalMoneyLost = 0;
    public int currentWinStreak = 0;
    public int bestWinStreak = 0;
    public int currentLoseStreak = 0;
    public int worstLoseStreak = 0;

    [Header("Hand Rankings Statistics")]
    public int[] handRankingCounts = new int[11];

    [Header("Special Achievements")]
    public int royalStraightFlushCount = 0;
    public int comboAchievements = 0;
    public int bigWins = 0;
    public float totalPlayTime = 0f;

    [Header("Session Data")]
    public float sessionStartTime;
    public int sessionGamesPlayed = 0;
    public int sessionMoneyChange = 0;

    public static GameStatistics Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            sessionStartTime = Time.time;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadStatistics();
    }

    public void OnGameStart()
    {
        totalGamesPlayed++;
        sessionGamesPlayed++;
        sessionStartTime = Time.time;
        sessionMoneyChange = -PlayerData.Instance.currentMoney;
    }

    public void OnRoundComplete(HandRankType handRank, int payout, bool isWin)
    {
        totalRoundsPlayed++;

        if (handRank != HandRankType.None)
        {
            int index = GetHandRankIndex(handRank);
            if (index >= 0 && index < handRankingCounts.Length)
            {
                handRankingCounts[index]++;
            }
        }

        if (handRank == HandRankType.RoyalStraightFlush)
        {
            royalStraightFlushCount++;
        }

        if (isWin && payout > 0)
        {
            totalMoneyWon += payout;
            currentWinStreak++;
            currentLoseStreak = 0;

            if (currentWinStreak > bestWinStreak)
            {
                bestWinStreak = currentWinStreak;
            }

            if (payout >= 10000)
            {
                bigWins++;
            }
        }
        else
        {
            totalMoneyLost += PlayerData.Instance.currentBet;
            currentLoseStreak++;
            currentWinStreak = 0;

            if (currentLoseStreak > worstLoseStreak)
            {
                worstLoseStreak = currentLoseStreak;
            }
        }

        if (PlayerData.Instance.IsComboActive())
        {
            comboAchievements++;
        }
    }

    public void OnGameEnd()
    {
        float sessionTime = Time.time - sessionStartTime;
        totalPlayTime += sessionTime;

        sessionMoneyChange += PlayerData.Instance.currentMoney;

        SaveStatistics();
    }

    private int GetHandRankIndex(HandRankType handRank)
    {
        switch (handRank)
        {
            case HandRankType.OnePair: return 0;
            case HandRankType.TwoPair: return 1;
            case HandRankType.RedFlush: return 2;
            case HandRankType.BlackFlush: return 3;
            case HandRankType.Triple: return 4;
            case HandRankType.Straight: return 5;
            case HandRankType.Flush: return 6;
            case HandRankType.FullHouse: return 7;
            case HandRankType.FourKind: return 8;
            case HandRankType.StraightFlush: return 9;
            case HandRankType.RoyalStraightFlush: return 10;
            default: return -1;
        }
    }

    public string GetHandRankName(int index)
    {
        HandRankType[] handRanks = {
            HandRankType.OnePair, HandRankType.TwoPair, HandRankType.RedFlush, HandRankType.BlackFlush,
            HandRankType.Triple, HandRankType.Straight, HandRankType.Flush, HandRankType.FullHouse,
            HandRankType.FourKind, HandRankType.StraightFlush, HandRankType.RoyalStraightFlush
        };

        if (index >= 0 && index < handRanks.Length)
        {
            return GetHandRankDisplayName(handRanks[index]);
        }
        return "Unknown";
    }

    private string GetHandRankDisplayName(HandRankType handRank)
    {
        switch (handRank)
        {
            case HandRankType.OnePair: return "One Pair";
            case HandRankType.TwoPair: return "Two Pair";
            case HandRankType.RedFlush: return "Red Flush";
            case HandRankType.BlackFlush: return "Black Flush";
            case HandRankType.Triple: return "Three of a Kind";
            case HandRankType.Straight: return "Straight";
            case HandRankType.Flush: return "Flush";
            case HandRankType.FullHouse: return "Full House";
            case HandRankType.FourKind: return "Four of a Kind";
            case HandRankType.StraightFlush: return "Straight Flush";
            case HandRankType.RoyalStraightFlush: return "Royal Straight Flush";
            default: return "None";
        }
    }

    public float GetWinRate()
    {
        if (totalRoundsPlayed == 0) return 0f;

        int totalWins = 0;
        foreach (int count in handRankingCounts)
        {
            totalWins += count;
        }

        return (float)totalWins / totalRoundsPlayed * 100f;
    }

    public void ResetAllStatistics()
    {
        totalGamesPlayed = 0;
        totalRoundsPlayed = 0;
        totalMoneyWon = 0;
        totalMoneyLost = 0;
        currentWinStreak = 0;
        bestWinStreak = 0;
        currentLoseStreak = 0;
        worstLoseStreak = 0;
        royalStraightFlushCount = 0;
        comboAchievements = 0;
        bigWins = 0;
        totalPlayTime = 0f;

        for (int i = 0; i < handRankingCounts.Length; i++)
        {
            handRankingCounts[i] = 0;
        }

        SaveStatistics();
    }

    public void SaveStatistics()
    {
        StatisticsData data = new StatisticsData(this);
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("RSF_Statistics", json);
        PlayerPrefs.Save();
    }

    public void LoadStatistics()
    {
        if (PlayerPrefs.HasKey("RSF_Statistics"))
        {
            string json = PlayerPrefs.GetString("RSF_Statistics");
            StatisticsData data = JsonUtility.FromJson<StatisticsData>(json);
            data.ApplyTo(this);
        }
    }
}

[Serializable]
public class StatisticsData
{
    public int totalGamesPlayed;
    public int totalRoundsPlayed;
    public int totalMoneyWon;
    public int totalMoneyLost;
    public int currentWinStreak;
    public int bestWinStreak;
    public int currentLoseStreak;
    public int worstLoseStreak;
    public int[] handRankingCounts;
    public int royalStraightFlushCount;
    public int comboAchievements;
    public int bigWins;
    public float totalPlayTime;

    public StatisticsData(GameStatistics stats)
    {
        totalGamesPlayed = stats.totalGamesPlayed;
        totalRoundsPlayed = stats.totalRoundsPlayed;
        totalMoneyWon = stats.totalMoneyWon;
        totalMoneyLost = stats.totalMoneyLost;
        currentWinStreak = stats.currentWinStreak;
        bestWinStreak = stats.bestWinStreak;
        currentLoseStreak = stats.currentLoseStreak;
        worstLoseStreak = stats.worstLoseStreak;
        handRankingCounts = (int[])stats.handRankingCounts.Clone();
        royalStraightFlushCount = stats.royalStraightFlushCount;
        comboAchievements = stats.comboAchievements;
        bigWins = stats.bigWins;
        totalPlayTime = stats.totalPlayTime;
    }

    public void ApplyTo(GameStatistics stats)
    {
        stats.totalGamesPlayed = totalGamesPlayed;
        stats.totalRoundsPlayed = totalRoundsPlayed;
        stats.totalMoneyWon = totalMoneyWon;
        stats.totalMoneyLost = totalMoneyLost;
        stats.currentWinStreak = currentWinStreak;
        stats.bestWinStreak = bestWinStreak;
        stats.currentLoseStreak = currentLoseStreak;
        stats.worstLoseStreak = worstLoseStreak;
        stats.handRankingCounts = (int[])handRankingCounts.Clone();
        stats.royalStraightFlushCount = royalStraightFlushCount;
        stats.comboAchievements = comboAchievements;
        stats.bigWins = bigWins;
        stats.totalPlayTime = totalPlayTime;
    }
}