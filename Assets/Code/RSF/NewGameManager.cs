using System;
using UnityEngine;
using UnityEngine.UI;

public class NewGameManager : MonoBehaviour
{
    public event Action OnPressedDrawButton;
    public event Action OnPressedBettingButton;
    public event Action OnPressedDealButton;
    public event Action OnPressedResetButton;
    public static NewGameManager Instance { get; private set; }

    public GameObject _gmaEnd;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    private void Start()
    {
        OnPressedDealButton += GmaEndShow;
    }


    public void OnPressBettingButton()
    {
        OnPressedBettingButton.Invoke();
    }

    public void OnPressDrawButton()
    {
        OnPressedDrawButton.Invoke();
    }
    public void OnPressDealButtion()
    {
        OnPressedDealButton.Invoke();
    }
    public void OnPressResetButton()
    {
        OnPressedResetButton.Invoke();
    }
    private void GmaEndShow()
    {
        _gmaEnd.SetActive(true);
    }


}
