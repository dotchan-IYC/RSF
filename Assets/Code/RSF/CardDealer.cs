using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Code.RSF
{
    public class CardDealer : MonoBehaviour
    {
        [Header("Card References")]
        public CardPool cardPool;

        [Header("UI Card Slots - SpriteRenderer")]
        public SpriteRenderer[] handCardSlots = new SpriteRenderer[2];      // Card1, Card2
        public SpriteRenderer[] communityCardSlots = new SpriteRenderer[3]; // CommunityCard1, 2, 3

        [Header("Animation")]
        public CardAnimationManager animationManager;

        [Header("Debug")]
        public bool showDebugInfo = true;

        // ���� ������ ī����� ����
        private List<int> usedCardIndices = new List<int>();

        private void Start()
        {
            // CardPool �ڵ� ã��
            if (cardPool == null)
                cardPool = FindAnyObjectByType<CardPool>();

            // AnimationManager �ڵ� ã��
            if (animationManager == null)
                animationManager = FindAnyObjectByType<CardAnimationManager>();

            // UI ���Ե� �ڵ� ã��
            AutoFindCardSlots();
        }

        // UI ���Ե��� �ڵ����� ã�� �Լ�
        private void AutoFindCardSlots()
        {
            // Hand Card ���Ե� ã��
            for (int i = 0; i < handCardSlots.Length; i++)
            {
                if (handCardSlots[i] == null)
                {
                    GameObject cardObj = GameObject.Find($"Card{i + 1}");
                    if (cardObj != null)
                    {
                        handCardSlots[i] = cardObj.GetComponent<SpriteRenderer>();
                        if (showDebugInfo && handCardSlots[i] != null)
                            Debug.Log($"Found hand card slot: {cardObj.name}");
                    }
                }
            }

            // Community Card ���Ե� ã��
            for (int i = 0; i < communityCardSlots.Length; i++)
            {
                if (communityCardSlots[i] == null)
                {
                    GameObject cardObj = GameObject.Find($"CommunityCard{i + 1}");
                    if (cardObj != null)
                    {
                        communityCardSlots[i] = cardObj.GetComponent<SpriteRenderer>();
                        if (showDebugInfo && communityCardSlots[i] != null)
                            Debug.Log($"Found community card slot: {cardObj.name}");
                    }
                }
            }
        }

        // ī�� ���� ���� �Լ�
        public void DealCards()
        {
            if (cardPool == null)
            {
                Debug.LogError("CardPool�� �������� �ʾҽ��ϴ�!");
                return;
            }

            // CardPool�� ī�� �迭�� ���� (public ������Ƽ �ʿ�)
            GameObject[] availableCards = GetCardsFromPool();
            if (availableCards == null || availableCards.Length == 0)
            {
                Debug.LogError("��� ������ ī�尡 �����ϴ�!");
                return;
            }

            // ���� ī�� �ε��� �ʱ�ȭ
            usedCardIndices.Clear();

            if (showDebugInfo)
                Debug.Log("=== ī�� ���� ���� ===");

            // �ڵ� ī�� 2�� ����
            for (int i = 0; i < handCardSlots.Length; i++)
            {
                if (handCardSlots[i] != null)
                {
                    Sprite cardSprite = GetRandomCardSprite(availableCards);
                    if (cardSprite != null)
                    {
                        handCardSlots[i].sprite = cardSprite;

                        if (showDebugInfo)
                            Debug.Log($"�ڵ� ī�� {i + 1}: {cardSprite.name}");
                    }
                }
            }

            // Ŀ�´�Ƽ ī�� 3�� ����
            for (int i = 0; i < communityCardSlots.Length; i++)
            {
                if (communityCardSlots[i] != null)
                {
                    Sprite cardSprite = GetRandomCardSprite(availableCards);
                    if (cardSprite != null)
                    {
                        communityCardSlots[i].sprite = cardSprite;

                        if (showDebugInfo)
                            Debug.Log($"Ŀ�´�Ƽ ī�� {i + 1}: {cardSprite.name}");
                    }
                }
            }

            // �ִϸ��̼� ����
            if (animationManager != null)
            {
                StartCoroutine(PlayDealAnimation());
            }

            if (showDebugInfo)
                Debug.Log("=== ī�� ���� �Ϸ� ===");
        }

        // CardPool���� ī�� �迭 ��������
        private GameObject[] GetCardsFromPool()
        {
            // ��� 1: public ������Ƽ ��� (CardPool�� �߰� �ʿ�)
            try
            {
                var cardsProperty = cardPool.GetType().GetProperty("Cards");
                if (cardsProperty != null)
                {
                    return cardsProperty.GetValue(cardPool) as GameObject[];
                }
            }
            catch { }

            // ��� 2: private �ʵ� ���÷������� ���� (�ӽ�)
            try
            {
                var cardsField = cardPool.GetType().GetField("_cards",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
                if (cardsField != null)
                {
                    return cardsField.GetValue(cardPool) as GameObject[];
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"ī�� Ǯ ���� ����: {e.Message}");
            }

            return null;
        }

        // ���� ī�� ��������Ʈ ���� (�ߺ� ����)
        private Sprite GetRandomCardSprite(GameObject[] availableCards)
        {
            if (usedCardIndices.Count >= availableCards.Length)
            {
                Debug.LogWarning("��� ī�尡 ���Ǿ����ϴ�! ī�带 �����մϴ�.");
                usedCardIndices.Clear();
            }

            int randomIndex;
            int attempts = 0;

            do
            {
                randomIndex = Random.Range(0, availableCards.Length);
                attempts++;

                // ���� ���� ����
                if (attempts > 100)
                {
                    Debug.LogWarning("ī�� ���ÿ� �����߽��ϴ�. ù ��° ��� ������ ī�带 ����մϴ�.");
                    for (int i = 0; i < availableCards.Length; i++)
                    {
                        if (!usedCardIndices.Contains(i))
                        {
                            randomIndex = i;
                            break;
                        }
                    }
                    break;
                }
            }
            while (usedCardIndices.Contains(randomIndex));

            usedCardIndices.Add(randomIndex);

            // GameObject���� Sprite ����
            GameObject cardObj = availableCards[randomIndex];
            return ExtractSpriteFromGameObject(cardObj);
        }

        // GameObject���� Sprite�� �����ϴ� �Լ�
        private Sprite ExtractSpriteFromGameObject(GameObject cardObj)
        {
            if (cardObj == null)
            {
                Debug.LogError("ī�� GameObject�� null�Դϴ�!");
                return null;
            }

            // ��� 1: SpriteRenderer���� sprite ��������
            SpriteRenderer sr = cardObj.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                if (showDebugInfo)
                    Debug.Log($"SpriteRenderer���� ��������Ʈ ����: {sr.sprite.name}");
                return sr.sprite;
            }

            // ��� 2: CardScript ������Ʈ���� ��������
            var cardScript = cardObj.GetComponent<CardScript>();
            if (cardScript != null)
            {
                // ���� ������ �ʵ�� �õ�
                var spriteField = cardScript.GetType().GetField("sprite") ??
                                 cardScript.GetType().GetField("cardSprite") ??
                                 cardScript.GetType().GetField("_sprite") ??
                                 cardScript.GetType().GetField("_cardSprite");

                if (spriteField != null)
                {
                    Sprite sprite = spriteField.GetValue(cardScript) as Sprite;
                    if (sprite != null)
                    {
                        if (showDebugInfo)
                            Debug.Log($"CardScript���� ��������Ʈ ����: {sprite.name}");
                        return sprite;
                    }
                }

                // ������Ƽ�� �õ�
                var spriteProperty = cardScript.GetType().GetProperty("sprite") ??
                                    cardScript.GetType().GetProperty("cardSprite") ??
                                    cardScript.GetType().GetProperty("Sprite") ??
                                    cardScript.GetType().GetProperty("CardSprite");

                if (spriteProperty != null)
                {
                    Sprite sprite = spriteProperty.GetValue(cardScript) as Sprite;
                    if (sprite != null)
                    {
                        if (showDebugInfo)
                            Debug.Log($"CardScript ������Ƽ���� ��������Ʈ ����: {sprite.name}");
                        return sprite;
                    }
                }
            }

            // ��� 3: ���� Sprite���� Ȯ�� (GameObject �̸����� Assets���� ã��)
            string cardName = cardObj.name;

            // Assets���� ���� �̸��� Sprite ã��
            Sprite foundSprite = Resources.Load<Sprite>($"Card/{cardName}");
            if (foundSprite != null)
            {
                if (showDebugInfo)
                    Debug.Log($"Resources���� ��������Ʈ �ε�: {foundSprite.name}");
                return foundSprite;
            }

            // ��� 4: ī�� �̸����� ��������Ʈ ã�� (�Ϲ����� ��ε�)
            string[] possiblePaths = {
                $"Cards/{cardName}",
                $"Sprites/{cardName}",
                $"04.Prefab/Card/{cardName}",
                cardName
            };

            foreach (string path in possiblePaths)
            {
                Sprite sprite = Resources.Load<Sprite>(path);
                if (sprite != null)
                {
                    if (showDebugInfo)
                        Debug.Log($"Resources({path})���� ��������Ʈ �ε�: {sprite.name}");
                    return sprite;
                }
            }

            Debug.LogError($"GameObject {cardName}���� Sprite�� ã�� �� �����ϴ�! " +
                          "SpriteRenderer�� CardScript�� �ִ��� Ȯ���ϼ���.");
            return null;
        }

        // ���� �ִϸ��̼� ����
        private System.Collections.IEnumerator PlayDealAnimation()
        {
            // �� ī�带 ������� �ִϸ��̼�
            yield return new WaitForSeconds(0.1f);

            // �ڵ� ī�� �ִϸ��̼�
            for (int i = 0; i < handCardSlots.Length; i++)
            {
                if (handCardSlots[i] != null && animationManager != null)
                {
                    // ī�� ������ �ִϸ��̼� �� ���� ����
                    yield return new WaitForSeconds(animationManager.dealDelay);
                }
            }

            // Ŀ�´�Ƽ ī�� �ִϸ��̼�
            for (int i = 0; i < communityCardSlots.Length; i++)
            {
                if (communityCardSlots[i] != null && animationManager != null)
                {
                    yield return new WaitForSeconds(animationManager.dealDelay);
                }
            }
        }

        // ī�� �ʱ�ȭ (RESET ��ư��)
        public void ResetCards()
        {
            // ��� ī�� ���� �ʱ�ȭ
            foreach (var slot in handCardSlots)
            {
                if (slot != null)
                    slot.sprite = null;
            }

            foreach (var slot in communityCardSlots)
            {
                if (slot != null)
                    slot.sprite = null;
            }

            usedCardIndices.Clear();

            if (showDebugInfo)
                Debug.Log("ī�尡 �ʱ�ȭ�Ǿ����ϴ�.");
        }

        // Ư�� ī�� ��ü (CHANGE CARDS ��ư��)
        public void ChangeCards(int[] cardIndices)
        {
            GameObject[] availableCards = GetCardsFromPool();
            if (availableCards == null) return;

            foreach (int index in cardIndices)
            {
                if (index >= 0 && index < handCardSlots.Length && handCardSlots[index] != null)
                {
                    // ���� ī�忡�� ����
                    string oldSpriteName = handCardSlots[index].sprite?.name;

                    // �� ī�� �Ҵ�
                    Sprite newCard = GetRandomCardSprite(availableCards);
                    if (newCard != null)
                    {
                        handCardSlots[index].sprite = newCard;

                        if (showDebugInfo)
                            Debug.Log($"ī�� ��ü: {oldSpriteName} �� {newCard.name}");
                    }
                }
            }
        }

        // UIManager���� ȣ���� �� �ִ� ���� �Լ���
        [ContextMenu("Deal New Hand")]
        public void DealNewHand()
        {
            DealCards();
        }

        [ContextMenu("Reset All Cards")]
        public void ResetAllCards()
        {
            ResetCards();
        }
    }
}