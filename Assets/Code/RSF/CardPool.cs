using System;
using UnityEngine;

namespace Code.RSF
{
    public class CardPool : MonoBehaviour
    {
        [SerializeField] GameCicle _gameCicle;

        public GameObject[] _cards;//풀 안에 있는 카드들
        GameObject[] _newPool;//새로 섞은 풀
        bool _isDraw;//드로우 했는가?
        public event Action DrawCards;

        private void Awake()
        {
            _newPool = new GameObject[_cards.Length];
            _gameCicle.ResetGame += ResetCard;
            DrawCards += Shuffle;
            _gameCicle.DrawCards += ADrawCards;
        }

        private void ADrawCards()
        {
            DrawCards.Invoke();
        }

        public void Shuffle()
        {
            int n = _cards.Length;
            for (int i = 0; i < n; i++)//랜덤으로 카드 풀 섞는 스크립트인데 대충 만든 거라 좀 비효율적임
            {
                
                int rnd = UnityEngine.Random.Range(0, n);
                if (_newPool[rnd] == null)
                {
                    _newPool[rnd] = _cards[i];
                }
                else
                {
                    //print(rnd);
                    i -= 1;
                    continue;
                }
            }

            _cards = _newPool;
            _newPool = new GameObject[_cards.Length];
        }

        void ResetCard()//되돌리기
        {
            for (int i = 0; i < 7; i++)
            {
                _cards[i].SetActive(false);
                _cards[i].transform.position = Vector3.zero;

            }
        }
    }
}
