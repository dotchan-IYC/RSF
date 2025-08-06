using System;
using UnityEngine;

namespace Code.RSF
{
    public class CardPool : MonoBehaviour
    {
        [SerializeField] GameCicle _gameCicle;

        public GameObject[] _cards;//Ǯ �ȿ� �ִ� ī���
        GameObject[] _newPool;//���� ���� Ǯ
        bool _isDraw;//��ο� �ߴ°�?
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
            for (int i = 0; i < n; i++)//�������� ī�� Ǯ ���� ��ũ��Ʈ�ε� ���� ���� �Ŷ� �� ��ȿ������
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

        void ResetCard()//�ǵ�����
        {
            for (int i = 0; i < 7; i++)
            {
                _cards[i].SetActive(false);
                _cards[i].transform.position = Vector3.zero;

            }
        }
    }
}
