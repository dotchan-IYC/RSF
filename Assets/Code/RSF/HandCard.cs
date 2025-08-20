using UnityEngine;

namespace Code.RSF
{
    public class HandCard : MonoBehaviour
    {
        CardPool cardPool;
        public GameObject[] _handCards = new GameObject[2];
        private void Start()
        {
            cardPool = GetComponent<CardPool>();
            cardPool.DrawCards += DrawHandCards;
        }

        private void DrawHandCards()//DrawCards �̺�Ʈ ȣ��Ǹ� ī�� Ǯ ���������� 2�� ������ �迭�� ����
        {
            for (int i = 0; i < 2; i++)
            {
                _handCards[i] = cardPool._cards[i];
                _handCards[i].SetActive(true);
                _handCards[i].transform.position = new Vector3((i - 0.5f) * 4, -2, 0);
            }
        }


    }
}
