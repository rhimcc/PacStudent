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
    private Vector3 ghostDirection;
    private bool moveGhostFromCentre = true;
    bool allowBacktracking = false;


    // Start is called before the first frame update
    void Start()
    {
        pacman = GameObject.Find("PacMan");
        ghostDirection = new Vector3(0, 0, 0);
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
        switch(gameObject.name)
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
            Vector3 clockwiseDirection = GetClockwiseDirection(validDirections);

            if (clockwiseDirection != Vector3.zero)
            {
                ghostDirection = clockwiseDirection;
                tweener.AddTween(gameObject.transform, gameObject.transform.position, ghostDirection);
                UpdateArray(GetDirectionFromPosition(ghostDirection));
                SetAnimation(ghostDirection);

                string currentDirection = GetDirectionFromPosition(ghostDirection);
                lastDirection = GetOppositeDirection(currentDirection);
            }
        }
        if (Vector3.Distance(gameObject.transform.position, ghostDirection) < 0.1f)
        {
            tweener.activeTween = null;
            currentPos[0] = targetPos[0];
            currentPos[1] = targetPos[1];
        }
    }

    Vector3 GetClockwiseDirection(List<Vector3> validDirections)
    {
        string[] clockwiseOrder = { "RIGHT", "DOWN", "LEFT", "UP" };

        foreach (string preferredDirection in clockwiseOrder)
        {
            Vector3 matchingDirection = validDirections.Find(dir =>
                GetDirectionFromPosition(dir) == preferredDirection);

            if (matchingDirection != Vector3.zero)
            {
                return matchingDirection;
            }
        }
        return validDirections.Count > 0 ? validDirections[Random.Range(0, validDirections.Count)] : Vector3.zero;
    }

    void Ghost3Movement()
    {
        if (tweener.activeTween == null)
        {
            List<Vector3> validDirections = GetValidDirections(gameObject.transform.position);
            int randomInt = Random.Range(0, validDirections.Count - 1);
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
        animator.SetBool("Walking", true);
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
            print("UP");
            validDirections.Add(upPosition);
        }
        if (IsValidPosition("DOWN") && (allowBacktracking || lastDirection != "DOWN") && !positionInCentre(downPosition))
        {
            print("DOWN");

            validDirections.Add(downPosition);
        }
        if (IsValidPosition("LEFT") && (allowBacktracking || lastDirection != "LEFT") && !positionInCentre(leftPosition))
        {
            print("LEFT");

            validDirections.Add(leftPosition);
        }

        if (IsValidPosition("RIGHT") && (allowBacktracking || lastDirection != "RIGHT") && !positionInCentre(rightPosition))
        {
            print("RIGHT");
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

    //void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.CompareTag("Tunnel"))
    //    {
    //        allowBacktracking = true;
    //    }
    //}


    //    Place all four ghosts in the center area of the map.
    //● Create a new script called “GhostController.cs” to handle ghost movement.
    //● This should work in the exact same way as the player movement, except
    //instead of waiting for player input, at the end of each lerp each ghost should
    //make a decision where to move next based on the following:
    //o No Unity Pathfinding: You are NOT allowed to use the Unity
    //Pathfinding tools.
    //o No stopping: In all cases below, the ghosts never stop moving.
    //o No backstep: In all cases below, ghosts cannot move back to a grid
    //position that they just came from (i.e.they cannot reverse direction)
    //unless there is no other choice.
    //o No walking through walls: The ghost should only move to “valid”
    //positions, such as those that PacStudent can walk on(e.g.the
    //corridors).
    //o Ghost cannot teleport: In all cases below, ghosts cannot use the
    //teleporters.
    //o Dead state ghosts: Move in a straight line, at a constant, frame-rate
    //independent speed, towards the ghost spawn area.
    //▪ Move through walls and ignore all collisions with PacStudent
    //o Ghosts respawning: If a ghost is in the spawn area and they are in a…
    //▪ Dead state:
    //● Reset the ghost to Walking, Scared, or Recovering to
    //match the other ghosts.
    //● If no other ghosts are in the Dead state, change the
    //background music to match the new state.
    //▪ Walking/Scared/Recovering state: Move directly to leave the
    //spawn area out (one) of the gaps in the spawn area walls.
    //▪ After exiting the spawn area, ghosts cannot re-enter unless they
    //are in their Dead state.
    //o Walking state ghosts: Match the below ghost behaviors with the
    //number above each ghost’s head(see in-game HUD – world space
    //section above).
    //▪ Ghost 1: Move in a random valid direction such that PacStudent
    //will be either further than or equal distance to the ghost
    //(straight-line distance) when compared to the ghost’s current
    //position.
    //▪ Ghost 2: Move in a random valid direction such that PacStudent
    //will be either closer than or equal distance to the ghost
    //(straight-line distance) when compared to the ghost’s current
    //position.
    //▪ Ghost 3: Move in a randomly selected valid direction
    //▪ Ghost 4: Move clockwise around the map, following the outside
    //wall
    //o Scared and Recovering state ghosts: All ghosts use Ghost 1 behavior
}
