using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    GameObject countdown;
    Text countdownTextObject;
    int countdownValue;
    string[] countdownValues = new string[] {"3", "2", "1", "GO!"};
    GameObject pacman;
    public PacStudentController pacStudentController;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartCountDown()
    {
        pacman = GameObject.Find("PacMan");
        pacStudentController = pacman.GetComponent<PacStudentController>();
        pacStudentController.enabled = false;
        countdownValue = 0;
        countdown = GameObject.Find("Countdown");
        countdownTextObject = countdown.GetComponent<Text>();
        while (countdownValue < countdownValues.Length)
        {
            print(countdownValues[countdownValue]);
            countdownTextObject.text = countdownValues[countdownValue];
            countdownValue++;
            yield return new WaitForSeconds(1.0f);
        }

        pacStudentController.enabled = true; // Enable the movement script
        countdown.SetActive(false);
    }

    //    o At the start of the round, on the HUD canvas, show a big countdown of
    //“3”, “2”, “1”, “GO!” (each displayed 1 second apart).
    //o During this time, the Game Timer(below) should remain at 0 and the
    //player and the ghosts should not be able to move(see previous and
    //later sections).
    //o After “GO!” has been shown for 1 second:
    //▪ Hide this UI element and start the game.
    //▪ Enable player control and ghost movement (if you complete the
    //90% section below)
    //▪ Start the background music for when ghosts are in their
    //Walking state, which should loop if it finishes.
}
