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

        // 현재 딜링된 카드들을 추적
        private List<int> usedCardIndices = new List<int>();

        private void Start()
        {
            // CardPool 자동 찾기
            if (cardPool == null)
                cardPool = FindAnyObjectByType<CardPool>();

            // AnimationManager 자동 찾기
            if (animationManager == null)
                animationManager = FindAnyObjectByType<CardAnimationManager>();

            // UI 슬롯들 자동 찾기
            AutoFindCardSlots();
        }

        // UI 슬롯들을 자동으로 찾는 함수
        private void AutoFindCardSlots()
        {
            // Hand Card 슬롯들 찾기
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

            // Community Card 슬롯들 찾기
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

        // 카드 딜링 메인 함수
        public void DealCards()
        {
            if (cardPool == null)
            {
                Debug.LogError("CardPool이 설정되지 않았습니다!");
                return;
            }

            // CardPool의 카드 배열에 접근 (public 프로퍼티 필요)
            GameObject[] availableCards = GetCardsFromPool();
            if (availableCards == null || availableCards.Length == 0)
            {
                Debug.LogError("사용 가능한 카드가 없습니다!");
                return;
            }

            // 사용된 카드 인덱스 초기화
            usedCardIndices.Clear();

            if (showDebugInfo)
                Debug.Log("=== 카드 딜링 시작 ===");

            // 핸드 카드 2장 딜링
            for (int i = 0; i < handCardSlots.Length; i++)
            {
                if (handCardSlots[i] != null)
                {
                    Sprite cardSprite = GetRandomCardSprite(availableCards);
                    if (cardSprite != null)
                    {
                        handCardSlots[i].sprite = cardSprite;

                        if (showDebugInfo)
                            Debug.Log($"핸드 카드 {i + 1}: {cardSprite.name}");
                    }
                }
            }

            // 커뮤니티 카드 3장 딜링
            for (int i = 0; i < communityCardSlots.Length; i++)
            {
                if (communityCardSlots[i] != null)
                {
                    Sprite cardSprite = GetRandomCardSprite(availableCards);
                    if (cardSprite != null)
                    {
                        communityCardSlots[i].sprite = cardSprite;

                        if (showDebugInfo)
                            Debug.Log($"커뮤니티 카드 {i + 1}: {cardSprite.name}");
                    }
                }
            }

            // 애니메이션 실행
            if (animationManager != null)
            {
                StartCoroutine(PlayDealAnimation());
            }

            if (showDebugInfo)
                Debug.Log("=== 카드 딜링 완료 ===");
        }

        // CardPool에서 카드 배열 가져오기
        private GameObject[] GetCardsFromPool()
        {
            // 방법 1: public 프로퍼티 사용 (CardPool에 추가 필요)
            try
            {
                var cardsProperty = cardPool.GetType().GetProperty("Cards");
                if (cardsProperty != null)
                {
                    return cardsProperty.GetValue(cardPool) as GameObject[];
                }
            }
            catch { }

            // 방법 2: private 필드 리플렉션으로 접근 (임시)
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
                Debug.LogError($"카드 풀 접근 실패: {e.Message}");
            }

            return null;
        }

        // 랜덤 카드 스프라이트 선택 (중복 방지)
        private Sprite GetRandomCardSprite(GameObject[] availableCards)
        {
            if (usedCardIndices.Count >= availableCards.Length)
            {
                Debug.LogWarning("모든 카드가 사용되었습니다! 카드를 재사용합니다.");
                usedCardIndices.Clear();
            }

            int randomIndex;
            int attempts = 0;

            do
            {
                randomIndex = Random.Range(0, availableCards.Length);
                attempts++;

                // 무한 루프 방지
                if (attempts > 100)
                {
                    Debug.LogWarning("카드 선택에 실패했습니다. 첫 번째 사용 가능한 카드를 사용합니다.");
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

            // GameObject에서 Sprite 추출
            GameObject cardObj = availableCards[randomIndex];
            return ExtractSpriteFromGameObject(cardObj);
        }

        // GameObject에서 Sprite를 추출하는 함수
        private Sprite ExtractSpriteFromGameObject(GameObject cardObj)
        {
            if (cardObj == null)
            {
                Debug.LogError("카드 GameObject가 null입니다!");
                return null;
            }

            // 방법 1: SpriteRenderer에서 sprite 가져오기
            SpriteRenderer sr = cardObj.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                if (showDebugInfo)
                    Debug.Log($"SpriteRenderer에서 스프라이트 추출: {sr.sprite.name}");
                return sr.sprite;
            }

            // 방법 2: CardScript 컴포넌트에서 가져오기
            var cardScript = cardObj.GetComponent<CardScript>();
            if (cardScript != null)
            {
                // 여러 가능한 필드명 시도
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
                            Debug.Log($"CardScript에서 스프라이트 추출: {sprite.name}");
                        return sprite;
                    }
                }

                // 프로퍼티로 시도
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
                            Debug.Log($"CardScript 프로퍼티에서 스프라이트 추출: {sprite.name}");
                        return sprite;
                    }
                }
            }

            // 방법 3: 직접 Sprite인지 확인 (GameObject 이름으로 Assets에서 찾기)
            string cardName = cardObj.name;

            // Assets에서 같은 이름의 Sprite 찾기
            Sprite foundSprite = Resources.Load<Sprite>($"Card/{cardName}");
            if (foundSprite != null)
            {
                if (showDebugInfo)
                    Debug.Log($"Resources에서 스프라이트 로드: {foundSprite.name}");
                return foundSprite;
            }

            // 방법 4: 카드 이름에서 스프라이트 찾기 (일반적인 경로들)
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
                        Debug.Log($"Resources({path})에서 스프라이트 로드: {sprite.name}");
                    return sprite;
                }
            }

            Debug.LogError($"GameObject {cardName}에서 Sprite를 찾을 수 없습니다! " +
                          "SpriteRenderer나 CardScript가 있는지 확인하세요.");
            return null;
        }

        // 딜링 애니메이션 실행
        private System.Collections.IEnumerator PlayDealAnimation()
        {
            // 각 카드를 순서대로 애니메이션
            yield return new WaitForSeconds(0.1f);

            // 핸드 카드 애니메이션
            for (int i = 0; i < handCardSlots.Length; i++)
            {
                if (handCardSlots[i] != null && animationManager != null)
                {
                    // 카드 뒤집기 애니메이션 등 실행 가능
                    yield return new WaitForSeconds(animationManager.dealDelay);
                }
            }

            // 커뮤니티 카드 애니메이션
            for (int i = 0; i < communityCardSlots.Length; i++)
            {
                if (communityCardSlots[i] != null && animationManager != null)
                {
                    yield return new WaitForSeconds(animationManager.dealDelay);
                }
            }
        }

        // 카드 초기화 (RESET 버튼용)
        public void ResetCards()
        {
            // 모든 카드 슬롯 초기화
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
                Debug.Log("카드가 초기화되었습니다.");
        }

        // 특정 카드 교체 (CHANGE CARDS 버튼용)
        public void ChangeCards(int[] cardIndices)
        {
            GameObject[] availableCards = GetCardsFromPool();
            if (availableCards == null) return;

            foreach (int index in cardIndices)
            {
                if (index >= 0 && index < handCardSlots.Length && handCardSlots[index] != null)
                {
                    // 사용된 카드에서 제거
                    string oldSpriteName = handCardSlots[index].sprite?.name;

                    // 새 카드 할당
                    Sprite newCard = GetRandomCardSprite(availableCards);
                    if (newCard != null)
                    {
                        handCardSlots[index].sprite = newCard;

                        if (showDebugInfo)
                            Debug.Log($"카드 교체: {oldSpriteName} → {newCard.name}");
                    }
                }
            }
        }

        // UIManager에서 호출할 수 있는 공개 함수들
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