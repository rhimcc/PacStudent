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
    int[,] currentPos = new int[,] { { 13, 14 }, { 13, 14 }, { 13, 14 }, { 13, 14 } };
    int[,] targetPos = new int[,] { { 13, 13 }, { 13, 13 }, { 13, 13 }, { 13, 13 } };
    public Tweener[] tweeners;
    public GameObject[] ghosts = new GameObject[4];


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Ghost1Movement();
        //Ghost2Movement();
        //Ghost3Movement();
        Ghost4Movement();
    }

    void Ghost4Movement()
    {

    }

    void Ghost1Movement()
    {
        if (tweeners[0].activeTween == null)
        {
            List<Vector3> validDirections = GetValidDirections(ghosts[0].transform.position, 0);
            int randomInt = Random.Range(0, validDirections.Count);
            print(randomInt);
            Vector3 direction = validDirections[randomInt];
        }
    }

    List<Vector3> GetValidDirections(Vector3 position, int ghost)
    {
        List<Vector3> validDirections = new List<Vector3>();
        if (IsValidPosition("UP", ghost)) validDirections.Add(position + new Vector3(0, 8, 0));
        if (IsValidPosition("DOWN", ghost)) validDirections.Add(position + new Vector3(0, -8, 0));
        if (IsValidPosition("LEFT", ghost)) validDirections.Add(position + new Vector3(-8, 0, 0));
        if (IsValidPosition("RIGHT", ghost)) validDirections.Add(position + new Vector3(8, 0, 0));
        return validDirections;
    }

    bool IsValidPosition(string direction, int ghost)
    {
        targetPos[ghost, 0] = currentPos[ghost, 0];
        targetPos[ghost, 1] = currentPos[ghost, 1];
        int lastXindex = levelMap.GetLength(1) - 1;
        int lastYindex = levelMap.GetLength(0) - 1;
        int quadrant = DetectQuadrant(ghosts[ghost].transform, direction);

        horizontalBorder = false;
        verticalBorder = false;
        DetectBorder(ghosts[ghost].transform, direction);

        switch (direction)
        {
            case "UP":
                targetPos[ghost, 1] = currentPos[ghost, 1];
                if (verticalBorder)
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
                if (verticalBorder)
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
                if (horizontalBorder)
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
                if (horizontalBorder)
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
        if (targetPos[ghost, 0] >= 0 && targetPos[ghost, 0] < levelMap.GetLength(0) &&
              targetPos[ghost, 1] >= 0 && targetPos[ghost, 1] < levelMap.GetLength(1))
        {

            int targetTile = levelMap[targetPos[ghost, 0], targetPos[ghost, 1]];
            return (targetTile == 5 || targetTile == 6 || targetTile == 0);
        }

        return false;
    }

    void DetectBorder(Transform transform, string direction)
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
