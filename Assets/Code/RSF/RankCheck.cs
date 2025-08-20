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
    int[] rank = new int[5] { 0, 0, 0, 0, 0 };//각 카드의 순위를 저장할 배열
    int[] suit = new int[5] { 0, 0, 0, 0, 0 };//각 카드의 순위를 저장할 배열
    int pair = 0;
    int RedCount = 0;//레드 카드의 개수를 세는 변수
    int BlackCount = 0;//블랙 카드의 개수를 세는 변수
    bool OnePair = false;//원페어인지 확인하는 변수
    bool TwoPair = false;//투페어인지 확인하는 변수
    bool ThreeOfAKind = false;//쓰리오브어카인드인지 확인하는 변수
    bool Straight = false;//스트레이트인지 확인하는 변수
    bool Flush = false;//플러시인지 확인하는 변수
    bool RedFlush = false;//레드 플러시인지 확인하는 변수
    bool BlackFlush = false;//블랙 플러시인지 확인하는 변수
    bool FullHouse = false;//풀하우스인지 확인하는 변수
    bool FourOfAKind = false;//포카드인지 확인하는 변수
    bool StraightFlush = false;//스트레이트 플러시인지 확인하는 변수
    bool RoyalFlush = false;//로얄 플러시인지 확인하는 변수


    private void Awake()
    {
        _gameCicle.EndGame += HandCheck;//구독
    }
    public void HandCheck()
    {
        ResetHandCheck();//핸드 체크 초기화

        GetCard();//카드 가져오기
                 
        Pair();
        FlushCheck();
        StraightCheck();

        FinalHandRank();

    }

    private void StraightCheck()
    {
        List<int> tempRanks = new List<int>(rank);
        tempRanks.Sort();

        // 중복 제거
        tempRanks = new List<int>(new HashSet<int>(tempRanks));
        tempRanks.Sort();

        if (tempRanks.Count < 5)
            return;

        // 스트레이트 확인: 연속된 숫자 5개
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
         OnePair = false;//원페어인지 확인하는 변수
         TwoPair = false;//투페어인지 확인하는 변수
         ThreeOfAKind = false;//쓰리오브어카인드인지 확인하는 변수
         Straight = false;//스트레이트인지 확인하는 변수
        Flush = false;//플러시인지 확인하는 변수
         RedFlush = false;//레드 플러시인지 확인하는 변수
         BlackFlush = false;//블랙 플러시인지 확인하는 변수
        FullHouse = false;//풀하우스인지 확인하는 변수
       FourOfAKind = false;//포카드인지 확인하는 변수
        StraightFlush = false;//스트레이트 플러시인지 확인하는 변수
        RoyalFlush = false;//로얄 플러시인지 확인하는 변수

        target = true;
    }
    private void FinalHandRank()//족보에 따라 최종적으로 행동
    {
        if (RoyalFlush)
        {
            SetOnlyThisTrue(ref RoyalFlush);
            Debug.Log("로얄 플러시");
        }
        else if (StraightFlush)
        {
            SetOnlyThisTrue(ref StraightFlush);
            Debug.Log("스트레이트 플러시");
        }
        else if (FourOfAKind)
        {
            SetOnlyThisTrue(ref FourOfAKind);
            Debug.Log("포카드");
        }
        else if (FullHouse)
        {
            SetOnlyThisTrue(ref FullHouse);
            Debug.Log("풀하우스");
        }
        else if (Flush)
        {
            SetOnlyThisTrue(ref Flush);
            Debug.Log("플러시");
        }
        else if (Straight)
        {
            SetOnlyThisTrue(ref Straight);
            Debug.Log("스트레이트");
        }
        else if (RedFlush)
        {
            SetOnlyThisTrue(ref RedFlush);
            Debug.Log("레드 플러시");
        }
        else if (BlackFlush)
        {
            SetOnlyThisTrue(ref BlackFlush);
            Debug.Log("블랙 플러시");
        }
        else if (ThreeOfAKind)
        {
            SetOnlyThisTrue(ref ThreeOfAKind);
            Debug.Log("쓰리카드");
        }
        else if (TwoPair)
        {
            SetOnlyThisTrue(ref TwoPair);
            Debug.Log("투페어");
        }
        else if (OnePair)
        {
            SetOnlyThisTrue(ref OnePair);
            Debug.Log("원페어");
        }
        else
        {
            Debug.Log("하이카드");
        }
    }

    private void FlushCheck()
    {
        for (int i = 0; i < suit.Length; i++)
        {
            if(suit[i] == 1 || suit[i] == 3)//레드 카드의 문양은 0, 3
                RedCount++;//레드 카드의 개수를 증가시킨다
            if (suit[i] == 0 || suit[i] == 2)//블랙 카드의 문양은 1, 2
                BlackCount++;//블랙 카드의 개수를 증가시킨다
            if (suitCount.ContainsKey(suit[i]))//같은 문향이 있다면
                suitCount[suit[i]]++;//해당 문양의 개수를 증가시킨다
            else
                suitCount[suit[i]] = 1;//해당 문양이 없다면 새로 추가한다
        }
        if (RedCount == 5)
        {
        RedFlush = true;//레드 플러시인지 확인
      

        }
        else if (BlackCount == 5)
        {
            BlackFlush = true;//블랙 플러시인지 확인
            
        }
        foreach (var entry in suitCount)//모든 문향을 하나씩 확인한다.
        {
            if (entry.Value == 5)//문향의 개수가 5개라면
            {
                Flush = true;//플러쉬 활성화

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
                pair++;//페어의 개수를 세는 변수
            else if (entry.Value == 3)
                ThreeOfAKind = true;//쓰리오브어카인드인지 확인
            else if (entry.Value == 4)
                FourOfAKind = true;//포카드인지 확인
        }
        if (pair == 1 & ThreeOfAKind)
        {
            FullHouse = true;//풀하우스인지 확인
            ThreeOfAKind = false;
        }
        else if (pair == 2)
        {
            TwoPair = true;//투페어인지 확인
        }
        else if (pair == 1)
        {
            OnePair = true;//원페어인지 확인
        }

      

    }

    private void GetCard()
    {

        activeCards.Clear();
        foreach (GameObject card in allCards)//모든 카드들중 보이는 카드들만 게임오브젝트로 저장
        {
            if (card.activeSelf)
            {
                activeCards.Add(card);

            }
        }
        for (int i = 0; i < activeCards.Count; i++)//저장한 게임오브젝트 카드들의 숫자를 하나씩 가져와서 배열에 넣는다.
        {
            Debug.Log(activeCards[i].GetComponent<CardScript>()._rank);//숫자 확인용
            rank[i] = activeCards[i].GetComponent<CardScript>()._rank;//숫자 구하기
            suit[i] = activeCards[i].GetComponent<CardScript>()._suit;//문양 구하기

        }
    }
}
