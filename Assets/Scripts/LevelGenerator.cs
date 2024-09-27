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
    private GameObject levelCorner;
    

    // Start is called before the first frame update
    void Start()
    {
        rowCount = levelMap.GetLength(0);
        colCount = levelMap.GetLength(1);
        var existingMap = GameObject.Find("LevelMap");
        GameObject.Destroy(existingMap);
        levelCorner = new GameObject("Corner");

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                int x = (colCount - col) * spriteSize - 4;
                int y = (rowCount - row) * spriteSize - 4;
                createElement(row, col, x, y, 1, 1);
            }
        }
        duplicateCorner(0, 0, -1, 1);
        duplicateCorner(0, spriteSize, 1, -1);
        duplicateCorner(0, spriteSize, -1, -1);
    }

    void duplicateCorner(int x, int y, int xScale, int yScale)
    {
        GameObject newCorner = Instantiate(levelCorner);
        newCorner.transform.localScale = new Vector3(xScale, yScale, 0);
        newCorner.transform.position = new Vector3(x, y, 0);
    }

    void createElement(int row, int col, int x, int y, int xScale, int yScale)
    {
   
        Vector3 position = new Vector3(-x, y, 0);
        int elementInt = levelMap[row, col];
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

        if (newObj != null)
        {
            newObj.transform.position = position;
            newObj.transform.rotation = Quaternion.Euler(0, 0, getRotation(row, col));
            newObj.transform.parent = levelCorner.transform;
            newObj.transform.localScale = new Vector3(xScale, yScale, 0);
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

            if (up == elementInt || up == elementInt - 1 || up == 7)
            {
                if (down == elementInt || down == elementInt - 1 || down == 7 || down == -1)
                {
                    return 90;
                }
            }
        }

        if (elementInt == 1 || elementInt == 3) // if it is a corner
        {
              
            if (left == -1 ) //if it is on the very far left
            {
                if (amountOfNeighbouringElements(row, col) > 2)
                {
                    return getAngleOfPellet(row, col);
                }
                else
                {
                    if (right == elementInt || right == elementInt + 1)
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
                    else
                    {
                        if (up == elementInt || up == elementInt + 1)
                        {
                            return 180;
                        }
                        if (down == elementInt || down == elementInt + 1)
                        {
                            return 270;
                        }
                    }
                }
              
            }

        
            if (right == -1) // if it is on the very far right
            {
                if (amountOfNeighbouringElements(row, col) > 2)
                {
                    return getAngleOfPellet(row, col);
                }
                else
                {
                    if (left == elementInt || left == elementInt + 1)
                    {
                        if (up == elementInt || up == elementInt + 1)
                        {
                            return 180;
                        }
                        if (down == elementInt || down == elementInt + 1)
                        {
                            return 270;
                        }
                    }
                    else
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
                }

            }

            if (right == 7 || left == 7 || up == 7 || down == 7) // Check if next to a T-junction
            {
                if (left == 7)
                {
                    if (up == 3 || up == 4)
                    {
                        return 180;
                    }
                    if (down == 3 || down == 4)
                    {
                        return 270;
                    }
                }
                if (right == 7)
                {
                    if (up == 3 || up == 4)
                    {
                        return 90;
                    }
                    if (down == 3 || down == 4)
                    {
                        return 0;
                    }
                }
              
            }


            if (amountOfNeighbouringElements(row, col) == 2)
            {
                if (left == elementInt || left == elementInt + 1 && wallIsHorizontal(row, col - 1)) // if left is connected to a wall or a corner, and the connected wall is horizontal
                {
                    if (up == elementInt || up == elementInt + 1 || up == 7)
                    {
                        return 180;
                    }
                    if (down == elementInt || down == elementInt + 1 || down == 7)
                    {
                        return 270;
                    }
                }
                if (right == elementInt || right == elementInt + 1 && wallIsHorizontal(row, col + 1)) // if the right is connected to wall or corner and the connected wall is horizontal
                {
                    if (up == elementInt || up == elementInt + 1 || up == 7)
                    {
                        return 90;
                    }
                    if (down == elementInt || down == elementInt + 1 || down == 7)
                    {
                        return 0;
                    }
                }

            }

            
            if (amountOfNeighbouringElements(row, col) > 2) {
            
                return getAngleOfPellet(row, col);
            }

            
        }

        if (elementInt == 7)
        {
            print("(" + row + ", " + col + ")" + " is tjunction");
            print("left: " + left);
            print("right: " + right);
            print("up: " + up);
            print("down: " + down);

            if (!(up == 1 || up == 2 || up == 7))
            {
                return 0;
            }
            if (!(left == 1 || left == 2 || left == 7))
            {
                return 90;
            }
            if (!(down == 1 || down == 2 || down == 7))
            {
                return 180;
            }
            if (!(right == 1 || right == 2 || right == 7))
            {
                return 0;
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

    int amountOfNeighbouringElements(int row, int col)
    {
        int amount = 0;
        int elementInt = levelMap[row, col];
        if (row + 1 < rowCount)
        {
            if (levelMap[row + 1, col] == elementInt || levelMap[row + 1, col] == elementInt + 1)
            {
                amount++;
            }
        }

        if (row - 1 > 0)
        {
            if (levelMap[row - 1, col] == elementInt || levelMap[row - 1, col] == elementInt + 1)
            {
                amount++;
            }
        }

        if (col + 1 < colCount)
        {
            if (levelMap[row, col + 1] == elementInt || levelMap[row, col + 1] == elementInt + 1)
            {
                amount++;
            }
        }
        if (col - 1 > 0)
        {
            if (levelMap[row, col - 1] == elementInt || levelMap[row, col - 1] == elementInt + 1)
            {
                amount++;
            }
        }
      
       
        print("amount of neighboring: " + amount);
        return amount;
    }

    int getAngleOfPellet(int row, int col) // is only called if a corner is surrounded by 3 or 4 elements
    {
        int row1 = row + 1;
        int col1 = col + 1;
        int row0 = row - 1;
        int col0 = col - 1;
        if (row + 1 < rowCount && col + 1 < colCount)
        {
            if (levelMap[row + 1, col + 1] == 5 || levelMap[row + 1, col + 1] == 6) // bottom right corner
            {
                print("pellet is in bottom right");
                return 0;
            }
        }

        if (row - 1 > 0 && col + 1 < colCount) {
            if (levelMap[row - 1, col + 1] == 6 || levelMap[row - 1, col + 1] == 5) // top right corner
            {

                print("pellet is in top right");
                return 90;
            }
        }
    
        if (row - 1 > 0 && col - 1 > 0)
        {
            if (levelMap[row - 1, col - 1] == 6 || levelMap[row - 1, col - 1] == 5) // top left corner
            {

                print("pellet is in top left");
                return 180;
            }
        }

        if (row + 1 < rowCount && col - 1 > 0)
        {
            if (levelMap[row + 1, col - 1] == 6 || levelMap[row + 1, col - 1] == 5) // bottom left corner
            {

                print("pellet is in bottom left");
                return 270;
            }
        }
      
        return 0;
    }
}