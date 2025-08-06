using System;
using UnityEngine;

namespace Code.RSF
{
    public class GameCicle : MonoBehaviour
    {
        public event Action DrawCards;//처음 카드 두 장 볼 수 있는 단계
        public event Action Betting;//베팅하는 단계
        public event Action ShowEveryCard;//나머지 카드가 드러나고, 카드를 바꿀 수 있는 단계
        public event Action EndGame;//게임이 종료되고 족보를 계산하는 단계
        public event Action ResetGame;//리셋

        public int _level;//현재 게임의 단계

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))//대충 스페이스바를 누르면 작동된다는 주석
            {
                switch (_level)//스위치문 + 이벤트로 턴 시스템 날먹하기
                {
                    case 0:
                        DrawCards?.Invoke();
                        print("드로우 단계");
                        _level++;
                        break;
                    case 1:
                        Betting?.Invoke();
                        print("베팅 단계");
                        _level++;
                        break;
                    case 2:
                        ShowEveryCard?.Invoke();
                        print("전부 드러내는 + 바꾸는 단계");
                        _level++;
                        break;
                    case 3:
                        EndGame?.Invoke();
                        print("족보 계산 + 입금 단계");
                        _level++;
                        break;
                    case 4:
                        ResetGame?.Invoke();
                        print("리셋");
                        _level = 0;
                        break;
                }
            }
        }
    }
}
