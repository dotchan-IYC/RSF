using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Code.RSF
{
    public class HandRankings : MonoBehaviour
    {
        [Header("Current Hand Info")]
        public string currentHandRanking;
        public HandRankType currentHandRankType = HandRankType.None;

        [Header("Components")]
        private HandCard handCard;
        private CommunityCards communityCards;
        private CardChange cardChange;

        [Header("Debug")]
        public bool showDebugInfo = false;

        [Header("Manual Card References (Fallback)")]
        public GameObject[] manualHandCards = new GameObject[2];
        public GameObject[] manualCommunityCards = new GameObject[3];

        private void Start()
        {
            handCard = GetComponent<HandCard>();
            communityCards = GetComponent<CommunityCards>();
            cardChange = GetComponent<CardChange>();

            if (cardChange != null)
            {
                cardChange.ChangeHandRanking += UpdateHandRanking;
            }

            // �ʱ� ���� üũ
            Invoke("CheckInitialHand", 1f);
        }

        private void CheckInitialHand()
        {
            UpdateHandRanking();
        }

        private void UpdateHandRanking()
        {
            // 5���� ī�� �迭 ���� (���� 2�� + Ŀ�´�Ƽ 3��)
            GameObject[] allCards = new GameObject[5];

            // ���� 2�� �������� (���� ��� �õ�)
            GameObject[] handCards = GetHandCards();
            for (int i = 0; i < 2 && i < handCards.Length; i++)
            {
                allCards[i] = handCards[i];
            }

            // Ŀ�´�Ƽ ī�� 3�� �������� (���� ��� �õ�)
            GameObject[] commCards = GetCommunityCards();
            for (int i = 0; i < 3 && i < commCards.Length; i++)
            {
                allCards[i + 2] = commCards[i];
            }

            // �����: ī�� ���� ���
            if (showDebugInfo)
            {
                Debug.Log("=== Card Debug Info ===");
                for (int i = 0; i < allCards.Length; i++)
                {
                    if (allCards[i] != null)
                    {
                        Debug.Log($"Card {i}: {allCards[i].name}");
                    }
                    else
                    {
                        Debug.Log($"Card {i}: NULL");
                    }
                }
            }

            // ���� ����
            currentHandRankType = EvaluateHand(allCards);
            currentHandRanking = GetHandRankingName(currentHandRankType);

            if (showDebugInfo)
            {
                Debug.Log("Current Hand Ranking: " + currentHandRanking);
            }

            // GameManager�� ��� ����
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ProcessHandResult(currentHandRankType);
            }
        }

        // ���� ī����� �������� �Լ� (���� ��� �õ�)
        private GameObject[] GetHandCards()
        {
            GameObject[] cards = new GameObject[2];

            if (handCard != null)
            {
                // ��� 1: _handCards �ʵ� �õ�
                try
                {
                    var field = handCard.GetType().GetField("_handCards");
                    if (field != null)
                    {
                        GameObject[] fieldValue = field.GetValue(handCard) as GameObject[];
                        if (fieldValue != null && fieldValue.Length >= 2)
                        {
                            return fieldValue;
                        }
                    }
                }
                catch { }

                // ��� 2: handCards �ʵ� �õ�
                try
                {
                    var field = handCard.GetType().GetField("handCards");
                    if (field != null)
                    {
                        GameObject[] fieldValue = field.GetValue(handCard) as GameObject[];
                        if (fieldValue != null && fieldValue.Length >= 2)
                        {
                            return fieldValue;
                        }
                    }
                }
                catch { }

                // ��� 3: _handCard �ʵ� �õ�
                try
                {
                    var field = handCard.GetType().GetField("_handCard");
                    if (field != null)
                    {
                        GameObject[] fieldValue = field.GetValue(handCard) as GameObject[];
                        if (fieldValue != null && fieldValue.Length >= 2)
                        {
                            return fieldValue;
                        }
                    }
                }
                catch { }

                // ��� 4: cards �ʵ� �õ�
                try
                {
                    var field = handCard.GetType().GetField("cards");
                    if (field != null)
                    {
                        GameObject[] fieldValue = field.GetValue(handCard) as GameObject[];
                        if (fieldValue != null && fieldValue.Length >= 2)
                        {
                            return fieldValue;
                        }
                    }
                }
                catch { }
            }

            // ��� 5: ���� ������ ī��� ���
            if (manualHandCards != null && manualHandCards.Length >= 2)
            {
                bool hasValidCards = true;
                for (int i = 0; i < 2; i++)
                {
                    if (manualHandCards[i] == null)
                    {
                        hasValidCards = false;
                        break;
                    }
                }
                if (hasValidCards)
                {
                    return manualHandCards;
                }
            }

            Debug.LogWarning("Could not find hand cards! Please check HandCard component or set manual references.");
            return cards; // �� �迭 ��ȯ
        }

        // Ŀ�´�Ƽ ī����� �������� �Լ� (���� ��� �õ�)
        private GameObject[] GetCommunityCards()
        {
            GameObject[] cards = new GameObject[3];

            if (communityCards != null)
            {
                // ��� 1: _communityCards �ʵ� �õ�
                try
                {
                    var field = communityCards.GetType().GetField("_communityCards");
                    if (field != null)
                    {
                        GameObject[] fieldValue = field.GetValue(communityCards) as GameObject[];
                        if (fieldValue != null && fieldValue.Length >= 3)
                        {
                            return fieldValue;
                        }
                    }
                }
                catch { }

                // ��� 2: communityCards �ʵ� �õ�
                try
                {
                    var field = communityCards.GetType().GetField("communityCards");
                    if (field != null)
                    {
                        GameObject[] fieldValue = field.GetValue(communityCards) as GameObject[];
                        if (fieldValue != null && fieldValue.Length >= 3)
                        {
                            return fieldValue;
                        }
                    }
                }
                catch { }

                // ��� 3: _communityCard �ʵ� �õ�
                try
                {
                    var field = communityCards.GetType().GetField("_communityCard");
                    if (field != null)
                    {
                        GameObject[] fieldValue = field.GetValue(communityCards) as GameObject[];
                        if (fieldValue != null && fieldValue.Length >= 3)
                        {
                            return fieldValue;
                        }
                    }
                }
                catch { }

                // ��� 4: cards �ʵ� �õ�
                try
                {
                    var field = communityCards.GetType().GetField("cards");
                    if (field != null)
                    {
                        GameObject[] fieldValue = field.GetValue(communityCards) as GameObject[];
                        if (fieldValue != null && fieldValue.Length >= 3)
                        {
                            return fieldValue;
                        }
                    }
                }
                catch { }
            }

            // ��� 5: ���� ������ ī��� ���
            if (manualCommunityCards != null && manualCommunityCards.Length >= 3)
            {
                bool hasValidCards = true;
                for (int i = 0; i < 3; i++)
                {
                    if (manualCommunityCards[i] == null)
                    {
                        hasValidCards = false;
                        break;
                    }
                }
                if (hasValidCards)
                {
                    return manualCommunityCards;
                }
            }

            Debug.LogWarning("Could not find community cards! Please check CommunityCards component or set manual references.");
            return cards; // �� �迭 ��ȯ
        }

        public HandRankType EvaluateHand(GameObject[] cards)
        {
            if (cards == null || cards.Length != 5)
            {
                Debug.LogWarning("Invalid card array for hand evaluation");
                return HandRankType.None;
            }

            // ī�� ���� ����
            List<CardInfo> cardInfos = new List<CardInfo>();

            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] != null)
                {
                    // ���� ������� ī�� ���� �������� �õ�
                    CardInfo info = GetCardInfo(cards[i]);
                    if (info.value > 0) // ��ȿ�� ī������ Ȯ��
                    {
                        cardInfos.Add(info);
                    }
                }
            }

            if (cardInfos.Count != 5)
            {
                Debug.LogWarning($"Could not extract all card info. Found {cardInfos.Count}/5 cards");
                return HandRankType.None;
            }

            // ī�带 ������ ����
            cardInfos.Sort((a, b) => a.value.CompareTo(b.value));

            if (showDebugInfo)
            {
                Debug.Log("=== Card Values ===");
                foreach (var card in cardInfos)
                {
                    Debug.Log($"Value: {card.value}, Suit: {card.suit}");
                }
            }

            // ���� ���� (���� �������� Ȯ��)
            if (IsRoyalStraightFlush(cardInfos)) return HandRankType.RoyalStraightFlush;
            if (IsStraightFlush(cardInfos)) return HandRankType.StraightFlush;
            if (IsFourOfAKind(cardInfos)) return HandRankType.FourKind;
            if (IsFullHouse(cardInfos)) return HandRankType.FullHouse;
            if (IsFlush(cardInfos)) return HandRankType.Flush;
            if (IsStraight(cardInfos)) return HandRankType.Straight;
            if (IsThreeOfAKind(cardInfos)) return HandRankType.Triple;
            if (IsTwoPair(cardInfos)) return HandRankType.TwoPair;
            if (IsOnePair(cardInfos)) return HandRankType.OnePair;
            if (IsRedFlush(cardInfos)) return HandRankType.RedFlush;
            if (IsBlackFlush(cardInfos)) return HandRankType.BlackFlush;

            return HandRankType.None;
        }

        // ī�� ������ �����ϴ� �Լ� (���� ��� �õ�)
        private CardInfo GetCardInfo(GameObject cardObject)
        {
            CardInfo info = new CardInfo();
            info.gameObject = cardObject;

            // ��� 1: CardScript ������Ʈ���� ��������
            CardScript cardScript = cardObject.GetComponent<CardScript>();
            if (cardScript != null)
            {
                // RSF �ý��ۿ� �ʵ尡 �ִ��� Ȯ��
                try
                {
                    var valueField = cardScript.GetType().GetField("cardValue");
                    var suitField = cardScript.GetType().GetField("cardSuit");

                    if (valueField != null && suitField != null)
                    {
                        info.value = (int)valueField.GetValue(cardScript);
                        info.suit = (CardSuit)suitField.GetValue(cardScript);

                        if (info.value > 0 && info.value <= 13)
                        {
                            return info;
                        }
                    }
                }
                catch { }
            }

            // ��� 2: ī�� �̸����� ���� (��: "HeartAce", "SpadeKing" ��)
            string cardName = cardObject.name.ToLower();

            // ��Ʈ �Ǻ�
            if (cardName.Contains("heart"))
                info.suit = CardSuit.Hearts;
            else if (cardName.Contains("diamond"))
                info.suit = CardSuit.Diamonds;
            else if (cardName.Contains("club"))
                info.suit = CardSuit.Clubs;
            else if (cardName.Contains("spade"))
                info.suit = CardSuit.Spades;

            // �� �Ǻ�
            if (cardName.Contains("ace") || cardName.Contains("a"))
                info.value = 1;
            else if (cardName.Contains("king") || cardName.Contains("k"))
                info.value = 13;
            else if (cardName.Contains("queen") || cardName.Contains("q"))
                info.value = 12;
            else if (cardName.Contains("jack") || cardName.Contains("j"))
                info.value = 11;
            else if (cardName.Contains("10"))
                info.value = 10;
            else if (cardName.Contains("9"))
                info.value = 9;
            else if (cardName.Contains("8"))
                info.value = 8;
            else if (cardName.Contains("7"))
                info.value = 7;
            else if (cardName.Contains("6"))
                info.value = 6;
            else if (cardName.Contains("5"))
                info.value = 5;
            else if (cardName.Contains("4"))
                info.value = 4;
            else if (cardName.Contains("3"))
                info.value = 3;
            else if (cardName.Contains("2"))
                info.value = 2;

            // ��� 3: �⺻�� ���� (�׽�Ʈ��)
            if (info.value == 0)
            {
                Debug.LogWarning($"Could not determine card value for {cardObject.name}. Using default values.");
                info.value = Random.Range(1, 14); // �ӽ� ���� ��
                info.suit = (CardSuit)Random.Range(0, 4); // �ӽ� ���� ��Ʈ
            }

            return info;
        }

        // ���� ���� �Լ���
        private bool IsRoyalStraightFlush(List<CardInfo> cards)
        {
            if (!IsFlush(cards)) return false;

            int[] royalValues = { 1, 10, 11, 12, 13 };
            var values = cards.Select(c => c.value).OrderBy(v => v).ToArray();

            return values.SequenceEqual(royalValues);
        }

        private bool IsStraightFlush(List<CardInfo> cards)
        {
            return IsFlush(cards) && IsStraight(cards);
        }

        private bool IsFourOfAKind(List<CardInfo> cards)
        {
            var groups = cards.GroupBy(c => c.value);
            return groups.Any(g => g.Count() == 4);
        }

        private bool IsFullHouse(List<CardInfo> cards)
        {
            var groups = cards.GroupBy(c => c.value);
            return groups.Count() == 2 && groups.Any(g => g.Count() == 3) && groups.Any(g => g.Count() == 2);
        }

        private bool IsFlush(List<CardInfo> cards)
        {
            var firstSuit = cards[0].suit;
            return cards.All(c => c.suit == firstSuit);
        }

        private bool IsStraight(List<CardInfo> cards)
        {
            var values = cards.Select(c => c.value).OrderBy(v => v).ToArray();

            // �Ϲ����� ��Ʈ����Ʈ Ȯ��
            for (int i = 0; i < values.Length - 1; i++)
            {
                if (values[i + 1] - values[i] != 1)
                {
                    // A-2-3-4-5 ��Ʈ����Ʈ Ȯ�� (A�� 1�� ���)
                    if (i == 0 && values[0] == 1 && values[4] == 13)
                    {
                        // A-10-J-Q-K Ȯ��
                        int[] aceHighValues = { 1, 10, 11, 12, 13 };
                        return values.SequenceEqual(aceHighValues);
                    }
                    return false;
                }
            }

            return true;
        }

        private bool IsThreeOfAKind(List<CardInfo> cards)
        {
            var groups = cards.GroupBy(c => c.value);
            return groups.Any(g => g.Count() == 3);
        }

        private bool IsTwoPair(List<CardInfo> cards)
        {
            var groups = cards.GroupBy(c => c.value);
            return groups.Count(g => g.Count() == 2) == 2;
        }

        private bool IsOnePair(List<CardInfo> cards)
        {
            var groups = cards.GroupBy(c => c.value);
            return groups.Count(g => g.Count() == 2) == 1;
        }

        private bool IsRedFlush(List<CardInfo> cards)
        {
            bool allRed = cards.All(c => c.suit == CardSuit.Hearts || c.suit == CardSuit.Diamonds);
            return allRed && !IsFlush(cards);
        }

        private bool IsBlackFlush(List<CardInfo> cards)
        {
            bool allBlack = cards.All(c => c.suit == CardSuit.Spades || c.suit == CardSuit.Clubs);
            return allBlack && !IsFlush(cards);
        }

        private string GetHandRankingName(HandRankType handRank)
        {
            switch (handRank)
            {
                case HandRankType.RoyalStraightFlush: return "Royal Straight Flush";
                case HandRankType.StraightFlush: return "Straight Flush";
                case HandRankType.FourKind: return "Four of a Kind";
                case HandRankType.FullHouse: return "Full House";
                case HandRankType.Flush: return "Flush";
                case HandRankType.Straight: return "Straight";
                case HandRankType.Triple: return "Three of a Kind";
                case HandRankType.TwoPair: return "Two Pair";
                case HandRankType.OnePair: return "One Pair";
                case HandRankType.RedFlush: return "Red Flush";
                case HandRankType.BlackFlush: return "Black Flush";
                default: return "High Card";
            }
        }

        // �������� ���� Ȯ�� (����׿�)
        [ContextMenu("Check Hand Ranking")]
        public void CheckHandRanking()
        {
            UpdateHandRanking();
        }

        // ���� ���� Ÿ�� ��ȯ
        public HandRankType GetCurrentHandRankType()
        {
            return currentHandRankType;
        }

        // ���� ���� �̸� ��ȯ
        public string GetCurrentHandRankingName()
        {
            return currentHandRanking;
        }

        // �������� ī�� �����ϴ� �Լ� (�׽�Ʈ��)
        public void SetManualCards(GameObject[] handCards, GameObject[] commCards)
        {
            if (handCards != null && handCards.Length >= 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    manualHandCards[i] = handCards[i];
                }
            }

            if (commCards != null && commCards.Length >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    manualCommunityCards[i] = commCards[i];
                }
            }

            UpdateHandRanking();
        }
    }

    // ī�� ������ ��� ����ü
    [System.Serializable]
    public struct CardInfo
    {
        public int value;
        public CardSuit suit;
        public GameObject gameObject;
    }
}
