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

        private void DrawCommunityCards()//DrawCards 이벤트 호출되면 카드 풀 위( + 2)에서부터 2개 꺼내서 배열로 만듦
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