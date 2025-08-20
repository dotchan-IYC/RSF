using Code.RSF;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RankCheck : MonoBehaviour
{
    Dictionary<int, int> rankCount = new Dictionary<int, int>();
    Dictionary<int, int> suitCount = new Dictionary<int, int>();
    List<GameObject> activeCards = new List<GameObject>();
    public List<GameObject> allCards = new List<GameObject>();
    [SerializeField] private GameCicle _gameCicle;
    int[] rank = new int[5] { 0, 0, 0, 0, 0 };//�� ī���� ������ ������ �迭
    int[] suit = new int[5] { 0, 0, 0, 0, 0 };//�� ī���� ������ ������ �迭
    int pair = 0;
    int RedCount = 0;//���� ī���� ������ ���� ����
    int BlackCount = 0;//�� ī���� ������ ���� ����
    bool OnePair = false;//��������� Ȯ���ϴ� ����
    bool TwoPair = false;//��������� Ȯ���ϴ� ����
    bool ThreeOfAKind = false;//���������ī�ε����� Ȯ���ϴ� ����
    bool Straight = false;//��Ʈ����Ʈ���� Ȯ���ϴ� ����
    bool Flush = false;//�÷������� Ȯ���ϴ� ����
    bool RedFlush = false;//���� �÷������� Ȯ���ϴ� ����
    bool BlackFlush = false;//�� �÷������� Ȯ���ϴ� ����
    bool FullHouse = false;//Ǯ�Ͽ콺���� Ȯ���ϴ� ����
    bool FourOfAKind = false;//��ī������ Ȯ���ϴ� ����
    bool StraightFlush = false;//��Ʈ����Ʈ �÷������� Ȯ���ϴ� ����
    bool RoyalFlush = false;//�ξ� �÷������� Ȯ���ϴ� ����


    private void Awake()
    {
        _gameCicle.EndGame += HandCheck;//����
    }
    public void HandCheck()
    {
        ResetHandCheck();//�ڵ� üũ �ʱ�ȭ

        GetCard();//ī�� ��������
                 
        Pair();
        FlushCheck();
        StraightCheck();

        FinalHandRank();

    }

    private void StraightCheck()
    {
        List<int> tempRanks = new List<int>(rank);
        tempRanks.Sort();

        // �ߺ� ����
        tempRanks = new List<int>(new HashSet<int>(tempRanks));
        tempRanks.Sort();

        if (tempRanks.Count < 5)
            return;

        // ��Ʈ����Ʈ Ȯ��: ���ӵ� ���� 5��
        bool isStraight = true;
        for (int i = 1; i < tempRanks.Count; i++)
        {
            if (tempRanks[i] - tempRanks[i - 1] != 1)
            {
                isStraight = false;
                break;
            }
        }

        if (isStraight)
        {
            Straight = true;
            return;
        }

    }
    void SetOnlyThisTrue(ref bool target)
    {
         OnePair = false;//��������� Ȯ���ϴ� ����
         TwoPair = false;//��������� Ȯ���ϴ� ����
         ThreeOfAKind = false;//���������ī�ε����� Ȯ���ϴ� ����
         Straight = false;//��Ʈ����Ʈ���� Ȯ���ϴ� ����
        Flush = false;//�÷������� Ȯ���ϴ� ����
         RedFlush = false;//���� �÷������� Ȯ���ϴ� ����
         BlackFlush = false;//�� �÷������� Ȯ���ϴ� ����
        FullHouse = false;//Ǯ�Ͽ콺���� Ȯ���ϴ� ����
       FourOfAKind = false;//��ī������ Ȯ���ϴ� ����
        StraightFlush = false;//��Ʈ����Ʈ �÷������� Ȯ���ϴ� ����
        RoyalFlush = false;//�ξ� �÷������� Ȯ���ϴ� ����

        target = true;
    }
    private void FinalHandRank()//������ ���� ���������� �ൿ
    {
        if (RoyalFlush)
        {
            SetOnlyThisTrue(ref RoyalFlush);
            Debug.Log("�ξ� �÷���");
        }
        else if (StraightFlush)
        {
            SetOnlyThisTrue(ref StraightFlush);
            Debug.Log("��Ʈ����Ʈ �÷���");
        }
        else if (FourOfAKind)
        {
            SetOnlyThisTrue(ref FourOfAKind);
            Debug.Log("��ī��");
        }
        else if (FullHouse)
        {
            SetOnlyThisTrue(ref FullHouse);
            Debug.Log("Ǯ�Ͽ콺");
        }
        else if (Flush)
        {
            SetOnlyThisTrue(ref Flush);
            Debug.Log("�÷���");
        }
        else if (Straight)
        {
            SetOnlyThisTrue(ref Straight);
            Debug.Log("��Ʈ����Ʈ");
        }
        else if (RedFlush)
        {
            SetOnlyThisTrue(ref RedFlush);
            Debug.Log("���� �÷���");
        }
        else if (BlackFlush)
        {
            SetOnlyThisTrue(ref BlackFlush);
            Debug.Log("�� �÷���");
        }
        else if (ThreeOfAKind)
        {
            SetOnlyThisTrue(ref ThreeOfAKind);
            Debug.Log("����ī��");
        }
        else if (TwoPair)
        {
            SetOnlyThisTrue(ref TwoPair);
            Debug.Log("�����");
        }
        else if (OnePair)
        {
            SetOnlyThisTrue(ref OnePair);
            Debug.Log("�����");
        }
        else
        {
            Debug.Log("����ī��");
        }
    }

    private void FlushCheck()
    {
        for (int i = 0; i < suit.Length; i++)
        {
            if(suit[i] == 1 || suit[i] == 3)//���� ī���� ������ 0, 3
                RedCount++;//���� ī���� ������ ������Ų��
            if (suit[i] == 0 || suit[i] == 2)//�� ī���� ������ 1, 2
                BlackCount++;//�� ī���� ������ ������Ų��
            if (suitCount.ContainsKey(suit[i]))//���� ������ �ִٸ�
                suitCount[suit[i]]++;//�ش� ������ ������ ������Ų��
            else
                suitCount[suit[i]] = 1;//�ش� ������ ���ٸ� ���� �߰��Ѵ�
        }
        if (RedCount == 5)
        {
        RedFlush = true;//���� �÷������� Ȯ��
      

        }
        else if (BlackCount == 5)
        {
            BlackFlush = true;//�� �÷������� Ȯ��
            
        }
        foreach (var entry in suitCount)//��� ������ �ϳ��� Ȯ���Ѵ�.
        {
            if (entry.Value == 5)//������ ������ 5�����
            {
                Flush = true;//�÷��� Ȱ��ȭ

            }
        }
    }
    private void ResetHandCheck()
    {
        RedCount = 0;
        BlackCount = 0;
        RedFlush = false;
        BlackFlush = false;
        rankCount.Clear();
        pair = 0;
        OnePair = false;
        TwoPair = false;
        ThreeOfAKind = false;
        Straight = false;
        Flush = false;
        FullHouse = false;
        FourOfAKind = false;
        StraightFlush = false;
        RoyalFlush = false;
    }
    private void Pair()
    {
        for (int i = 0; i < rank.Length; i++)
        {
            if (rankCount.ContainsKey(rank[i]))
                rankCount[rank[i]]++;
            else
                rankCount[rank[i]] = 1;

        }
        foreach (var entry in rankCount)
        {
            if (entry.Value == 2)
                pair++;//����� ������ ���� ����
            else if (entry.Value == 3)
                ThreeOfAKind = true;//���������ī�ε����� Ȯ��
            else if (entry.Value == 4)
                FourOfAKind = true;//��ī������ Ȯ��
        }
        if (pair == 1 & ThreeOfAKind)
        {
            FullHouse = true;//Ǯ�Ͽ콺���� Ȯ��
            ThreeOfAKind = false;
        }
        else if (pair == 2)
        {
            TwoPair = true;//��������� Ȯ��
        }
        else if (pair == 1)
        {
            OnePair = true;//��������� Ȯ��
        }

      

    }

    private void GetCard()
    {

        activeCards.Clear();
        foreach (GameObject card in allCards)//��� ī����� ���̴� ī��鸸 ���ӿ�����Ʈ�� ����
        {
            if (card.activeSelf)
            {
                activeCards.Add(card);

            }
        }
        for (int i = 0; i < activeCards.Count; i++)//������ ���ӿ�����Ʈ ī����� ���ڸ� �ϳ��� �����ͼ� �迭�� �ִ´�.
        {
            Debug.Log(activeCards[i].GetComponent<CardScript>()._rank);//���� Ȯ�ο�
            rank[i] = activeCards[i].GetComponent<CardScript>()._rank;//���� ���ϱ�
            suit[i] = activeCards[i].GetComponent<CardScript>()._suit;//���� ���ϱ�

        }
    }
}
