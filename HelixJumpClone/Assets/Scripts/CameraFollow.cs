using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private float _offset;
    void Awake()
    {
        _offset = transform.position.y - target.transform.position.y;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = target.transform.position.y + _offset;
        transform.position = currentPosition;
    }
}
