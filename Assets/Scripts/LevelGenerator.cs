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
    private int rowCount = 0;
    private int colCount = 0;
    private string[] directions = { "left", "right", "up", "down" };

    // Start is called before the first frame update
    void Start()
    {
        rowCount = levelMap.GetLength(0);
        colCount = levelMap.GetLength(1);
        var existingMap = GameObject.Find("LevelMap");
        GameObject.Destroy(existingMap);

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                createElement(row, col);
            }
        }
    }

    void createElement(int row, int col)
    {
        int x = (colCount - col) * spriteSize - 4;
        int y = (rowCount - row) * spriteSize - 4;
        Vector3 position = new Vector3(-x, y, 0);
        int elementInt = levelMap[row, col];
        GameObject newObj = null;

        switch (elementInt)
        {
            case 0:
                break;

            case 1:
                newObj = Instantiate(outerCorner);
                newObj.transform.rotation = Quaternion.Euler(0, 0, getRotation(row, col));
                break;

            case 2:
                newObj = Instantiate(outerWall);
                newObj.transform.rotation = Quaternion.Euler(0, 0, getRotation(row, col));
                break;

            case 3:
                newObj = Instantiate(innerCorner);
                newObj.transform.rotation = Quaternion.Euler(0, 0, getRotation(row, col));
                break;

            case 4:
                newObj = Instantiate(innerWall);
                newObj.transform.rotation = Quaternion.Euler(0, 0, getRotation(row, col));
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

        if (newObj != null)
        {
            newObj.transform.position = position;
        }
    }

    int getRotation(int row, int col)
    {
        int elementInt = levelMap[row, col];
        int left = (col > 0) ? levelMap[row, col - 1] : -1;
        int right = (col < colCount - 1) ? levelMap[row, col + 1] : -1;
        int up = (row > 0) ? levelMap[row - 1, col] : -1;
        int down = (row < rowCount - 1) ? levelMap[row + 1, col] : -1;
        print("row: " + row + ", col: " + col + ", left: " + left + ", right: " + right + ", up: " + up + ", down: " + down + ", ELEMENT: " + elementInt);

        if (elementInt == 2 || elementInt == 4) // if it is a wall
        {

            if (up == elementInt || up == elementInt - 1)
            {
                if (down == elementInt || down == elementInt - 1)
                {
                    return 90;
                }
            }
        }

        if (elementInt == 1 || elementInt == 3) // if it is a corner
        {
              
            if (left == -1 ) //if it is on the very far left
            {
                if (up == elementInt || up == elementInt + 1)
                {
                    return 90;
                } 
                if (down == elementInt || down == elementInt + 1)
                {
                    return 0;
                }
            }
            if (right == -1) // if it is on the very far right
            {
                if (up == elementInt || up == elementInt + 1)
                {
                    return 90;
                }
                if (down == elementInt || down == elementInt + 1)
                {
                    return 0;
                }
            }
            if (left == elementInt || left == elementInt + 1 && wallIsHorizontal(row, col - 1)) // if left is connected to a wall or a corner, and the connected wall is horizontal
            {
                if (up == elementInt || up == elementInt + 1)
                {
                    print("left, up");
                    return 180;
                }
                if (down == elementInt || down == elementInt + 1)
                {

                    print("left, down");
                    return 270;
                }
            }
            if (right == elementInt  || right == elementInt + 1 && wallIsHorizontal(row, col + 1)) // if the right is connected to wall or corner and the connected wall is horizontal
            {
                if (up == elementInt || up == elementInt + 1)
                {

                    print("right, up");
                    return 90;
                }
                if (down == elementInt || down == elementInt + 1)
                {

                    print("right, down");
                    return 0;
                }
            }
        }
        return 0;
    }

    bool wallIsHorizontal(int row, int col)
    {
        int elementInt = levelMap[row, col];
        int left = (col > 0) ? levelMap[row, col - 1] : -1;
        int right = (col < colCount - 1) ? levelMap[row, col + 1] : -1;
        int up = (row > 0) ? levelMap[row - 1, col] : -1;
        int down = (row < rowCount - 1) ? levelMap[row + 1, col] : -1;
        if (up == elementInt || up == elementInt - 1)
        {
            if (down == elementInt || down == elementInt - 1)
            {
                return false;
            }
        }
        return true;
    }
 
    void Update()
    {
    }
}