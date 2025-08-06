using System;
using UnityEngine;

namespace Code.RSF
{
    public class HideCommunityCard : MonoBehaviour
    {
        [SerializeField] GameCicle _gameCicle;

        SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
        private void Start()
        {
            _gameCicle.ShowEveryCard += Hide;
            _gameCicle.ResetGame += Show;
        }

        private void Hide()
        {
            _renderer.enabled = false;
        }

        void Show()
        {
            _renderer.enabled = true;
        }
    }
}
