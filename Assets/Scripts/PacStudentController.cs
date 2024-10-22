using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    int[,] levelMap =
      {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},

        };

    int[] currentPos;
    string lastInput;
    string currentInput;
    public AudioClip[] movement;
    public AudioSource audioSource;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = movement[0];
        tweener = gameObject.GetComponent<Tweener>();
        currentPos = new int[] { 1, 1 };
        currentInput = ""; // Initialize currentInput
    }

    // Update is called once per frame
    void Update()
    {
        if (tweener.activeTween != null && tweener.activeTween.EndPos == transform.position)
        {
            tweener.activeTween = null;
        }
        if (Input.GetKeyDown(KeyCode.W)) lastInput = "W";
        if (Input.GetKeyDown(KeyCode.S)) lastInput = "S";
        if (Input.GetKeyDown(KeyCode.A)) lastInput = "A";
        if (Input.GetKeyDown(KeyCode.D)) lastInput = "D";

        if (tweener.activeTween == null)
        {
            if (CanMove(lastInput))
            {
                currentInput = lastInput;
                SetAnimation(currentInput);
                Move(lastInput);
            }
            else
            {
                if (CanMove(currentInput))
                {
                    Move(currentInput);
                } else
                {
                    animator.speed = 0;
                }
            }
        }

    }

    bool CanMove(string direction)
    {
        int[] targetPos = new int[2];
       
        switch (direction)
        {
            case "W":
                targetPos[0] = currentPos[0] - 1;
                targetPos[1] = currentPos[1];
                break;
            case "S":
                targetPos[0] = currentPos[0] + 1;
                targetPos[1] = currentPos[1];
                break;
            case "A":
                targetPos[0] = currentPos[0];
                targetPos[1] = currentPos[1] - 1;
                break;
            case "D":
                targetPos[0] = currentPos[0];
                targetPos[1] = currentPos[1] + 1;
                break;
            default:
                return false; // No valid direction
        }

        if (targetPos[0] >= 0 && targetPos[0] < levelMap.GetLength(0) &&
            targetPos[1] >= 0 && targetPos[1] < levelMap.GetLength(1))
        {
            int targetInt = levelMap[targetPos[0], targetPos[1]];
            return (targetInt == 5 || targetInt == 6 || targetInt == 0);
        }


        return false; // Out of bounds
    }

    void Move(string direction)
    {
        int[] targetPos = new int[2];
        Vector3 targetPosition = new Vector3(0, 0, 0);
        Vector3 currentPosition = transform.position;


        // Determine the target position based on the input direction
        switch (direction)
        {
            case "W":
                targetPos[0] = currentPos[0] - 1;
                targetPos[1] = currentPos[1];
                targetPosition = currentPosition += new Vector3(0, 8, 0);
                break;
            case "S":
                targetPos[0] = currentPos[0] + 1;
                targetPos[1] = currentPos[1];
                targetPosition = currentPosition += new Vector3(0, -8, 0);
                break;
            case "A":
                targetPos[0] = currentPos[0];
                targetPos[1] = currentPos[1] - 1;
                targetPosition = currentPosition += new Vector3(-8, 0, 0);
                break;
            case "D":
                targetPos[0] = currentPos[0];
                targetPos[1] = currentPos[1] + 1;
                targetPosition = currentPosition += new Vector3(8, 0, 0);
                break;
        }
        if (levelMap[targetPos[0], targetPos[1]] == 5)
        {
            audioSource.clip = movement[1];
            audioSource.Play();
        } else
        {
            audioSource.clip = movement[0];
            audioSource.Play();
        }
        currentPos = targetPos; // Update the current position
        tweener.AddTween(transform, transform.position, targetPosition); // Call the tweener
        animator.speed = 1;
    }

    void SetAnimation(string direction)
    {
        switch (direction)
        {
            case "W":
                animator.SetBool("Up", true);
                animator.SetBool("Down", false);
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
                break;
            case "S":
                animator.SetBool("Down", true);
                animator.SetBool("Up", false);
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);

                break;
            case "D":
                animator.SetBool("Right", true);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                animator.SetBool("Left", false);

                break;
            case "A":
                animator.SetBool("Left", true);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                animator.SetBool("Right", false);

                break;
        }
    }
}


