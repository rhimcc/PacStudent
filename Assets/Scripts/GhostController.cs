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

    bool[] horizontalBorders = new bool[4];
    bool[] verticalBorders = new bool[4];
    int[,] currentPos = new int[,] { { 14, 13 }, { 14, 13 }, { 14, 13 }, { 14, 13 } };
    int[,] targetPos = new int[,] { { 12, 13 }, { 12, 13 }, { 12, 13 }, { 12, 13 } };
    public Tweener[] tweeners;
    public GameObject[] ghosts = new GameObject[4];
    public bool movementAllowed = false;
    GameObject pacman;
    public Animator[] animators;
    private string[] lastDirections = new string[4] { "", "", "", "" };
    private Vector3[] ghostDirections = new Vector3[4];
    private bool moveGhostsFromCentre = true;


    // Start is called before the first frame update
    void Start()
    {
        pacman = GameObject.Find("PacMan");
        for (int i = 0; i < 4; i++)
        {
            ghostDirections[i] = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (movementAllowed)
        {
            if (moveGhostsFromCentre) {
                MoveGhostsFromCentre();
            } else
            {
                Ghost1Movement(0);
                Ghost2Movement(1);
                Ghost3Movement(2);
                Ghost4Movement(3);
            }
        }
    }


    void MoveGhostsFromCentre()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 targetPosition = new Vector3(-4, 28, 0);
            if (tweeners[i].activeTween == null)
            {
                tweeners[i].AddTween(ghosts[i].transform, ghosts[i].transform.position, targetPosition);
            }
            if (Vector3.Distance(ghosts[i].transform.position, targetPosition) < 0.1f)
            {
                tweeners[i].activeTween = null;
                currentPos[i, 0] = targetPos[i, 0] - 1;
                currentPos[i, 1] = targetPos[i, 1];
                moveGhostsFromCentre = false;
            }
            animators[i].SetBool("Right", false);
            animators[i].SetBool("Up", true);
        }
    }


    void Ghost4Movement(int ghost)
    {
        if (tweeners[ghost].activeTween == null)
        {
            List<Vector3> validDirections = GetValidDirections(ghosts[ghost].transform.position, ghost);
            int randomInt = Random.Range(0, validDirections.Count);
            ghostDirections[ghost] = validDirections[randomInt];
            tweeners[ghost].AddTween(ghosts[ghost].transform, ghosts[ghost].transform.position, ghostDirections[ghost]);
            UpdateArray(ghost, GetDirectionFromPosition(ghost, ghostDirections[ghost]));
            SetAnimation(ghost, ghostDirections[ghost]);

            string currentDirection = GetDirectionFromPosition(ghost, ghostDirections[ghost]);
            lastDirections[ghost] = GetOppositeDirection(currentDirection);
        }
        if (Vector3.Distance(ghosts[ghost].transform.position, ghostDirections[ghost]) < 0.1f)
        {
            tweeners[ghost].activeTween = null;
            currentPos[ghost, 0] = targetPos[ghost, 0];
            currentPos[ghost, 1] = targetPos[ghost, 1];
        }
    }

    void Ghost3Movement(int ghost)
    {
        if (tweeners[ghost].activeTween == null)
        {
            List<Vector3> validDirections = GetValidDirections(ghosts[ghost].transform.position, ghost);
            int randomInt = Random.Range(0, validDirections.Count);
            ghostDirections[ghost] = validDirections[randomInt];
            tweeners[ghost].AddTween(ghosts[ghost].transform, ghosts[ghost].transform.position, ghostDirections[ghost]);
            UpdateArray(ghost, GetDirectionFromPosition(ghost, ghostDirections[ghost]));
            SetAnimation(ghost, ghostDirections[ghost]);

            string currentDirection = GetDirectionFromPosition(ghost, ghostDirections[ghost]);
            lastDirections[ghost] = GetOppositeDirection(currentDirection);
        }
        if (Vector3.Distance(ghosts[ghost].transform.position, ghostDirections[ghost]) < 0.1f)
        {
            tweeners[ghost].activeTween = null;
            currentPos[ghost, 0] = targetPos[ghost, 0];
            currentPos[ghost, 1] = targetPos[ghost, 1];
        }
    }

    void Ghost1Movement(int ghost)
    {
        if (tweeners[ghost].activeTween == null)
        {
            List<Vector3> validDirections = GetValidDirections(ghosts[ghost].transform.position, ghost);
            ghostDirections[ghost] = GetLargestDistance(validDirections);
            tweeners[ghost].AddTween(ghosts[ghost].transform, ghosts[ghost].transform.position, ghostDirections[ghost]);
            UpdateArray(ghost, GetDirectionFromPosition(ghost, ghostDirections[ghost]));
            SetAnimation(ghost, ghostDirections[ghost]);

            string currentDirection = GetDirectionFromPosition(ghost, ghostDirections[ghost]);
            lastDirections[ghost] = GetOppositeDirection(currentDirection);
        }
        if (Vector3.Distance(ghosts[ghost].transform.position, ghostDirections[ghost]) < 0.1f)
        {
            tweeners[ghost].activeTween = null;
            currentPos[ghost, 0] = targetPos[ghost, 0];
            currentPos[ghost, 1] = targetPos[ghost, 1];
        }
       
    }

    void Ghost2Movement(int ghost)
    {
        if (tweeners[ghost].activeTween == null)
        {
            List<Vector3> validDirections = GetValidDirections(ghosts[ghost].transform.position, ghost);
            ghostDirections[ghost] = GetSmallestDistance(validDirections);
            tweeners[ghost].AddTween(ghosts[ghost].transform, ghosts[ghost].transform.position, ghostDirections[ghost]);
            UpdateArray(ghost, GetDirectionFromPosition(ghost, ghostDirections[ghost]));
            SetAnimation(ghost, ghostDirections[ghost]);

            string currentDirection = GetDirectionFromPosition(ghost, ghostDirections[ghost]);
            lastDirections[ghost] = GetOppositeDirection(currentDirection);
        }
        if (Vector3.Distance(ghosts[ghost].transform.position, ghostDirections[ghost]) < 0.1f)
        {
            tweeners[ghost].activeTween = null;
            currentPos[ghost, 0] = targetPos[ghost, 0];
            currentPos[ghost, 1] = targetPos[ghost, 1];
        }
    }

    void UpdateArray(int ghost, string direction)
    {
        int currentX = currentPos[ghost, 0];
        int currentY = currentPos[ghost, 1];
        int lastXindex = levelMap.GetLength(1) - 1;
        int lastYindex = levelMap.GetLength(0) - 1;
        int quadrant = DetectQuadrant(ghosts[ghost].transform, direction);

        horizontalBorders[ghost] = false;
        verticalBorders[ghost] = false;
        DetectBorder(ghost, direction);

        switch (direction)
        {
            case "UP":
                targetPos[ghost, 1] = currentPos[ghost, 1];
                if (verticalBorders[ghost])
                {
                    if (quadrant == 1 || quadrant == 2)
                    {
                        targetPos[ghost, 0] = lastYindex;
                    }
                    else
                    {
                        targetPos[ghost, 0] = lastYindex - 1;
                    }

                }
                else if (quadrant == 1 || quadrant == 2)
                {
                    targetPos[ghost, 0] = currentPos[ghost, 0] - 1;
                }
                else
                {
                    targetPos[ghost, 0] = currentPos[ghost, 0] + 1;
                }
                break;

            case "DOWN":
                targetPos[ghost, 1] = currentPos[ghost, 1];
                if (verticalBorders[ghost])
                {
                    if (quadrant == 1 || quadrant == 2)
                    {
                        targetPos[ghost, 0] = lastYindex;
                    }
                    else
                    {
                        targetPos[ghost, 0] = lastYindex - 1;
                    }
                }
                else
                if (quadrant == 1 || quadrant == 2)
                {
                    targetPos[ghost, 0] = currentPos[ghost, 0] + 1;
                }
                else
                {
                    targetPos[ghost, 0] = currentPos[ghost, 0] - 1;
                }
                break;

            case "LEFT":
                targetPos[ghost, 0] = currentPos[ghost, 0];
                if (horizontalBorders[ghost])
                {
                    targetPos[ghost, 1] = lastXindex;
                }
                else
            if (quadrant == 1 || quadrant == 4)
                {
                    targetPos[ghost, 1] = currentPos[ghost, 1] - 1;
                }
                else
                {
                    targetPos[ghost, 1] = currentPos[ghost, 1] + 1;
                }
                break;

            case "RIGHT":
                targetPos[ghost, 0] = currentPos[ghost, 0];
                if (horizontalBorders[ghost])
                {
                    targetPos[ghost, 1] = lastXindex;
                }
                else
                if (quadrant == 1 || quadrant == 4)
                {
                    targetPos[ghost, 1] = currentPos[ghost, 1] + 1;
                }
                else
                {
                    targetPos[ghost, 1] = currentPos[ghost, 1] - 1;
                }
                break;
        }
    }

    void SetAnimation(int ghost, Vector3 direction)
    {
        animators[ghost].SetBool("Walking", true);
        animators[ghost].SetBool("Left", false);
        animators[ghost].SetBool("Right", false);
        animators[ghost].SetBool("Up", false);
        animators[ghost].SetBool("Down", false);
        switch(GetDirectionFromPosition(ghost, direction))
        {
            case "RIGHT":
                animators[ghost].SetBool("Right", true);
                break;
            case "LEFT":
                animators[ghost].SetBool("Left", true);
                break;
            case "UP":
                animators[ghost].SetBool("Up", true);
                break;
            case "DOWN":
                animators[ghost].SetBool("Down", true);
                break;
        }

    }

    string GetDirectionFromPosition(int ghost, Vector3 direction)
    {
        Vector3 ghostPosition = ghosts[ghost].transform.position;
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


    float CalculateDistance(Vector3 direction, GameObject ghost)
    {
        return Vector3.Distance(direction, ghost.transform.position);
    }

    bool positionInCentre(Vector3 position)
    {
        return position.x > -33 && position.x < 23 && position.y > -12 && position.y < 20;
    }


    List<Vector3> GetValidDirections(Vector3 position, int ghost, bool allowBacktracking = false)
    {
        List<Vector3> validDirections = new List<Vector3>();
        if (IsValidPosition("UP", ghost) && (allowBacktracking || lastDirections[ghost] != "UP") && !positionInCentre(position))
        {
            validDirections.Add(position + new Vector3(0, 8, 0));
        }

        if (IsValidPosition("DOWN", ghost) && (allowBacktracking || lastDirections[ghost] != "DOWN") && !positionInCentre(position))
        {
            validDirections.Add(position + new Vector3(0, -8, 0));
        }
        if (IsValidPosition("LEFT", ghost) && (allowBacktracking || lastDirections[ghost] != "LEFT") && !positionInCentre(position))
        {
            validDirections.Add(position + new Vector3(-8, 0, 0));
        }

        if (IsValidPosition("RIGHT", ghost) && (allowBacktracking || lastDirections[ghost] != "RIGHT") && !positionInCentre(position))
        {
            validDirections.Add(position + new Vector3(8, 0, 0));
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


    bool IsValidPosition(string direction, int ghost)
    {
        int currentX = currentPos[ghost, 0];
        int currentY = currentPos[ghost, 1];
        int[] targetPos = new int[] { currentX, currentY };
        int lastXindex = levelMap.GetLength(1) - 1;
        int lastYindex = levelMap.GetLength(0) - 1;
        int quadrant = DetectQuadrant(ghosts[ghost].transform, direction);

        horizontalBorders[ghost] = false;
        verticalBorders[ghost] = false;
        DetectBorder(ghost, direction);

        switch (direction)
        {
            case "UP":
                targetPos[1] = currentY;
                if (verticalBorders[ghost])
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
                if (verticalBorders[ghost])
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
                if (horizontalBorders[ghost])
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
                if (horizontalBorders[ghost])
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
        if (targetPos[0] >= 0 && targetPos[0] < levelMap.GetLength(0) &&
              targetPos[1] >= 0 && targetPos[1] < levelMap.GetLength(1))
        {
            int targetTile = levelMap[targetPos[0], targetPos[1]];
            return (targetTile == 5 || targetTile == 6 || targetTile == 0);
        }

        return false;
    }

    void DetectBorder(int ghost, string direction)
    {
        Vector3 position = ghosts[ghost].transform.position;
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
            horizontalBorders[ghost] = true;
        }
        if (position.x > 108 || position.x < -108)
        {
            horizontalBorders[ghost] = true;
        }

        if (position.y >= 0 && position.y <= 8)
        {
            verticalBorders[ghost] = true;
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
