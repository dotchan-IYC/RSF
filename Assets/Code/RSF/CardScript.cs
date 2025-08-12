using UnityEngine;

namespace Code.RSF
{
    public class CardScript : MonoBehaviour
    {
        [Header("Card Data for RSF System")]
        public int cardValue;     // 1=A, 2-10=숫자, 11=J, 12=Q, 13=K
        public CardSuit cardSuit; // Hearts, Diamonds, Clubs, Spades
        public int _rank;//숫자(잭, 퀸, 킹은 각각 11, 12, 13)
        public int _suit;//문양(스페이드, 다이아몬드, 클럽, 하트 순으로 각각 0, 1, 2, 3)

        public void SetCardData(int value, CardSuit suit)
        {
            cardValue = value;
            cardSuit = suit;
        }
        
    }
}
