




//���� ����ٰ� ���� �� ��Ʈ��� �ؼ� �ϴ��� �ּ����θ� ���ܵ׾� ������ ��� X









//using Code.RSF;
//using System;
//using UnityEngine;

//namespace Code.RSF
//{
//    public class HandRankings : MonoBehaviour
//    {
//        public string _handRanking;//���� ����
//        HandCard _handCard;
//        CommunityCards _communityCards;
//        CardChange _cardChange;
//        private void Start()
//        {
//            _handCard = GetComponent<HandCard>();
//            _communityCards = GetComponent<CommunityCards>();
//            _cardChange = GetComponent<CardChange>();
//            _cardChange.ChangeHandRanking += UpdateHandRanking; �̰� ���� ���� Ŭ������ �� ���� �ٲ�� �� �޼ҵ� ���� �� ����
//        }

//        private void UpdateHandRanking()  ���� ī�带 �� �� ������ �ڿ� ������ ������ �� �����ƺ����� ���� �غ�
//        {
//            GameObject[] cards = new GameObject[5];
//            for (int i = 0; i < 3; i++)
//            {
//                cards[i + 2] = _communityCards._communityCards[i];//���� ī�带 0 + 2��, 1 + 2��, 2 + 2�� �ڸ��� ����
//                if (i != 2) cards[i] = _handCard._handCards[i];//�� �и� 0, 1�� �ڸ��� ����. i�� 2�� �۵����� ����.
//            }

//            GameObject[] cards2 = new GameObject[5];
//            for (int i = 0; i < 5; i++)//�������� ����
//            {
//                for (int j = 4; j >= i; j--)
//                {
//                    if (cards2[i] == null)
//                    {
//                        cards2[i] = cards[j];
//                    }
//                    else
//                    {
//                        if (cards2[i]. < cards[j])
//                        {

//                        }
//                    }
//                }
//            }
//        }
//    }
//}
