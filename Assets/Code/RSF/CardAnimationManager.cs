using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardAnimationManager : MonoBehaviour
{
    [Header("Animation Settings")]
    public float flipDuration = 0.5f;
    public float dealDelay = 0.2f;
    public float moveSpeed = 2f;
    public AnimationCurve flipCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Card Positions")]
    public Transform[] handCardPositions = new Transform[2];
    public Transform[] communityCardPositions = new Transform[3];
    public Transform deckPosition;

    [Header("Effects")]
    public GameObject cardDealEffect;
    public GameObject cardFlipEffect;
    public AudioSource audioSource;
    public AudioClip dealSound;
    public AudioClip flipSound;
    public AudioClip shuffleSound;

    public static CardAnimationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FlipCard(GameObject card, bool showFront, System.Action onComplete = null)
    {
        StartCoroutine(FlipCardCoroutine(card, showFront, onComplete));
    }

    private IEnumerator FlipCardCoroutine(GameObject card, bool showFront, System.Action onComplete)
    {
        Transform cardTransform = card.transform;
        Vector3 originalScale = cardTransform.localScale;

        PlaySound(flipSound);

        if (cardFlipEffect != null)
        {
            Instantiate(cardFlipEffect, cardTransform.position, Quaternion.identity);
        }

        float elapsedTime = 0f;
        while (elapsedTime < flipDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (flipDuration / 2);
            float curveValue = flipCurve.Evaluate(progress);

            Vector3 scale = originalScale;
            scale.x = Mathf.Lerp(originalScale.x, 0, curveValue);
            cardTransform.localScale = scale;

            yield return null;
        }

        AnimatedCard animatedCard = card.GetComponent<AnimatedCard>();
        if (animatedCard != null)
        {
            animatedCard.SetCardFace(showFront);
        }

        elapsedTime = 0f;
        while (elapsedTime < flipDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (flipDuration / 2);
            float curveValue = flipCurve.Evaluate(progress);

            Vector3 scale = originalScale;
            scale.x = Mathf.Lerp(0, originalScale.x, curveValue);
            cardTransform.localScale = scale;

            yield return null;
        }

        cardTransform.localScale = originalScale;
        onComplete?.Invoke();
    }

    public void DealCards(List<GameObject> cards, Transform[] targetPositions, System.Action onComplete = null)
    {
        StartCoroutine(DealCardsCoroutine(cards, targetPositions, onComplete));
    }

    private IEnumerator DealCardsCoroutine(List<GameObject> cards, Transform[] targetPositions, System.Action onComplete)
    {
        PlaySound(shuffleSound);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < cards.Count && i < targetPositions.Length; i++)
        {
            GameObject card = cards[i];
            Transform targetPos = targetPositions[i];

            card.transform.position = deckPosition.position;
            card.transform.rotation = deckPosition.rotation;
            card.SetActive(true);

            PlaySound(dealSound);

            if (cardDealEffect != null)
            {
                Instantiate(cardDealEffect, deckPosition.position, Quaternion.identity);
            }

            yield return StartCoroutine(MoveCardToPosition(card, targetPos.position, targetPos.rotation));
            yield return new WaitForSeconds(dealDelay);
        }

        onComplete?.Invoke();
    }

    private IEnumerator MoveCardToPosition(GameObject card, Vector3 targetPosition, Quaternion targetRotation)
    {
        Transform cardTransform = card.transform;
        Vector3 startPosition = cardTransform.position;
        Quaternion startRotation = cardTransform.rotation;

        float elapsedTime = 0f;
        float moveTime = Vector3.Distance(startPosition, targetPosition) / moveSpeed;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveTime;
            float curveValue = flipCurve.Evaluate(progress);

            cardTransform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);
            cardTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, curveValue);

            yield return null;
        }

        cardTransform.position = targetPosition;
        cardTransform.rotation = targetRotation;
    }

    public void CelebrateCards(List<GameObject> winningCards)
    {
        StartCoroutine(CelebrateCardsCoroutine(winningCards));
    }

    private IEnumerator CelebrateCardsCoroutine(List<GameObject> winningCards)
    {
        float bounceHeight = 0.5f;
        float bounceSpeed = 4f;

        List<Vector3> originalPositions = new List<Vector3>();
        foreach (GameObject card in winningCards)
        {
            originalPositions.Add(card.transform.position);
        }

        float elapsedTime = 0f;
        float celebrationDuration = 2f;

        while (elapsedTime < celebrationDuration)
        {
            elapsedTime += Time.deltaTime;

            for (int i = 0; i < winningCards.Count; i++)
            {
                Vector3 originalPos = originalPositions[i];
                float bounce = Mathf.Sin((elapsedTime + i * 0.2f) * bounceSpeed) * bounceHeight;
                winningCards[i].transform.position = originalPos + Vector3.up * bounce;
            }

            yield return null;
        }

        for (int i = 0; i < winningCards.Count; i++)
        {
            winningCards[i].transform.position = originalPositions[i];
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}