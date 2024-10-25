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
    int[] targetPos;

    string lastInput;
    string currentInput;
    public AudioClip[] movement;
    public AudioSource audioSource;
    public Animator animator;
    bool reducing = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = movement[0];
        tweener = gameObject.GetComponent<Tweener>();
        currentPos = new int[] { 1, 1 };
        targetPos = new int[] { 1, 1 };

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
                }
                else
                {
                    animator.speed = 0;
                }
            }
        }

    }

    bool CanMove(string direction)
    {
        // Reset targetPos to current position at start of check
        targetPos[0] = currentPos[0];
        targetPos[1] = currentPos[1];
        int lastXindex = levelMap.GetLength(1) - 1;
        int lastYindex = levelMap.GetLength(0) - 1;
        int quadrant = DetectQuadrant(direction);
        bool border = DetectBorder(direction);

        switch (direction)
        {
            case "W":
                targetPos[1] = currentPos[1];
                if (border)
                {
                    if (quadrant == 1 || quadrant == 2)
                    {
                        targetPos[0] = lastYindex;
                    } else
                    {
                        targetPos[0] = lastYindex - 1;
                    }

                } else if (quadrant == 1 || quadrant == 2)
                {
                    targetPos[0] = currentPos[0] - 1;
                }
                else
                {
                    targetPos[0] = currentPos[0] + 1;
                }
                break;

            case "S":
                targetPos[1] = currentPos[1];
                if (border)
                {
                    if (quadrant == 1 || quadrant == 2)
                    {
                        targetPos[0] = lastYindex;
                    }
                    else
                    {
                        targetPos[0] = lastYindex - 1;
                    }
                } else 
                if (quadrant == 1 || quadrant == 2)
                {
                    targetPos[0] = currentPos[0] + 1;
                }
                else
                {
                    targetPos[0] = currentPos[0] - 1;
                }
                break;

            case "A":
                targetPos[0] = currentPos[0];
                if (border)
                {
                    targetPos[1] = lastXindex;
                }
                else
            if (quadrant == 1 || quadrant == 4)
                {
                    targetPos[1] = currentPos[1] - 1;
                } else
                {
                    targetPos[1] = currentPos[1] + 1;
                }
                break;

            case "D":
                targetPos[0] = currentPos[0];
                if (border)
                {
                    targetPos[1] = lastXindex;
                }
                else
                if (quadrant == 1 || quadrant == 4)
                {
                    targetPos[1] = currentPos[1] + 1;
                }
                else
                {
                    targetPos[1] = currentPos[1] - 1;
                }
                break;

            default:
                return false;
        }
        print(targetPos[0] + " " + targetPos[1]);
        if (targetPos[0] >= 0 && targetPos[0] < levelMap.GetLength(0) &&
            targetPos[1] >= 0 && targetPos[1] < levelMap.GetLength(1))
        {
            int targetTile = levelMap[targetPos[0], targetPos[1]];
            return (targetTile == 5 || targetTile == 6 || targetTile == 0);
        }

        return false;
    }

    void Move(string direction)
    {
        Vector3 targetPosition = new Vector3(0, 0, 0);
        Vector3 currentPosition = transform.position;

        switch (direction)
        {
            case "W":
                targetPosition = currentPosition + new Vector3(0, 8, 0);
                break;
            case "S":
                targetPosition = currentPosition + new Vector3(0, -8, 0);
                break;
            case "A":
                targetPosition = currentPosition + new Vector3(-8, 0, 0);
                break;
            case "D":
                targetPosition = currentPosition + new Vector3(8, 0, 0);
                break;
        }

        if (levelMap[targetPos[0], targetPos[1]] == 5)
        {
            audioSource.clip = movement[1];
            audioSource.Play();
        }
        else
        {
            audioSource.clip = movement[0];
            audioSource.Play();
        }

        currentPos = new int[] { targetPos[0], targetPos[1] };  // Create new array to avoid reference issues
        tweener.AddTween(transform, transform.position, targetPosition);
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

    int DetectQuadrant(string direction)
    {
        Vector3 position = transform.position;
        switch(direction)
        {
            case "W":
                position += new Vector3(0, 8, 0);
                break;
            case "S":
                position += new Vector3(0, -8, 0);
                break;
            case "A":
                position += new Vector3(-8, 0, 0);
                break;
            case "D":
                position += new Vector3(8, 0, 0);
                break;
        }
        if (position.x <= 0 && position.x >= -108)
        {
            if (position.y >= 0 && position.y <= 116)
            {
                return 1;
            }
            else if (position.y < 0 && position.y >= -108)
            {
                return 4;
            }
            else return 0;
        } else
        {

            if (position.y >= 0 && position.y <= 116)
            {
                return 2;
            }
            else if(position.y < 0 && position.y >= -108)
            {
                return 3;
            }
            else return 0;
        }
    }

    bool DetectBorder(string direction)
    {
        Vector3 position = transform.position;
        switch (direction)
        {
            case "W":
                position += new Vector3(0, 8, 0);
                break;
            case "S":
                position += new Vector3(0, -8, 0);
                break;
            case "A":
                position += new Vector3(-8, 0, 0);
                break;
            case "D":
                position += new Vector3(8, 0, 0);
                break;
        }
        if (position.x >= -4 && position.x <= 4)
        {
            return true;
        }
        else
        {
            if (position.y >= 0 && position.y <= 8)
            {
                return true;
            }
        }
        return false;
    }
}