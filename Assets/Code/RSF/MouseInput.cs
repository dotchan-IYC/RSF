using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MouseInput : MonoBehaviour
{
    public event Action LeftMouseClick;
    public Vector2 _mousePosition;
    public GameObject _collisionGameObject;
    private void Update()//��¥ �� �״�� ���콺 ��Ŭ���ϸ� ��ġ Ȯ���ϴ� ����
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
