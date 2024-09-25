using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMovement : MonoBehaviour
{
    Animator animationController;
    Tweener tweener;
    void Start()
    {
        animationController = GetComponent<Animator>();
        tweener = GetComponent<Tweener>();
        animationController.SetBool("Right", true);
        tweener.AddTween(transform, transform.position, new Vector3(-60, 100, 0));
    }

    void Update()
    {
        print("Right: " + animationController.GetBool("Right"));
        print("Down: " + animationController.GetBool("Down"));
        print("Left: " + animationController.GetBool("Left"));
        print("Up: " + animationController.GetBool("Up"));
        //print("transform position: ");
        //print(transform.position);
        //print("active tween end pos");
        //print(tweener.activeTween.EndPos);

        if (transform.position == tweener.activeTween.EndPos)
        {
            if (animationController.GetBool("Right"))
            {
                animationController.SetBool("Right", false);
                animationController.SetBool("Down", true);
                tweener.AddTween(transform, transform.position, new Vector3(-60, 68, 0));

            }
            else if (animationController.GetBool("Down"))
            {
                animationController.SetBool("Down", false);
                animationController.SetBool("Left", true);
                tweener.AddTween(transform, transform.position, new Vector3(-100, 68, 0));

            } else if (animationController.GetBool("Left"))
            {
                animationController.SetBool("Left", false);
                animationController.SetBool("Up", true);
                tweener.AddTween(transform, transform.position, new Vector3(-100, 100, 0));

            } else if (animationController.GetBool("Up"))
            {
                animationController.SetBool("Up", false);
                animationController.SetBool("Right", true);
                tweener.AddTween(transform, transform.position, new Vector3(-60, 100, 0));


            }
        }
    }
}
