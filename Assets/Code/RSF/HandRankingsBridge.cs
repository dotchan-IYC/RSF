using UnityEngine;
using Code.RSF;

public class HandRankingsBridge : MonoBehaviour
{
    [Header("References")]
    public Code.RSF.HandRankings handRankings;

    private void Start()
    {
        // 같은 GameObject에 있는 HandRankings 컴포넌트 자동 찾기
        if (handRankings == null)
        {
            handRankings = GetComponent<Code.RSF.HandRankings>();
        }

        if (handRankings == null)
        {
            Debug.LogError("HandRankings component not found! Please add HandRankings component to this GameObject.");
        }
    }

    // 기존 HandRankings의 결과를 새로운 HandRankType으로 변환
    public HandRankType ConvertToHandRankType()
    {
        if (handRankings == null)
        {
            Debug.LogWarning("HandRankings component is null!");
            return HandRankType.None;
        }

        // 현재 handRankings에서 족보 타입 직접 가져오기
        return handRankings.GetCurrentHandRankType();
    }

    // 족보 평가 및 GameManager에 결과 전달
    public void EvaluateHand()
    {
        if (handRankings == null)
        {
            Debug.LogError("HandRankings is null! Cannot evaluate hand.");
            return;
        }

        // HandRankings에서 족보 체크 실행
        handRankings.CheckHandRanking();

        // 결과 가져오기
        HandRankType result = handRankings.GetCurrentHandRankType();

        Debug.Log("Hand evaluated: " + result + " (" + handRankings.GetCurrentHandRankingName() + ")");

        // GameManager에 결과 전달 (HandRankings에서 이미 자동으로 하지만, 수동으로도 가능)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ProcessHandResult(result);
        }
    }

    // 현재 족보 이름 가져오기
    public string GetCurrentHandRankingName()
    {
        if (handRankings != null)
        {
            return handRankings.GetCurrentHandRankingName();
        }
        return "Unknown";
    }

    // 현재 족보 타입 가져오기
    public HandRankType GetCurrentHandRankType()
    {
        if (handRankings != null)
        {
            return handRankings.GetCurrentHandRankType();
        }
        return HandRankType.None;
    }

    // 수동으로 족보 체크 (디버그용)
    [ContextMenu("Check Hand Ranking")]
    public void ManualCheckHandRanking()
    {
        EvaluateHand();
    }
}