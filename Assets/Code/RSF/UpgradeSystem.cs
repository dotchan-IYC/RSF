using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeCost
    {
        public HandRankType handRank;
        public int[] levelCosts = new int[4];
    }

    [Header("Upgrade Costs")]
    public UpgradeCost[] upgradeCosts;

    public static UpgradeSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeUpgradeCosts();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeUpgradeCosts()
    {
        upgradeCosts = new UpgradeCost[11];

        upgradeCosts[0] = CreateUpgradeCost(HandRankType.OnePair, new int[] { 20000, 30000, 40000, 50000 });
        upgradeCosts[1] = CreateUpgradeCost(HandRankType.TwoPair, new int[] { 30000, 35000, 40000, 45000 });
        upgradeCosts[2] = CreateUpgradeCost(HandRankType.RedFlush, new int[] { 35000, 40000, 45000, 50000 });
        upgradeCosts[3] = CreateUpgradeCost(HandRankType.BlackFlush, new int[] { 35000, 40000, 45000, 50000 });
        upgradeCosts[4] = CreateUpgradeCost(HandRankType.Triple, new int[] { 40000, 45000, 50000, 60000 });
        upgradeCosts[5] = CreateUpgradeCost(HandRankType.Straight, new int[] { 45000, 50000, 55000, 60000 });
        upgradeCosts[6] = CreateUpgradeCost(HandRankType.Flush, new int[] { 45000, 50000, 55000, 60000 });
        upgradeCosts[7] = CreateUpgradeCost(HandRankType.FullHouse, new int[] { 50000, 55000, 60000, 65000 });
        upgradeCosts[8] = CreateUpgradeCost(HandRankType.FourKind, new int[] { 60000, 70000, 75000, 80000 });
        upgradeCosts[9] = CreateUpgradeCost(HandRankType.StraightFlush, new int[] { 70000, 80000, 90000, 100000 });
        upgradeCosts[10] = CreateUpgradeCost(HandRankType.RoyalStraightFlush, new int[] { 50000, 50000, 50000, 50000 });
    }

    private UpgradeCost CreateUpgradeCost(HandRankType handRank, int[] costs)
    {
        UpgradeCost uc = new UpgradeCost();
        uc.handRank = handRank;
        uc.levelCosts = costs;
        return uc;
    }

    public int GetUpgradeCost(HandRankType handRank, int currentLevel)
    {
        if (currentLevel >= 5) return -1;

        foreach (var cost in upgradeCosts)
        {
            if (cost.handRank == handRank)
            {
                return cost.levelCosts[currentLevel - 1];
            }
        }
        return -1;
    }

    public bool CanUpgrade(HandRankType handRank)
    {
        PlayerData playerData = PlayerData.Instance;
        int currentLevel = playerData.GetHandRankLevel(handRank);
        int cost = GetUpgradeCost(handRank, currentLevel);

        return cost != -1 && playerData.CanAfford(cost);
    }

    public bool TryUpgrade(HandRankType handRank)
    {
        PlayerData playerData = PlayerData.Instance;
        int currentLevel = playerData.GetHandRankLevel(handRank);
        int cost = GetUpgradeCost(handRank, currentLevel);

        if (cost != -1 && playerData.CanAfford(cost))
        {
            playerData.SpendMoney(cost);
            playerData.UpgradeHandRank(handRank);
            return true;
        }
        return false;
    }
}
