using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.mute == false)
            GetComponentInParent<AudioSource>().Play();

        GameManager.Instance.NextLevel();
    }
}
