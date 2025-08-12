using UnityEngine;
using System.Collections.Generic;

public class PayoutManager : MonoBehaviour
{
    [System.Serializable]
    public class HandRankMultiplier
    {
        public HandRankType handRank;
        public float[] levelMultipliers = new float[5];
    }

    [Header("Hand Rank Multipliers")]
    public List<HandRankMultiplier> handRankMultipliers = new List<HandRankMultiplier>();

    [Header("RSF Probability")]
    public float[] rsfProbabilities = { 0.003f, 0.006f, 0.012f, 0.024f, 0.048f };

    public static PayoutManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeMultipliers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeMultipliers()
    {
        handRankMultipliers.Clear();

        AddHandRankMultiplier(HandRankType.OnePair, new float[] { 1.0f, 1.2f, 1.3f, 1.4f, 1.5f });
        AddHandRankMultiplier(HandRankType.TwoPair, new float[] { 2.0f, 2.5f, 2.9f, 3.3f, 3.5f });
        AddHandRankMultiplier(HandRankType.RedFlush, new float[] { 2.5f, 3.0f, 3.5f, 4.0f, 4.5f });
        AddHandRankMultiplier(HandRankType.BlackFlush, new float[] { 2.5f, 3.0f, 3.5f, 4.0f, 4.5f });
        AddHandRankMultiplier(HandRankType.Triple, new float[] { 3.0f, 4.0f, 4.5f, 5.0f, 5.5f });
        AddHandRankMultiplier(HandRankType.Straight, new float[] { 20.0f, 28.0f, 34.0f, 40.0f, 45.0f });
        AddHandRankMultiplier(HandRankType.Flush, new float[] { 20.0f, 28.0f, 34.0f, 40.0f, 45.0f });
        AddHandRankMultiplier(HandRankType.FullHouse, new float[] { 25.0f, 40.0f, 50.0f, 55.0f, 60.0f });
        AddHandRankMultiplier(HandRankType.FourKind, new float[] { 50.0f, 85.0f, 110.0f, 130.0f, 140.0f });
        AddHandRankMultiplier(HandRankType.StraightFlush, new float[] { 300.0f, 500.0f, 700.0f, 850.0f, 1000.0f });
    }

    private void AddHandRankMultiplier(HandRankType handRank, float[] multipliers)
    {
        HandRankMultiplier hrm = new HandRankMultiplier();
        hrm.handRank = handRank;
        hrm.levelMultipliers = multipliers;
        handRankMultipliers.Add(hrm);
    }

    public int CalculatePayout(HandRankType handRank, int betAmount)
    {
        if (handRank == HandRankType.None || handRank == HandRankType.RoyalStraightFlush)
            return 0;

        PlayerData playerData = PlayerData.Instance;
        int level = playerData.GetHandRankLevel(handRank);
        float multiplier = GetMultiplier(handRank, level);

        int basePayout = (int)(betAmount * multiplier);

        if (playerData.IsComboActive())
        {
            basePayout *= 2;
        }

        return basePayout;
    }

    public float GetMultiplier(HandRankType handRank, int level)
    {
        foreach (var hrm in handRankMultipliers)
        {
            if (hrm.handRank == handRank)
            {
                return hrm.levelMultipliers[level - 1];
            }
        }
        return 0f;
    }

    public float GetRSFProbability(int level)
    {
        return rsfProbabilities[level - 1];
    }

    public bool CheckRSFWin()
    {
        PlayerData playerData = PlayerData.Instance;
        int rsfLevel = playerData.GetHandRankLevel(HandRankType.RoyalStraightFlush);
        float probability = GetRSFProbability(rsfLevel);

        return Random.Range(0f, 1f) < probability;
    }
}