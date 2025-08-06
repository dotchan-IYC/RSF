




//조금 만들다가 족보 네 파트라고 해서 일단은 주석으로만 남겨뒀어 지워도 상관 X









//using Code.RSF;
//using System;
//using UnityEngine;

//namespace Code.RSF
//{
//    public class HandRankings : MonoBehaviour
//    {
//        public string _handRanking;//현재 족보
//        HandCard _handCard;
//        CommunityCards _communityCards;
//        CardChange _cardChange;
//        private void Start()
//        {
//            _handCard = GetComponent<HandCard>();
//            _communityCards = GetComponent<CommunityCards>();
//            _cardChange = GetComponent<CardChange>();
//            _cardChange.ChangeHandRanking += UpdateHandRanking; 이거 문장 쓰면 클릭했을 때 족보 바뀌는 거 메소드 만들 수 있음
//        }

//        private void UpdateHandRanking()  나는 카드를 한 번 정렬한 뒤에 족보를 따지는 게 괜찮아보여서 이케 해봄
//        {
//            GameObject[] cards = new GameObject[5];
//            for (int i = 0; i < 3; i++)
//            {
//                cards[i + 2] = _communityCards._communityCards[i];//공유 카드를 0 + 2번, 1 + 2번, 2 + 2번 자리에 넣음
//                if (i != 2) cards[i] = _handCard._handCards[i];//손 패를 0, 1번 자리에 넣음. i가 2면 작동하지 않음.
//            }

//            GameObject[] cards2 = new GameObject[5];
//            for (int i = 0; i < 5; i++)//오름차순 정렬
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
