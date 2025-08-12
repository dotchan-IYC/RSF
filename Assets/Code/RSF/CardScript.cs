using UnityEngine;

namespace Code.RSF
{
    public class CardScript : MonoBehaviour
    {
        [Header("Card Data for RSF System")]
        public int cardValue;     // 1=A, 2-10=����, 11=J, 12=Q, 13=K
        public CardSuit cardSuit; // Hearts, Diamonds, Clubs, Spades
        public int _rank;//����(��, ��, ŷ�� ���� 11, 12, 13)
        public int _suit;//����(�����̵�, ���̾Ƹ��, Ŭ��, ��Ʈ ������ ���� 0, 1, 2, 3)

        public void SetCardData(int value, CardSuit suit)
        {
            cardValue = value;
            cardSuit = suit;
        }
        
    }
}
