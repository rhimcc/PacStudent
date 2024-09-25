using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    int spriteSize = 8;
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

    public GameObject outerCorner;
    public GameObject outerWall;
    public GameObject innerCorner;
    public GameObject innerWall;
    public GameObject bonus;
    public GameObject pellet;
    public GameObject tJunction;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        var existingMap = GameObject.Find("Level 01");
        GameObject.Destroy(existingMap);

        for (int row = 0; row < levelMap.GetLength(0); row++)
        {
            print("row: " + row);
            for (int col = 0; col < levelMap.GetLength(1); col++)
            {
                print("col: " + col);

                createElement(row, col);
                print(count);
            }
        }
    }

    void createElement(int row, int col)
    {
        count++;
        int x = (levelMap.GetLength(0) - col) * spriteSize;
        int y = (levelMap.GetLength(1) - row) * spriteSize;


        Vector3 position = new Vector3(-x, y, 0);
        int elementInt = levelMap[row, col];
        Debug.Log($"Instantiating {elementInt} at ({-x}, {y})");
        GameObject newObj = null;

        switch (elementInt)
        {
            case 0:
                break;
            case 1:
                newObj = Instantiate(outerCorner);
                break;
            case 2:
                newObj = Instantiate(outerWall);
                break;
            case 3:
                newObj = Instantiate(innerCorner);
                break;
            case 4:
                newObj = Instantiate(innerWall);
                break;
            case 5:
                newObj = Instantiate(pellet);
                break;
            case 6:
                newObj = Instantiate(bonus);
                break;
            case 7:
                newObj = Instantiate(tJunction);
                break;
      
            default:
                break;

        }
        if (newObj == null)
        {
            Debug.LogError($"Prefab for element {elementInt} is not assigned.");
            return; // Exit the method early
        }
        newObj.transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
}
/* Determine the position of each piece
● Determine a way for your code to decide which rotation angle each of
the wall and corner pieces should be at to align with each other.Hint:
have you code look at the pieces around it to determine how it should
be rotated to connect to them (in the case of walls)
● Mirror the above 2D array or instantiated pieces three times
(horizontally, vertically, and horizontally-and-vertically) to get the other
three quadrants of the level to make the full level.
o For the vertical mirroring, you will need to ignore or delete the
bottom row of the 2D array so that there is only a single row of
empty spots.
● Adjust the game camera to react to the size of the level such that in
Play mode the entire level layout can be seen. */
