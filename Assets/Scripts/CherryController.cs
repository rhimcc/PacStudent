using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    //    ● Create a script called “CherryController.cs” to implement the spawning and
    //movement of the bonus cherry sprite created in the previous assessment.
    //o The bonus cherry should spawn once every 10 seconds.
    //o It should be instantiated at a random location just outside of the
    //camera view on any side of the level.The starting location should be
    //different every time.
    //o It should move in a straight line, via linear lerping, from one side of the
    //screen to the other, passing through the center point of the level and
    //ignoring collisions with walls, ghosts, and pellets.
    //o If the cherry reaches the other side of the level, outside of camera
    //view, destroy it.
    //o See below for what to do if PacStudent collides with the cherry.
    // Start is called before the first frame update
    float time;
    bool newCherry = false;
    Camera cam;
    float height;
    float width;
    public GameObject cherryPrefab;
    public Tweener tweener;
    GameObject currentCherry;
    Vector3 endPoint;


    void Start()
    {
         cam = Camera.main;
         height = 2f * cam.orthographicSize;
         width = height * cam.aspect;
        time = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (((int) time) % 10 == 0) {
            if (!newCherry)
            {
                Vector3 randomStart;
                int randomDirection = Random.Range(0, 4);

                newCherry = true;
                randomStart = getRandomStart(randomDirection);
                endPoint = getEndPoint(randomStart, randomDirection);
                print(endPoint);
                currentCherry = Instantiate(cherryPrefab, randomStart, Quaternion.identity);
                tweener = currentCherry.GetComponent<Tweener>();
                tweener.speed = width / 8;
                tweener.AddTween(currentCherry.transform, randomStart, endPoint);
            }
        } else
        {
            newCherry = false;
        }
        if (currentCherry != null && currentCherry.transform.position == endPoint)
        {
            Destroy(currentCherry);
        }
    }



Vector3 getRandomStart(int randomStart)
    {
        // Generate a random start location just outside the camera view
        float startX = 0;
        float startY = 0;

        switch (randomStart)
        {
            case 0: // Top
                startX = Random.Range(-108, 108);
                startY = height/2;
                break;
            case 1: // Right
                startX = width/2;
                startY = Random.Range(-108, 116);
                break;
            case 2: // Bottom
                startX = Random.Range(-108, 108);
                startY = -height/2;
                break;
            case 3: // Left
                startX = -width/2;
                startY = Random.Range(-108, 116);
                break;
        }
    return new Vector3(startX, startY);
    }

    Vector3 getEndPoint(Vector3 randomStart, int randomDirection)
    {
        print(width);
        print(randomStart);
        switch (randomDirection)
        {
            case 0: // Top
                return new Vector3( -randomStart.x, -height / 2 - 10f); // Moving to the bottom
            case 1: // Right
                return new Vector3(-width / 2 - 10f, -randomStart.y + 4); // Moving to the left
            case 2: // Bottom
                return new Vector3(-randomStart.x, height / 2 + 10f); // Moving to the top
            case 3: // Left
                return new Vector3(width / 2 + 10f, -randomStart.y + 4); // Moving to the right
        }
        return Vector3.zero; // Default case
    }

    bool isInCameraView(Vector3 position)
    {

        return position.x >= -width / 2 && position.x <= width / 2 &&
               position.y >= -height / 2 && position.y <= height / 2;
    }
}


