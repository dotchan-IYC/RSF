using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MouseInput : MonoBehaviour
{
    public event Action LeftMouseClick;
    public Vector2 _mousePosition;
    public GameObject _collisionGameObject;
    private void Update()//진짜 말 그대로 마우스 좌클릭하면 위치 확인하는 거임
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(_mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                _collisionGameObject = hit.collider.gameObject;
            }
            else
            {
                _collisionGameObject = null;
            }
            LeftMouseClick.Invoke();
        }
    }
}
