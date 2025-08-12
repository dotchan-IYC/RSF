using UnityEngine;

public class AnimatedCard : MonoBehaviour
{
    [Header("Card Faces")]
    public GameObject frontFace;
    public GameObject backFace;

    [Header("Card Data")]
    public int cardValue;
    public CardSuit cardSuit;

    private bool isCardFaceUp = false;
    private bool isSelected = false;

    private void Start()
    {
        SetCardFace(false);
    }

    public void SetCardFace(bool showFront)
    {
        isCardFaceUp = showFront;

        if (frontFace != null)
            frontFace.SetActive(showFront);
        if (backFace != null)
            backFace.SetActive(!showFront);
    }

    public void FlipCard()
    {
        if (CardAnimationManager.Instance != null)
        {
            CardAnimationManager.Instance.FlipCard(gameObject, !isCardFaceUp);
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance != null && GameManager.Instance.isRoundInProgress)
        {
            ToggleSelection();
        }
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
        if (CardAnimationManager.Instance != null)
        {
            // 하이라이트 효과는 추후 구현
            Debug.Log("Card selected: " + isSelected);
        }
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public bool IsFaceUp()
    {
        return isCardFaceUp;
    }
}