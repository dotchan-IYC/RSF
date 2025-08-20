using System;
using UnityEngine;

namespace Code.RSF
{
    public class CardChange : MonoBehaviour
    {
        [SerializeField] GameCicle _gameCicle;
        CardPool _cardPool;
        HandCard _handCard;
        CommunityCards _communityCards;
        MouseInput _mouseInput;
        [SerializeField] int _changeNum = 0;
        public event Action ChangeHandRanking;

        private void Start()
        {
            _cardPool = GetComponent<CardPool>();
            _handCard = GetComponent<HandCard>();
            _communityCards = GetComponent<CommunityCards>();
            _mouseInput = GetComponent<MouseInput>();
            _mouseInput.LeftMouseClick += ChangeCard;
            _gameCicle.ResetGame += ResetCanChangeNum;
        }

        private void ResetCanChangeNum()
        {
            _changeNum = 0;
            ChangeDecks();
        }

        private void ChangeCard()
        {
            if (_changeNum == 2) return;//�� �� �� �ٲ����� �۵� �� ��
            if (_gameCicle._level != 2) return;//������ �ٲ� �� �ִ� ���� �ƴϸ� �۵� �� ��
            GameObject[] changeCard = _handCard._handCards;
            for (int i = 0; i < changeCard.Length; i++)
            {
                if (_mouseInput._collisionGameObject != null && _mouseInput._collisionGameObject == changeCard[i])//��üũ + ī�� ���� ���� Ȯ��
                {
                    changeCard[i].transform.position = Vector3.zero;
                    changeCard[i].SetActive(false);//������ ī�� ��Ȱ��ȭ
                    changeCard[i] = _cardPool._cards[5 + _changeNum];//_handCards�� ī�� �ٲٱ�
                    changeCard[i].SetActive(true);//���� ī�� Ȱ��ȭ
                    changeCard[i].transform.position = new Vector3((i - 0.5f) * 4, -2, 0);
                    _changeNum++;
                    ChangeDecks();
                    break;
                }

            }
            GameObject[] changeCard2 =_communityCards._communityCards;
            for (int i = 0; i < changeCard2.Length; i++)
            {
                if (_mouseInput._collisionGameObject != null && _mouseInput._collisionGameObject == changeCard2[i])//��üũ + ī�� ���� ���� Ȯ��
                {
                    changeCard2[i].transform.position = Vector3.zero;
                    changeCard2[i].SetActive(false);//������ ī�� ��Ȱ��ȭ
                    changeCard2[i] = _cardPool._cards[5 + _changeNum];//_handCards�� ī�� �ٲٱ�
                    changeCard2[i].SetActive(true);//���� ī�� Ȱ��ȭ
                    changeCard2[i].transform.position = new Vector3((i - 1f) * 2, 2, 0);
                    _changeNum++;
                    ChangeDecks();
                    break;
                }

            }
        }

        void ChangeDecks()
        {
            ChangeHandRanking?.Invoke();
        }
    }
}
