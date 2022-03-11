using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixRotator : MonoBehaviour
{
    private Vector2 _lastPos;

    private float _rotationSpeed = 0.45f;

    void Update()
    {
        if (Input.GetMouseButton(0) && GameManager.Instance.gameOver == false)
        {
            Vector2 currentPos = Input.mousePosition;

            if (_lastPos == Vector2.zero)
                _lastPos = currentPos;

            float delta = _lastPos.x - currentPos.x;

            transform.Rotate(Vector3.up * delta * _rotationSpeed);

            _lastPos = currentPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _lastPos = Vector2.zero;
        }
    }
}
