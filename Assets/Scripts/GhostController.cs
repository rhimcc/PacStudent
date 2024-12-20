using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
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

    bool horizontalBorder;
    bool verticalBorder;
    int[] currentPos = new int[] { 14, 13 };
    int[] targetPos = new int[] { 12, 13 };
    public Tweener tweener;
    public GameObject[] ghosts = new GameObject[4];
    public bool movementAllowed = false;
    GameObject pacman;
    public Animator animator;
    private string lastDirection = "";
    private string previousDirection = "UP";

    private Vector3 ghostDirection;
    private bool moveGhostFromCentre = true;
    bool allowBacktracking = false;
    bool touchingOuterWall = false;
    Vector3 nextWall = new Vector3(12, 108, 0);
    string wall = "TOP";
    public string currentState = "Walking";
    private string[] states = new string[] { "Walking", "Scared", "Recovering", "Dead" };

    private GameObject mainCamera;
    BackgroundMusic backgroundMusic;
    public AudioClip[] audioClips;
    // Start is called before the first frame update
    void Start()
    {
        pacman = GameObject.Find("PacMan");
        ghostDirection = new Vector3(0, 0, 0);
        mainCamera = GameObject.Find("Main Camera");
        backgroundMusic = mainCamera.GetComponent<BackgroundMusic>();

    }

    // Update is called once per frame
    void Update()
    {
        if (movementAllowed)
        {
            if (moveGhostFromCentre) {
                MoveGhostFromCentre();
            } else
            {
                GhostMovement();
            }
        }
    }

    void GhostMovement()
    {
        if (animator.GetBool("Scared") || animator.GetBool("Recovering"))
        {
            Ghost1Movement();
        }
        else
        {
            if (animator.GetBool("Dead"))
            {
                DeathMovement();
            }
            else
            {
                switch (gameObject.name)
                {
                    case "Ghost1":
                        Ghost1Movement();
                        break;
                    case "Ghost2":
                        Ghost2Movement();
                        break;
                    case "Ghost3":
                        Ghost3Movement();
                        break;
                    case "Ghost4":
                        Ghost4Movement();
                        break;
                }
            }
        }
    }


    void DeathMovement()
    {
        Vector3 targetPosition = new Vector3(-4, 4, 0);
        if (tweener.activeTween == null)
        {
            tweener.AddTween(gameObject.transform, gameObject.transform.position, targetPosition);
        }
        if (Vector3.Distance(gameObject.transform.position, targetPosition) < 0.1f)
        {
            tweener.activeTween = null;
            transform.position = targetPosition;
            currentPos = new int[] { 14, 13 };
            targetPos = new int[] { 12, 13 };
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
            animator.SetBool("Down", false);
            animator.SetBool("Up", true);
            animator.SetBool("Walking", true);
            animator.SetBool("Dead", false);
            moveGhostFromCentre = true;
            CheckIfAnyDead();
        }
    }

    void CheckIfAnyDead()
    {
        int deadCount = 0;
        foreach (GameObject ghost in ghosts)
        {
            if (ghost.GetComponent<Animator>().GetBool("Dead"))
            {
                deadCount++;
            }
        }
        if (deadCount == 0)
        {
            switch(currentState)
            {
                case "Walking":
                    backgroundMusic.PlayNormalMusic();
                    break;
                case "Scared":
                    backgroundMusic.PlayGhostScaredMusic();
                    break;
                case "Recovering":
                    backgroundMusic.PlayGhostScaredMusic();
                    break;
            }
        }
    }




    void MoveGhostFromCentre()
    {
        Vector3 targetPosition = new Vector3(-4, 28, 0);
        if (tweener.activeTween == null)
        {
            tweener.AddTween(gameObject.transform, gameObject.transform.position, targetPosition);
        }
        if (Vector3.Distance(gameObject.transform.position, targetPosition) < 0.1f)
        {
            tweener.activeTween = null;
            currentPos[0] = targetPos[0] - 1;
            currentPos[1] = targetPos[1];
            moveGhostFromCentre = false;
        }
        animator.SetBool("Right", false);
        animator.SetBool("Up", true);
    }

    void Ghost4Movement()
    {
        if (tweener.activeTween == null)
        {
            List<Vector3> validDirections = GetValidDirections(gameObject.transform.position);
            ghostDirection = GoToNextWall(validDirections);
            tweener.AddTween(gameObject.transform, gameObject.transform.position, ghostDirection);
            UpdateArray(GetDirectionFromPosition(ghostDirection));
            SetAnimation(ghostDirection);
            string currentDirection = GetDirectionFromPosition(ghostDirection);
            previousDirection = currentDirection;
            lastDirection = GetOppositeDirection(currentDirection);
        }
        if (Vector3.Distance(gameObject.transform.position, ghostDirection) < 0.1f)
        {
            transform.position = ghostDirection;
            tweener.activeTween = null;
            currentPos[0] = targetPos[0];
            currentPos[1] = targetPos[1];
        }
        if (Vector3.Distance(gameObject.transform.position, nextWall) < 0.1f)
        {
            GetNextWall();
        }
    }

    Vector3 GoToNextWall(List<Vector3> validDirections)
    {
        Vector3 closestDirection = validDirections[0];
        foreach (Vector3 validDirection in validDirections)
        {
            if (Vector3.Distance(validDirection, nextWall) < Vector3.Distance(closestDirection, nextWall))
            {
                closestDirection = validDirection;
            }
        }
        return closestDirection;
    }

    void GetNextWall()
    {
        if (nextWall.x == 100 && nextWall.y == 52) // top of tunnel on the right
        {
            nextWall = new Vector3(100, 4, 0); // inside the tunnel on the right
            return;
        }
        if (nextWall.x == 100 && nextWall.y == 4) // inside the tunnel on the right
        {
            nextWall = new Vector3(100, -44, 0); // bottom of tunnel on the right
            return;
        }
        if (nextWall.x == -100 && nextWall.y == -44) // bottom of tunnel on the right
        {
            nextWall = new Vector3(-100, 4, 0); // inside the tunnel on the right
            return;
        }
        if (nextWall.x == -100 && nextWall.y == 4) // inside the tunnel on the right
        {
            nextWall = new Vector3(-100, 52, 0); // bottom of tunnel on the right
            return;
        }

        if (nextWall.x == 12 && nextWall.y == -100) // bar at bottom
        {
            nextWall = new Vector3(-12, -100, 0);
        }
        if (nextWall.x == -12 && nextWall.y == 108) // bar at bottom
        {
            nextWall = new Vector3(12, 108, 0);
        }


        if (nextWall.x == 100 && nextWall.y == 108) // top right corner
        {
            wall = "RIGHT";
        }
        if (nextWall.x == -100 && nextWall.y == -100) // bottom left corner
        {
            wall = "LEFT";
        }
        if (nextWall.x == -100 && nextWall.y == 108) // top left corner
        {
            wall = "TOP";
        }
        if (nextWall.x == 100 && nextWall.y == -100) // bottom right corner
        {
            wall = "BOTTOM";
        }

        switch (wall)
        {
            case "TOP":
                nextWall += new Vector3(8, 0, 0);
                break;
            case "RIGHT":
                nextWall += new Vector3(0, -8, 0);
                break;
            case "BOTTOM":
                nextWall += new Vector3(-8, 0, 0);
                break;
            case "LEFT":
                nextWall += new Vector3(0, 8, 0);
                break;
        }
        
    }

    bool isOuterWall(GameObject gameObject)
    {
        Vector3 position = gameObject.transform.position;
        return position.x >= 108 || position.x <= -108 || position.y >= 116 || position.y <= -116;
    }

    string GetLeftRotation(string direction)
    {
        switch(direction)
        {
            case "UP":
                return "LEFT";
            case "LEFT":
                return "DOWN";
            case "DOWN":
                return "RIGHT";
            case "RIGHT":
                return "UP";
        }
        return "";
    }

    string GetRightRotation(string direction)
    {
        switch (direction)
        {
            case "LEFT":
                return "UP";
            case "DOWN":
                return "LEFT";
            case "RIGHT":
                return "DOWN";
            case "UP":
                return "RIGHT";
        }
        return "";
    }

    void Ghost3Movement()
    {
        if (tweener.activeTween == null)
        {
            List<Vector3> validDirections = GetValidDirections(gameObject.transform.position);
            int randomInt = Random.Range(0, validDirections.Count);
            ghostDirection = validDirections[randomInt];
            tweener.AddTween(gameObject.transform, gameObject.transform.position, ghostDirection);
            UpdateArray(GetDirectionFromPosition(ghostDirection));
            SetAnimation(ghostDirection);

            string currentDirection = GetDirectionFromPosition(ghostDirection);
            lastDirection = GetOppositeDirection(currentDirection);
        }
        if (Vector3.Distance(gameObject.transform.position, ghostDirection) < 0.1f)
        {
            tweener.activeTween = null;
            currentPos[0] = targetPos[0];
            currentPos[1] = targetPos[1];
        }
    }

    void Ghost1Movement()
    {
        if (tweener.activeTween == null)
        {
            List<Vector3> validDirections = GetValidDirections(gameObject.transform.position);
            ghostDirection = GetLargestDistance(validDirections);
            tweener.AddTween(gameObject.transform, gameObject.transform.position, ghostDirection);
            UpdateArray(GetDirectionFromPosition(ghostDirection));
            SetAnimation(ghostDirection);

            string currentDirection = GetDirectionFromPosition(ghostDirection);
            lastDirection = GetOppositeDirection(currentDirection);
        }
        if (Vector3.Distance(gameObject.transform.position, ghostDirection) < 0.1f)
        {
            tweener.activeTween = null;
            currentPos[0] = targetPos[0];
            currentPos[1] = targetPos[1];
        }
       
    }

    void Ghost2Movement()
    {
        if (tweener.activeTween == null)
        {
            List<Vector3> validDirections = GetValidDirections(gameObject.transform.position);
            ghostDirection = GetSmallestDistance(validDirections);
            tweener.AddTween(gameObject.transform, gameObject.transform.position, ghostDirection);
            UpdateArray(GetDirectionFromPosition(ghostDirection));
            SetAnimation(ghostDirection);

            string currentDirection = GetDirectionFromPosition(ghostDirection);
            lastDirection = GetOppositeDirection(currentDirection);
        }
        if (Vector3.Distance(gameObject.transform.position, ghostDirection) < 0.1f)
        {
            tweener.activeTween = null;
            currentPos[0] = targetPos[0];
            currentPos[1] = targetPos[1];
        }
    }

    void UpdateArray(string direction)
    {
        int currentX = currentPos[0];
        int currentY = currentPos[1];
        int lastXindex = levelMap.GetLength(1) - 1;
        int lastYindex = levelMap.GetLength(0) - 1;
        int quadrant = DetectQuadrant(gameObject.transform, direction);

        horizontalBorder = false;
        verticalBorder= false;
        DetectBorder(direction);

        switch (direction)
        {
            case "UP":
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

            case "DOWN":
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

            case "LEFT":
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

            case "RIGHT":
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
        }
    }

    void SetAnimation(Vector3 direction)
    {
        foreach (string state in states)
        {
            animator.SetBool(state, false);
        }
        animator.SetBool(currentState, true);
        animator.SetBool("Left", false);
        animator.SetBool("Right", false);
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        switch (GetDirectionFromPosition(direction))
        {
            case "RIGHT":
                animator.SetBool("Right", true);
                boxCollider.size = new Vector3(6f, 3f, 0.2f);
                break;
            case "LEFT":
                animator.SetBool("Left", true);
                boxCollider.size = new Vector3(6f, 3f, 0.2f);
                break;
            case "UP":
                animator.SetBool("Up", true);
                boxCollider.size = new Vector3(3f, 6f, 0.2f);
                break;
            case "DOWN":
                animator.SetBool("Down", true);
                boxCollider.size = new Vector3(3f, 6f, 0.2f);
                break;
        }
        

    }

    string GetDirectionFromPosition(Vector3 direction)
    {
        Vector3 ghostPosition = gameObject.transform.position;
        Vector3 positionDifference = direction - ghostPosition;
        if (positionDifference.x > 0) // right
        {
            return "RIGHT";
        }
        else if (positionDifference.x < 0) // left
        {
            return "LEFT";

        }
        else if (positionDifference.y > 0) // up
        {
            return "UP";

        }
        else if (positionDifference.y < 0) //down
        {
            return "DOWN";

        }
        return "";
    }
    Vector3 GetPositionFromDirection(string direction)
    {
        Vector3 ghostPosition = gameObject.transform.position;
        switch(direction)
        {
            case "UP":
                return ghostPosition + new Vector3(0, 8, 0);
            case "DOWN":
                return ghostPosition + new Vector3(0, -8, 0);
            case "LEFT":
                return ghostPosition + new Vector3(-8, 0, 0);
            case "RIGHT":
                return ghostPosition + new Vector3(8, 0, 0);

        }
        return new Vector3(0, 0, 0);


    }

    Vector3 GetLargestDistance(List<Vector3> validDirections)
    {
        Vector3 directionWithLargestDistance = new Vector3(0,0,0);
        float distance = 0;
        foreach (Vector3 validDirection in validDirections)
        {
            float calculatedDistance = CalculateDistance(validDirection, pacman);
            if (calculatedDistance > distance)
            {
                distance = calculatedDistance;
                directionWithLargestDistance = validDirection;
            }
        }
        return directionWithLargestDistance;
    }

    Vector3 GetSmallestDistance(List<Vector3> validDirections)
    {
        Vector3 directionWithSmallestDistance = validDirections[0];
        float distance = CalculateDistance(validDirections[0], pacman);
        foreach (Vector3 validDirection in validDirections)
        {
           
            float calculatedDistance = CalculateDistance(validDirection, pacman);
            if (calculatedDistance < distance)
            {
                distance = calculatedDistance;
                directionWithSmallestDistance = validDirection;
            }
        }
        return directionWithSmallestDistance;
    }


    float CalculateDistance(Vector3 direction, GameObject gameObject)
    {
        return Vector3.Distance(direction, gameObject.transform.position);
    }

    bool positionInCentre(Vector3 position)
    {
        return position.x > -35 && position.x < 35 && position.y > -19 && position.y < 27;
    }


    List<Vector3> GetValidDirections(Vector3 position)
    {
        List<Vector3> validDirections = new List<Vector3>();
        Vector3 upPosition = position + new Vector3(0, 8, 0);
        Vector3 downPosition = position + new Vector3(0, -8, 0);
        Vector3 leftPosition = position + new Vector3(-8, 0, 0);
        Vector3 rightPosition = position + new Vector3(8, 0, 0);
        allowBacktracking = false;
        if (IsValidPosition("UP") && (allowBacktracking || lastDirection != "UP") && !positionInCentre(upPosition))
        {
            validDirections.Add(upPosition);
        }
        if (IsValidPosition("DOWN") && (allowBacktracking || lastDirection != "DOWN") && !positionInCentre(downPosition))
        {
            validDirections.Add(downPosition);
        }
        if (IsValidPosition("LEFT") && (allowBacktracking || lastDirection != "LEFT") && !positionInCentre(leftPosition))
        {
            validDirections.Add(leftPosition);
        }

        if (IsValidPosition("RIGHT") && (allowBacktracking || lastDirection != "RIGHT") && !positionInCentre(rightPosition))
        {
            validDirections.Add(rightPosition);
        }

        return validDirections;
    }

    private string GetOppositeDirection(string direction)
    {
        switch (direction)
        {
            case "UP": return "DOWN";
            case "DOWN": return "UP";
            case "LEFT": return "RIGHT";
            case "RIGHT": return "LEFT";
            default: return "";
        }
    }


    bool IsValidPosition(string direction)
    {
       
        int currentX = currentPos[0];
        int currentY = currentPos[1];
        int[] targetPos = new int[] { currentX, currentY };
        int lastXindex = levelMap.GetLength(1) - 1;
        int lastYindex = levelMap.GetLength(0) - 1;
        int quadrant = DetectQuadrant(gameObject.transform, direction);

        horizontalBorder = false;
        verticalBorder = false;
        DetectBorder(direction);
        float x = transform.position.x;
        float y = transform.position.y;
        switch (direction)
        {
            case "UP":
                targetPos[1] = currentY;
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
                    targetPos[0] = currentX - 1;
                }
                else
                {
                    targetPos[0] = currentX + 1;
                }
                break;

            case "DOWN":
                targetPos[1] = currentY;
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
                    targetPos[0] = currentX + 1;
                }
                else
                {
                    targetPos[0] = currentX - 1;
                }
                break;

            case "LEFT":
                targetPos[0] = currentX;
                if (x > 104)
                {
                    allowBacktracking = true;
                    targetPos[1] = 0;
                    return true;
                }
                if (horizontalBorder)
                {
                    targetPos[1] = lastXindex;
                }
                else
            if (quadrant == 1 || quadrant == 4)
                {
                    targetPos[1] = currentY - 1;
                }
                else
                {
                    targetPos[1] = currentY + 1;
                }
                break;

            case "RIGHT":
                targetPos[0] = currentX;
                if (x < -104)
                {
                    allowBacktracking = true;
                    targetPos[1] = 0;
                    return true;
                }
                if (horizontalBorder)
                {
                    targetPos[1] = lastXindex;
                }
                else
                if (quadrant == 1 || quadrant == 4)
                {
                    targetPos[1] = currentY + 1;
                }
                else
                {
                    targetPos[1] = currentY - 1;
                }
                break;
        }
        if (x < -104 || x > 104)
        {
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

    void DetectBorder(string direction)
    {
        Vector3 position = gameObject.transform.position;
        switch (direction)
        {
            case "UP":
                position += new Vector3(0, 8, 0);
                break;
            case "DOWN":
                position += new Vector3(0, -8, 0);
                break;
            case "LEFT":
                position += new Vector3(-8, 0, 0);
                break;
            case "RIGHT":
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


    int DetectQuadrant(Transform transform, string direction)
    {
        Vector3 position = transform.position;

        switch (direction)
        {
            case "UP":
                position += new Vector3(0, 8, 0);
                break;
            case "DOWN":
                position += new Vector3(0, -8, 0);
                break;
            case "LEFT":
                position += new Vector3(-8, 0, 0);
                break;
            case "RIGHT":
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
}
