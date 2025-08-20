using System;
using UnityEngine;

namespace Code.RSF
{
    public class GameCicle : MonoBehaviour
    {
        public event Action DrawCards;//ó�� ī�� �� �� �� �� �ִ� �ܰ� + �����ϴ� �ܰ�
        public event Action ShowEveryCard;//������ ī�尡 �巯����, ī�带 �ٲ� �� �ִ� �ܰ�
        public event Action EndGame;//������ ����ǰ� ������ ����ϴ� �ܰ�
        public event Action ResetGame;//����

        public int _level;//���� ������ �ܰ�

        public MouseInput  _mouseInput;


        private void Start()
        {
            //_mouseInput.LeftMouseClick += EnterResetTurn;
            NewGameManager.Instance.OnPressedDrawButton += EnterDrawTrun;
            NewGameManager.Instance.OnPressedBettingButton += EnterChangeTurn;
            NewGameManager.Instance.OnPressedDealButton += EnterCalculateTurn;
            NewGameManager.Instance.OnPressedResetButton += EnterResetTurn;
        }

        void EnterDrawTrun()
        {
            if (_level != 0) return;
            DrawCards?.Invoke();
            print("��ο� �ܰ�");
            _level++;
        }
        private void EnterChangeTurn()
        {
            if (_level != 1) return;
            ShowEveryCard?.Invoke();
            print("���� �巯���� + �ٲٴ� �ܰ�");
            _level++;
        }
        void EnterCalculateTurn()//Įŧ����Ʈ ��Ÿ�� ��ĥ �ð� x
        {
            if (_level != 2) return;
            EndGame?.Invoke();
            print("���� ��� + �Ա� �ܰ�");
            _level++;
        }
        void EnterResetTurn()
        {
            if (_level != 3) return;
            ResetGame?.Invoke();
            print("����");
            _level = 0;
        }
    }
}
