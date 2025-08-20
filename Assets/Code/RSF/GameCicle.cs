using System;
using UnityEngine;

namespace Code.RSF
{
    public class GameCicle : MonoBehaviour
    {
        public event Action DrawCards;//처음 카드 두 장 볼 수 있는 단계 + 베팅하는 단계
        public event Action ShowEveryCard;//나머지 카드가 드러나고, 카드를 바꿀 수 있는 단계
        public event Action EndGame;//게임이 종료되고 족보를 계산하는 단계
        public event Action ResetGame;//리셋

        public int _level;//현재 게임의 단계

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
            print("드로우 단계");
            _level++;
        }
        private void EnterChangeTurn()
        {
            if (_level != 1) return;
            ShowEveryCard?.Invoke();
            print("전부 드러내는 + 바꾸는 단계");
            _level++;
        }
        void EnterCalculateTurn()//칼큘레이트 오타임 고칠 시간 x
        {
            if (_level != 2) return;
            EndGame?.Invoke();
            print("족보 계산 + 입금 단계");
            _level++;
        }
        void EnterResetTurn()
        {
            if (_level != 3) return;
            ResetGame?.Invoke();
            print("리셋");
            _level = 0;
        }
    }
}
