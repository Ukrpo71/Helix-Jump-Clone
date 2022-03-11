using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCheck : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.AddScore(2);

        BallBounce ballBounce = other.GetComponent<BallBounce>();

        ballBounce._passingAudio.pitch += 0.5f;
        if (GameManager.Instance.mute == false)
            ballBounce._passingAudio.Play();

        /*for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var animator = transform.GetChild(i).GetComponent<Animator>();
            animator.Play("Destroy");

        }
        */

        ballBounce.perfectPasses++;
        ballBounce.totalRingsPassed++;
        if (ballBounce.perfectPasses > 1 && !ballBounce.isSuperSpeedActive)
            ballBounce.ToggleSuper();

        GameManager.Instance.UpdateSlide();
    }
}
