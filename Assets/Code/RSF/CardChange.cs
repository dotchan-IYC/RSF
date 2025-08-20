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
            if (_changeNum == 2) return;//두 번 다 바꿨으면 작동 안 함
            if (_gameCicle._level != 2) return;//지금이 바꿀 수 있는 때가 아니면 작동 안 함
            GameObject[] changeCard = _handCard._handCards;
            for (int i = 0; i < changeCard.Length; i++)
            {
                if (_mouseInput._collisionGameObject != null && _mouseInput._collisionGameObject == changeCard[i])//널체크 + 카드 같은 건지 확인
                {
                    changeCard[i].transform.position = Vector3.zero;
                    changeCard[i].SetActive(false);//이전의 카드 비활성화
                    changeCard[i] = _cardPool._cards[5 + _changeNum];//_handCards의 카드 바꾸기
                    changeCard[i].SetActive(true);//현재 카드 활성화
                    changeCard[i].transform.position = new Vector3((i - 0.5f) * 4, -2, 0);
                    _changeNum++;
                    ChangeDecks();
                    break;
                }

            }
            GameObject[] changeCard2 =_communityCards._communityCards;
            for (int i = 0; i < changeCard2.Length; i++)
            {
                if (_mouseInput._collisionGameObject != null && _mouseInput._collisionGameObject == changeCard2[i])//널체크 + 카드 같은 건지 확인
                {
                    changeCard2[i].transform.position = Vector3.zero;
                    changeCard2[i].SetActive(false);//이전의 카드 비활성화
                    changeCard2[i] = _cardPool._cards[5 + _changeNum];//_handCards의 카드 바꾸기
                    changeCard2[i].SetActive(true);//현재 카드 활성화
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
