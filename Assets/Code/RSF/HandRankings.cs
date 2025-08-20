using UnityEngine;
using UnityEngine.UI;
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

            // �ʱ� ���� üũ�� ī�尡 ������ �Ŀ��� ����
            // Invoke("CheckInitialHand", 1f); // ����: �� ī��� ���� ���� ����
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

                        // ��������Ʈ �̸��� ���
                        Image cardImage = allCards[i].GetComponent<Image>();
                        if (cardImage != null && cardImage.sprite != null)
                        {
                            Debug.Log($"  �� Sprite: {cardImage.sprite.name}");
                        }
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
                if (currentHandRankType != HandRankType.None)
                {
                    Debug.Log("Current Hand Ranking: " + currentHandRanking);
                }
                else
                {
                    Debug.Log("No hand ranking (cards not dealt or incomplete)");
                }
            }

            // GameManager�� ��� ���� (ī�尡 ������ ������ ��츸)
            if (GameManager.Instance != null && currentHandRankType != HandRankType.None)
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

            // ī�尡 �ϳ��� �������� ���� ���
            if (cardInfos.Count == 0)
            {
                if (showDebugInfo)
                {
                    Debug.Log("No cards dealt yet - skipping hand evaluation");
                }
                return HandRankType.None;
            }

            // 5���� ��� �������� ���� ���
            if (cardInfos.Count != 5)
            {
                if (showDebugInfo)
                {
                    Debug.Log($"Incomplete hand: Found {cardInfos.Count}/5 cards - skipping evaluation");
                }
                return HandRankType.None;
            }

            // ī�带 ������ ����
            cardInfos.Sort((a, b) => a.value.CompareTo(b.value));

            if (showDebugInfo)
            {
                //Debug.Log("=== Card Values ===");
                foreach (var card in cardInfos)
                {
                    //Debug.Log($"Value: {card.value}, Suit: {card.suit}");
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

        // ī�� ������ �����ϴ� �Լ� (�� ī�� ó�� ����)
        private CardInfo GetCardInfo(GameObject cardObject)
        {
            CardInfo info = new CardInfo();
            info.gameObject = cardObject;

            // ��� 1: CardScript ������Ʈ���� ��������
            CardScript cardScript = cardObject.GetComponent<CardScript>();
            if (cardScript != null)
            {
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
                            if (showDebugInfo)
                            {
                                Debug.Log($"Card info from CardScript: {cardObject.name} = Value: {info.value}, Suit: {info.suit}");
                            }
                            return info;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Error reading CardScript from {cardObject.name}: {e.Message}");
                }
            }

            // ��� 2: Image ������Ʈ�� ��������Ʈ �̸����� ����
            Image cardImage = cardObject.GetComponent<Image>();
            if (cardImage != null && cardImage.sprite != null)
            {
                string spriteName = cardImage.sprite.name;

                if (showDebugInfo)
                {
                    Debug.Log($"Reading sprite name: {spriteName} from object: {cardObject.name}");
                }

                // ��Ʈ �Ǻ� (��Ȯ�� ���� ���ڿ��� üũ)
                if (spriteName.StartsWith("Heart"))
                    info.suit = CardSuit.Hearts;
                else if (spriteName.StartsWith("Diamond"))
                    info.suit = CardSuit.Diamonds;
                else if (spriteName.StartsWith("Club"))
                    info.suit = CardSuit.Clubs;
                else if (spriteName.StartsWith("Spade"))
                    info.suit = CardSuit.Spades;
                else
                {
                    if (showDebugInfo)
                        Debug.LogWarning($"Cannot determine suit for sprite: {spriteName} on object: {cardObject.name}");
                    return info; // ��Ʈ�� �Ǻ��� �� ������ �⺻�� ��ȯ
                }

                // �� �Ǻ� (��Ȯ�� �� ���ڿ��� üũ)
                if (spriteName.EndsWith("Ace"))
                    info.value = 1;
                else if (spriteName.EndsWith("King"))
                    info.value = 13;
                else if (spriteName.EndsWith("Queen"))
                    info.value = 12;
                else if (spriteName.EndsWith("Jack"))
                    info.value = 11;
                else if (spriteName.EndsWith("10"))
                    info.value = 10;
                else if (spriteName.EndsWith("9"))
                    info.value = 9;
                else if (spriteName.EndsWith("8"))
                    info.value = 8;
                else if (spriteName.EndsWith("7"))
                    info.value = 7;
                else if (spriteName.EndsWith("6"))
                    info.value = 6;
                else if (spriteName.EndsWith("5"))
                    info.value = 5;
                else if (spriteName.EndsWith("4"))
                    info.value = 4;
                else if (spriteName.EndsWith("3"))
                    info.value = 3;
                else if (spriteName.EndsWith("2"))
                    info.value = 2;
                else
                {
                    if (showDebugInfo)
                        Debug.LogWarning($"Cannot determine value for sprite: {spriteName} on object: {cardObject.name}");
                    return info; // ���� �Ǻ��� �� ������ �⺻�� ��ȯ
                }

                if (showDebugInfo)
                {
                    Debug.Log($"Card info from sprite: {spriteName} = Value: {info.value}, Suit: {info.suit}");
                }

                return info;
            }

            // ��� 3: ī�� ������Ʈ �̸����� ���� (fallback)
            string cardName = cardObject.name;

            // ��Ʈ �Ǻ� (��Ȯ�� ���� ���ڿ��� üũ)
            if (cardName.StartsWith("Heart"))
                info.suit = CardSuit.Hearts;
            else if (cardName.StartsWith("Diamond"))
                info.suit = CardSuit.Diamonds;
            else if (cardName.StartsWith("Club"))
                info.suit = CardSuit.Clubs;
            else if (cardName.StartsWith("Spade"))
                info.suit = CardSuit.Spades;
            else
            {
                // �� ī��� ������ ���� (���� ��� ���׸�)
                if (showDebugInfo)
                    Debug.LogWarning($"Card {cardName} has no sprite assigned - skipping");
                return info; // �� ī��� value=0���� ��ȯ�Ǿ� ���õ�
            }

            // �� �Ǻ� (��Ȯ�� �� ���ڿ��� üũ)
            if (cardName.EndsWith("Ace"))
                info.value = 1;
            else if (cardName.EndsWith("King"))
                info.value = 13;
            else if (cardName.EndsWith("Queen"))
                info.value = 12;
            else if (cardName.EndsWith("Jack"))
                info.value = 11;
            else if (cardName.EndsWith("10"))
                info.value = 10;
            else if (cardName.EndsWith("9"))
                info.value = 9;
            else if (cardName.EndsWith("8"))
                info.value = 8;
            else if (cardName.EndsWith("7"))
                info.value = 7;
            else if (cardName.EndsWith("6"))
                info.value = 6;
            else if (cardName.EndsWith("5"))
                info.value = 5;
            else if (cardName.EndsWith("4"))
                info.value = 4;
            else if (cardName.EndsWith("3"))
                info.value = 3;
            else if (cardName.EndsWith("2"))
                info.value = 2;
            else
            {
                if (showDebugInfo)
                    Debug.LogWarning($"Cannot determine value for card: {cardName}");
                return info; // ���� �Ǻ��� �� ������ �⺻�� ��ȯ
            }

            if (showDebugInfo)
            {
                Debug.Log($"Card info from name: {cardName} = Value: {info.value}, Suit: {info.suit}");
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
                case HandRankType.None: return "No Hand";
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