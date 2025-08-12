using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Code.RSF;

public class UIManager : MonoBehaviour
{
    [Header("Main UI")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI betAmountText;
    public TextMeshProUGUI handRankText;
    public TextMeshProUGUI payoutText;
    public TextMeshProUGUI comboText;
    public CardDealer cardDealer;

    [Header("Betting UI")]
    public Slider betSlider;
    public Button placeBetButton;
    public Button[] quickBetButtons;
    public int[] quickBetAmounts = { 100, 500, 1000, 5000 };

    [Header("Upgrade UI")]
    public GameObject upgradePanel;
    public Transform upgradeButtonsParent;
    public GameObject upgradeButtonPrefab;
    public Button upgradeToggleButton;

    [Header("Game Control")]
    public Button dealButton;
    public Button changeCardsButton;
    public Button restartButton;
    public Button statisticsButton;

    [Header("Result UI")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public Button continueButton;

    [Header("Statistics UI")]
    public StatisticsUIManager statisticsUI;

    private int currentBetAmount = 100;

    private void Start()
    {
        InitializeUI();
        SetupEventListeners();
        CreateUpgradeButtons();
    }

    private void InitializeUI()
    {
        if (betSlider != null)
        {
            betSlider.minValue = GameManager.Instance.minBet;
            betSlider.maxValue = GameManager.Instance.maxBet;
            betSlider.value = currentBetAmount;
        }

        // Quick bet buttons setup
        for (int i = 0; i < quickBetButtons.Length && i < quickBetAmounts.Length; i++)
        {
            if (quickBetButtons[i] != null)
            {
                int betAmount = quickBetAmounts[i];
                quickBetButtons[i].onClick.AddListener(() => SetQuickBet(betAmount));
                var buttonText = quickBetButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = "$" + betAmount.ToString();
                }
            }
        }

        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        UpdateUI();
    }

    public void SetupEventListeners()
    {
        if (betSlider != null)
            betSlider.onValueChanged.AddListener(OnBetSliderChanged);

        if (placeBetButton != null)
            placeBetButton.onClick.AddListener(OnPlaceBetClicked);

        if (dealButton != null)
            dealButton.onClick.AddListener(OnDealClicked);

        if (changeCardsButton != null)
            changeCardsButton.onClick.AddListener(OnChangeCardsClicked);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        if (upgradeToggleButton != null)
            upgradeToggleButton.onClick.AddListener(ToggleUpgradePanel);

        if (statisticsButton != null)
            statisticsButton.onClick.AddListener(OpenStatistics);

        // Game Manager events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHandRankResult.AddListener(OnHandRankResult);
            GameManager.Instance.OnPayoutCalculated.AddListener(OnPayoutCalculated);
            GameManager.Instance.OnGameWon.AddListener(OnGameWon);
            GameManager.Instance.OnGameLost.AddListener(OnGameLost);
        }
    }

    private void CreateUpgradeButtons()
    {
        if (upgradeButtonsParent == null || upgradeButtonPrefab == null) return;

        HandRankType[] handRanks = {
            HandRankType.OnePair, HandRankType.TwoPair, HandRankType.RedFlush, HandRankType.BlackFlush,
            HandRankType.Triple, HandRankType.Straight, HandRankType.Flush, HandRankType.FullHouse,
            HandRankType.FourKind, HandRankType.StraightFlush, HandRankType.RoyalStraightFlush
        };

        foreach (HandRankType handRank in handRanks)
        {
            GameObject buttonObj = Instantiate(upgradeButtonPrefab, upgradeButtonsParent);
            UpgradeButton upgradeButton = buttonObj.GetComponent<UpgradeButton>();
            if (upgradeButton != null)
            {
                upgradeButton.Setup(handRank);
            }
        }
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (PlayerData.Instance == null) return;

        PlayerData playerData = PlayerData.Instance;

        if (moneyText != null)
            moneyText.text = "Money: $" + playerData.currentMoney.ToString();

        if (betAmountText != null)
            betAmountText.text = "Bet: $" + currentBetAmount.ToString();

        // Combo display
        if (comboText != null)
        {
            if (playerData.comboCount > 1)
            {
                comboText.text = "Combo: " + playerData.comboCount + "x";
                comboText.color = playerData.IsComboActive() ? Color.yellow : Color.white;
            }
            else
            {
                comboText.text = "";
            }
        }

        // Button states
        if (placeBetButton != null)
            placeBetButton.interactable = !GameManager.Instance.isRoundInProgress &&
                                         playerData.CanAfford(currentBetAmount);

        if (dealButton != null)
            dealButton.interactable = GameManager.Instance.isRoundInProgress;

        if (changeCardsButton != null)
            changeCardsButton.interactable = GameManager.Instance.isRoundInProgress;
    }

    public void OnBetSliderChanged(float value)
    {
        currentBetAmount = (int)value;
    }

    private void SetQuickBet(int amount)
    {
        currentBetAmount = amount;
        if (betSlider != null)
            betSlider.value = amount;
    }

    public void OnPlaceBetClicked()
    {
        if (GameManager.Instance.PlaceBet(currentBetAmount))
        {
            Debug.Log("Bet placed: $" + currentBetAmount);
        }
    }

    public void OnDealClicked()
    {
        // 기존 카드 시스템과 연결
        HandRankingsBridge bridge = FindAnyObjectByType<HandRankingsBridge>();
        if (bridge != null)
        {
            bridge.EvaluateHand();
        }
        Debug.Log("Dealing cards...");
        if (cardDealer != null)
        {
            cardDealer.DealCards();
        }
    }

    public void OnChangeCardsClicked()
    {
        Debug.Log("Change cards...");
    }

    public void OnRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnContinueClicked()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);
    }

    public void OnHandRankResult(HandRankType handRank)
    {
        if (handRankText != null)
            handRankText.text = "Hand: " + GetHandRankDisplayName(handRank);
    }

    public void OnPayoutCalculated(int payout)
    {
        if (payoutText != null)
            payoutText.text = "Payout: $" + payout.ToString();

        if (payout > 0)
        {
            ShowResult("You won $" + payout + "!");
        }
        else
        {
            ShowResult("No win this round.");
        }
    }

    public void OnGameWon()
    {
        ShowResult("ROYAL STRAIGHT FLUSH! YOU WIN!", true);
    }

    public void OnGameLost()
    {
        ShowResult("Game Over! Not enough money to continue.", true);
    }

    private void ShowResult(string message, bool gameEnd = false)
    {
        if (resultPanel == null || resultText == null || continueButton == null) return;

        resultText.text = message;
        resultPanel.SetActive(true);

        var buttonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
        if (gameEnd)
        {
            if (buttonText != null) buttonText.text = "Restart";
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnRestartClicked);
        }
        else
        {
            if (buttonText != null) buttonText.text = "Continue";
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinueClicked);
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
            case HandRankType.Triple: return "Three of a Kind";
            case HandRankType.Straight: return "Straight";
            case HandRankType.Flush: return "Flush";
            case HandRankType.FullHouse: return "Full House";
            case HandRankType.FourKind: return "Four of a Kind";
            case HandRankType.StraightFlush: return "Straight Flush";
            case HandRankType.RoyalStraightFlush: return "Royal Straight Flush";
            default: return "High Card";
        }
    }

    public void ToggleUpgradePanel()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(!upgradePanel.activeSelf);
    }

    private void OpenStatistics()
    {
        if (statisticsUI != null)
            statisticsUI.OpenStatistics();
    }
}