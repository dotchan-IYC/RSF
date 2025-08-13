using UnityEngine;

public class NewGame : MonoBehaviour
{
    [SerializeField] SpriteRenderer _card1;
    [SerializeField] SpriteRenderer _card2;
    [SerializeField] SpriteRenderer _communityCard3;
    [SerializeField] SpriteRenderer _communityCard4;
    [SerializeField] SpriteRenderer _communityCard5;

    [SerializeField] Sprite _back_of_card;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //카드들 다시 뒷면으로 바꾸는 코드(일단 시간 없어서 대충 함)
            _card1.sprite = _back_of_card;
            _card2.sprite = _back_of_card;
            _communityCard3.sprite = _back_of_card;
            _communityCard4.sprite = _back_of_card;
            _communityCard5.sprite = _back_of_card;


            gameObject.SetActive(false);//자기 자신 끄기
        }
    }
}
