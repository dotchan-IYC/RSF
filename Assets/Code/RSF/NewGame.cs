using Code.RSF;
using System;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    public GameCicle _cicle;
    private void Start()
    {
        _cicle.ResetGame += ActiveFalse;
    }

    private void ActiveFalse()
    {
        gameObject.SetActive(false);//�ڱ� �ڽ� ����
    }
}
