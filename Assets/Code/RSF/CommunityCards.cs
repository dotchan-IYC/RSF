using System;
using UnityEngine;

namespace Code.RSF
{
    public class CommunityCards : MonoBehaviour
    {
        CardPool cardPool;
        public GameObject[] _communityCards = new GameObject[3];
        private void Start()
        {
            cardPool = GetComponent<CardPool>();
            cardPool.DrawCards += DrawCommunityCards;
        }

        private void DrawCommunityCards()//DrawCards �̺�Ʈ ȣ��Ǹ� ī�� Ǯ ��( + 2)�������� 2�� ������ �迭�� ����
        {
            for (int i = 0; i < 3; i++)
            {
                _communityCards[i] = cardPool._cards[i + 2];
                _communityCards[i].SetActive(true);
                _communityCards[i].transform.position = new Vector3((i - 1) * 2, 2, 0);
            }
        }
    }
}