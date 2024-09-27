using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMovement : MonoBehaviour
{
    Animator animationController;
    Tweener tweener;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animationController = GetComponent<Animator>();
        tweener = GetComponent<Tweener>();
        animationController.SetBool("Right", true);
        tweener.AddTween(transform, transform.position, new Vector3(-60, 108, 0));
        audioSource.Play();
    }

    void Update()
    {

        if (transform.position == tweener.activeTween.EndPos)
        {
            if (animationController.GetBool("Right"))
            {
                animationController.SetBool("Right", false);
                animationController.SetBool("Down", true);
                tweener.AddTween(transform, transform.position, new Vector3(-60, 78, 0));

            }
            else if (animationController.GetBool("Down"))
            {
                animationController.SetBool("Down", false);
                animationController.SetBool("Left", true);
                tweener.AddTween(transform, transform.position, new Vector3(-100, 78, 0));

            }
            else if (animationController.GetBool("Left"))
            {
                animationController.SetBool("Left", false);
                animationController.SetBool("Up", true);
                tweener.AddTween(transform, transform.position, new Vector3(-100, 108, 0));

            }
            else if (animationController.GetBool("Up"))
            {
                animationController.SetBool("Up", false);
                animationController.SetBool("Right", true);
                tweener.AddTween(transform, transform.position, new Vector3(-60, 108, 0));


            }
        }
    }
}
