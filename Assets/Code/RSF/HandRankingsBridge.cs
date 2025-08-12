using UnityEngine;
using Code.RSF;

public class HandRankingsBridge : MonoBehaviour
{
    [Header("References")]
    public Code.RSF.HandRankings handRankings;

    private void Start()
    {
        // ���� GameObject�� �ִ� HandRankings ������Ʈ �ڵ� ã��
        if (handRankings == null)
        {
            handRankings = GetComponent<Code.RSF.HandRankings>();
        }

        if (handRankings == null)
        {
            Debug.LogError("HandRankings component not found! Please add HandRankings component to this GameObject.");
        }
    }

    // ���� HandRankings�� ����� ���ο� HandRankType���� ��ȯ
    public HandRankType ConvertToHandRankType()
    {
        if (handRankings == null)
        {
            Debug.LogWarning("HandRankings component is null!");
            return HandRankType.None;
        }

        // ���� handRankings���� ���� Ÿ�� ���� ��������
        return handRankings.GetCurrentHandRankType();
    }

    // ���� �� �� GameManager�� ��� ����
    public void EvaluateHand()
    {
        if (handRankings == null)
        {
            Debug.LogError("HandRankings is null! Cannot evaluate hand.");
            return;
        }

        // HandRankings���� ���� üũ ����
        handRankings.CheckHandRanking();

        // ��� ��������
        HandRankType result = handRankings.GetCurrentHandRankType();

        Debug.Log("Hand evaluated: " + result + " (" + handRankings.GetCurrentHandRankingName() + ")");

        // GameManager�� ��� ���� (HandRankings���� �̹� �ڵ����� ������, �������ε� ����)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ProcessHandResult(result);
        }
    }

    // ���� ���� �̸� ��������
    public string GetCurrentHandRankingName()
    {
        if (handRankings != null)
        {
            return handRankings.GetCurrentHandRankingName();
        }
        return "Unknown";
    }

    // ���� ���� Ÿ�� ��������
    public HandRankType GetCurrentHandRankType()
    {
        if (handRankings != null)
        {
            return handRankings.GetCurrentHandRankType();
        }
        return HandRankType.None;
    }

    // �������� ���� üũ (����׿�)
    [ContextMenu("Check Hand Ranking")]
    public void ManualCheckHandRanking()
    {
        EvaluateHand();
    }
}