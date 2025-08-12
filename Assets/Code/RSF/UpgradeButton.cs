using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI handRankText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI multiplierText;
    public Button upgradeButton;

    private HandRankType handRank;

    private void Start()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
    }

    public void Setup(HandRankType handRankType)
    {
        handRank = handRankType;
        UpdateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (PlayerData.Instance == null || UpgradeSystem.Instance == null || PayoutManager.Instance == null)
            return;

        PlayerData playerData = PlayerData.Instance;
        int currentLevel = playerData.GetHandRankLevel(handRank);
        int upgradeCost = UpgradeSystem.Instance.GetUpgradeCost(handRank, currentLevel);
        float currentMultiplier = PayoutManager.Instance.GetMultiplier(handRank, currentLevel);

        if (handRankText != null)
            handRankText.text = GetHandRankDisplayName(handRank);

        if (levelText != null)
            levelText.text = "Lv." + currentLevel;

        if (multiplierText != null)
            multiplierText.text = "x" + currentMultiplier.ToString("F1");

        if (costText != null && upgradeButton != null)
        {
            if (upgradeCost == -1)
            {
                costText.text = "MAX";
                upgradeButton.interactable = false;
            }
            else
            {
                costText.text = "$" + upgradeCost.ToString();
                upgradeButton.interactable = UpgradeSystem.Instance.CanUpgrade(handRank);
            }
        }
    }

    private void OnUpgradeClicked()
    {
        if (UpgradeSystem.Instance != null && UpgradeSystem.Instance.TryUpgrade(handRank))
        {
            Debug.Log("Upgraded " + handRank + " successfully!");
        }
    }

    private string GetHandRankDisplayName(HandRankType handRank)
    {
        switch (handRank)
        {
            case HandRankType.OnePair: return "One Pair";
            case HandRankType.TwoPair: return "Two Pair";
            case HandRankType.RedFlush: return "Red Flush";
            case HandRankType.BlackFlush: return "Black Flush";
            case HandRankType.Triple: return "Triple";
            case HandRankType.Straight: return "Straight";
            case HandRankType.Flush: return "Flush";
            case HandRankType.FullHouse: return "Full House";
            case HandRankType.FourKind: return "Four Kind";
            case HandRankType.StraightFlush: return "Straight Flush";
            case HandRankType.RoyalStraightFlush: return "Royal SF";
            default: return handRank.ToString();
        }
    }
}