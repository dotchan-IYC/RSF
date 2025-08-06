using System;
using UnityEngine;

namespace Code.RSF
{
    public class GameCicle : MonoBehaviour
    {
        public event Action DrawCards;//ó�� ī�� �� �� �� �� �ִ� �ܰ�
        public event Action Betting;//�����ϴ� �ܰ�
        public event Action ShowEveryCard;//������ ī�尡 �巯����, ī�带 �ٲ� �� �ִ� �ܰ�
        public event Action EndGame;//������ ����ǰ� ������ ����ϴ� �ܰ�
        public event Action ResetGame;//����

        public int _level;//���� ������ �ܰ�

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))//���� �����̽��ٸ� ������ �۵��ȴٴ� �ּ�
            {
                switch (_level)//����ġ�� + �̺�Ʈ�� �� �ý��� �����ϱ�
                {
                    case 0:
                        DrawCards?.Invoke();
                        print("��ο� �ܰ�");
                        _level++;
                        break;
                    case 1:
                        Betting?.Invoke();
                        print("���� �ܰ�");
                        _level++;
                        break;
                    case 2:
                        ShowEveryCard?.Invoke();
                        print("���� �巯���� + �ٲٴ� �ܰ�");
                        _level++;
                        break;
                    case 3:
                        EndGame?.Invoke();
                        print("���� ��� + �Ա� �ܰ�");
                        _level++;
                        break;
                    case 4:
                        ResetGame?.Invoke();
                        print("����");
                        _level = 0;
                        break;
                }
            }
        }
    }
}
