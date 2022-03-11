using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = GameManager.Instance.ballColor;
        transform.Rotate(90,0,0);

        transform.Rotate(0, 0, Random.Range(-90, 90));
        transform.lossyScale.Set(Random.Range(0.12f, 0.2f), Random.Range(0.13f, 0.17f), 1);

        Destroy(gameObject, 5f);
    }
}
