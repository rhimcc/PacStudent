using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    bool horizontalBorder;
    bool verticalBorder;
    ParticleSystem particleSystem;
    int score = 0;
    Text scoreText;
    float ghostScaredTime = 10;
    GameObject ghostScaredTimer;
    Text ghostScaredText;
    List<Vector3> eatenPellets = new List<Vector3>();
    GameObject[] ghosts;
    Animator[] ghostAnimators;


    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GameObject.Find("PacManTrail").GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = movement[0];
        tweener = gameObject.GetComponent<Tweener>();
        currentPos = new int[] { 1, 1 };
        targetPos = new int[] { 1, 1 };
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        currentInput = ""; // Initialize currentInput
        ghostScaredTimer = GameObject.Find("GhostScaredTimer");
        ghostScaredText = ghostScaredTimer.GetComponent<Text>();

        GameObject ghost1 = GameObject.Find("Ghost1");
        GameObject ghost2 = GameObject.Find("Ghost2");
        GameObject ghost3 = GameObject.Find("Ghost3");
        GameObject ghost4 = GameObject.Find("Ghost4");

        Animator ghost1Animator = ghost1.GetComponent<Animator>();
        Animator ghost2Animator = ghost2.GetComponent<Animator>();
        Animator ghost3Animator = ghost3.GetComponent<Animator>();
        Animator ghost4Animator = ghost4.GetComponent<Animator>();
        ghosts = new GameObject[] { ghost1, ghost2, ghost3, ghost4 };
        ghostAnimators = new Animator[] { ghost1Animator, ghost2Animator, ghost3Animator, ghost4Animator };
        foreach (Animator animator in ghostAnimators)
        {
            animator.SetBool("Right", true);
        }
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
                SetTrail(currentInput);
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
                    audioSource.Stop();
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

        horizontalBorder = false;
        verticalBorder = false;
        DetectBorder(direction);


        switch (direction)
        {
            case "W":
                targetPos[1] = currentPos[1];
                if (verticalBorder)
                {
                    if (quadrant == 1 || quadrant == 2)
                    {
                        targetPos[0] = lastYindex;
                    }
                    else
                    {
                        targetPos[0] = lastYindex - 1;
                    }

                }
                else if (quadrant == 1 || quadrant == 2)
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
                if (verticalBorder)
                {
                    if (quadrant == 1 || quadrant == 2)
                    {
                        targetPos[0] = lastYindex;
                    }
                    else
                    {
                        targetPos[0] = lastYindex - 1;
                    }
                }
                else
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
                if (horizontalBorder)
                {
                    targetPos[1] = lastXindex;
                }
                else
            if (quadrant == 1 || quadrant == 4)
                {
                    targetPos[1] = currentPos[1] - 1;
                }
                else
                {
                    targetPos[1] = currentPos[1] + 1;
                }
                break;

            case "D":
                targetPos[0] = currentPos[0];
                if (horizontalBorder)
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

        if (levelMap[targetPos[0], targetPos[1]] == 5 && !eatenPellets.Contains(targetPosition))
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

    void SetTrail(string direction)
    {
        Transform particleTransform = particleSystem.transform;
        Vector3 rotation = particleTransform.eulerAngles;
        Vector3 position = transform.position + new Vector3(0, -5, 0);
        switch (direction)
        {
            case "W":
                rotation.z = 90;
                position += new Vector3(0, -2, 0);
                break;

            case "S":
                rotation.z = 90;
                position += new Vector3(0, 2, 0);

                break;

            case "D":
                position += new Vector3(-2, 0, 0);

                break;
            case "A":
                position += new Vector3(2, 0, 0);

                break;
        }
        particleTransform.eulerAngles = rotation;
        particleTransform.position = position;

    }

    void DetectBorder(string direction)
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
        if (position.x >= -8 && position.x <= 8)
        {
            horizontalBorder = true;
        }
        if (position.x > 108 || position.x < -108)
        {
            horizontalBorder = true;
        }

        if (position.y >= 0 && position.y <= 8)
        {
            verticalBorder = true;
        }

    }

    int DetectQuadrant(string direction)
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
                else
                {
                    return 0;
                }
            }
            else if (position.x >= 0 && position.x <= 116)
            {
                if (position.y >= 0 && position.y <= 116)
                {
                    return 2;
                }
                else if (position.y < 0 && position.y >= -108)
                {
                    return 3;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

    

        void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("Collision with Wall");
            // Handle wall collision logic here
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        switch(collider.tag)
        {
            case "Pellet":
                HandlePellet(collider);
                break;

            case "Tunnel":
                HandleTunnel();
                break;

            case "Bonus":
                HandleBonus(collider);
                break;

            case "Power":
                HandlePower(collider);
                break;
        }
    }

    void updateScore(int points)
    {
        score += points;
        scoreText.text = "\n" + score;
    }
    // Power pills:
//? Change the Ghost animator state to ?Scared?.
//? Change the background music to match this state.
//? Start a timer for 10 seconds.Make the Ghost Timer UI element
//visible and set it to this timer.
//? With 3 seconds left to go on this timer, change the Ghosts to
//the Recovering state.
//? After 10 seconds have passed, set the Ghosts back to their
//Walking states and hide the Ghost Timer UI element.

    void HandleTunnel()
    {
        float currentX = transform.position.x;
        float currentY = transform.position.y;
        float newX = 0;
        float newY = currentY;
        if (currentX > 0)
        {
            newX = -100;
            currentPos = new int[] { 14, 1 };
        }
        else
        {
            newX = 100;
            currentPos = new int[] { 14, 1 };
        }
        transform.position = new Vector3(newX, newY, 0);
        tweener.activeTween = null;
    }

    void HandlePellet(Collider collider)
    {
        Destroy(collider.gameObject);
        eatenPellets.Add(collider.gameObject.transform.position);
        updateScore(10);    
    }

    void HandleBonus(Collider collider)
    {
        Destroy(collider.gameObject);
        updateScore(100);
    }

    void HandlePower(Collider collider)
    {
        Destroy(collider.gameObject);
        foreach (Animator animator in ghostAnimators)
        {
            animator.SetBool("Scared", true);
            animator.SetBool("Walking", false);
        }

        StartCoroutine(HandleTimer());

    }



    private IEnumerator HandleTimer()
    {
        ghostScaredTimer.SetActive(true);
        ghostScaredTime = 10;

        while (ghostScaredTime > 0)
        {
            string time = "\n\n" + ghostScaredTime;
            ghostScaredText.text = time;

            yield return new WaitForSeconds(1f);
            ghostScaredTime--;
            if (ghostScaredTime == 3)
            {
                foreach (Animator animator in ghostAnimators)
                {
                    animator.SetBool("Recovering", true);
                    animator.SetBool("Scared", false);

                }
            }
            if (ghostScaredTime == 0)
            {
                foreach (Animator animator in ghostAnimators)
                {
                    animator.SetBool("Recovering", false);
                    animator.SetBool("Walking", true);

                }
                ghostScaredTimer.SetActive(false);
            }
        }

        ghostScaredText.text = "\n\n0";
    }



}

