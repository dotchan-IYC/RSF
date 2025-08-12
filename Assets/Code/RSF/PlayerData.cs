using UnityEngine;
using System;

[Serializable]
public class PlayerData : MonoBehaviour
{
    [Header("Player Money")]
    public int currentMoney = 15000;
    public int currentBet = 0;

    [Header("Hand Rankings Upgrade Levels")]
    public int onePairLevel = 1;
    public int twoPairLevel = 1;
    public int redFlushLevel = 1;
    public int blackFlushLevel = 1;
    public int tripleLevel = 1;
    public int straightLevel = 1;
    public int flushLevel = 1;
    public int fullHouseLevel = 1;
    public int fourKindLevel = 1;
    public int straightFlushLevel = 1;
    public int royalStraightFlushLevel = 1;

    [Header("Combo System")]
    public int comboCount = 0;
    public HandRankType lastHandRank = HandRankType.None;

    public static PlayerData Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanAfford(int amount)
    {
        return currentMoney >= amount;
    }

    public void SpendMoney(int amount)
    {
        currentMoney -= amount;
        currentMoney = Mathf.Max(0, currentMoney);
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
    }

    public void SetBet(int betAmount)
    {
        currentBet = betAmount;
    }

    public int GetHandRankLevel(HandRankType handRank)
    {
        switch (handRank)
        {
            case HandRankType.OnePair: return onePairLevel;
            case HandRankType.TwoPair: return twoPairLevel;
            case HandRankType.RedFlush: return redFlushLevel;
            case HandRankType.BlackFlush: return blackFlushLevel;
            case HandRankType.Triple: return tripleLevel;
            case HandRankType.Straight: return straightLevel;
            case HandRankType.Flush: return flushLevel;
            case HandRankType.FullHouse: return fullHouseLevel;
            case HandRankType.FourKind: return fourKindLevel;
            case HandRankType.StraightFlush: return straightFlushLevel;
            case HandRankType.RoyalStraightFlush: return royalStraightFlushLevel;
            default: return 1;
        }
    }

    public void UpgradeHandRank(HandRankType handRank)
    {
        switch (handRank)
        {
            case HandRankType.OnePair:
                onePairLevel = Mathf.Min(5, onePairLevel + 1);
                break;
            case HandRankType.TwoPair:
                twoPairLevel = Mathf.Min(5, twoPairLevel + 1);
                break;
            case HandRankType.RedFlush:
                redFlushLevel = Mathf.Min(5, redFlushLevel + 1);
                break;
            case HandRankType.BlackFlush:
                blackFlushLevel = Mathf.Min(5, blackFlushLevel + 1);
                break;
            case HandRankType.Triple:
                tripleLevel = Mathf.Min(5, tripleLevel + 1);
                break;
            case HandRankType.Straight:
                straightLevel = Mathf.Min(5, straightLevel + 1);
                break;
            case HandRankType.Flush:
                flushLevel = Mathf.Min(5, flushLevel + 1);
                break;
            case HandRankType.FullHouse:
                fullHouseLevel = Mathf.Min(5, fullHouseLevel + 1);
                break;
            case HandRankType.FourKind:
                fourKindLevel = Mathf.Min(5, fourKindLevel + 1);
                break;
            case HandRankType.StraightFlush:
                straightFlushLevel = Mathf.Min(5, straightFlushLevel + 1);
                break;
            case HandRankType.RoyalStraightFlush:
                royalStraightFlushLevel = Mathf.Min(5, royalStraightFlushLevel + 1);
                break;
        }
    }

    public void UpdateCombo(HandRankType currentHandRank)
    {
        if (currentHandRank == lastHandRank && currentHandRank != HandRankType.None)
        {
            comboCount++;
        }
        else
        {
            comboCount = 1;
            lastHandRank = currentHandRank;
        }
    }

    public bool IsComboActive()
    {
        return comboCount >= 3;
    }
}