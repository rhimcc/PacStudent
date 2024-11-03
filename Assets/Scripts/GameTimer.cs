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
    public bool isRunning = false;
    int mins;
    int hours;
    int seconds;
    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            timer = Time.timeSinceLevelLoad - 4;
            UpdateTimerDisplay();
        }
    }

    public IEnumerator StartCountDown()
    {
        isRunning = false;
        pacman = GameObject.Find("PacMan");
        pacStudentController = pacman.GetComponent<PacStudentController>();
        GameObject ghost1 = GameObject.Find("Ghost1");
        GameObject ghost2 = GameObject.Find("Ghost2");
        GameObject ghost3 = GameObject.Find("Ghost3");
        GameObject ghost4 = GameObject.Find("Ghost4");

        GhostController ghost1Movement = ghost1.GetComponent<GhostController>();
        GhostController ghost2Movement = ghost2.GetComponent<GhostController>();
        GhostController ghost3Movement = ghost3.GetComponent<GhostController>();
        GhostController ghost4Movement = ghost4.GetComponent<GhostController>();

        countdownValue = 0;
        countdown = GameObject.Find("Countdown");
        countdownTextObject = countdown.GetComponent<Text>();

        while (countdownValue < countdownValues.Length)
        {
            countdownTextObject.text = countdownValues[countdownValue];
            countdownValue++;
            yield return new WaitForSeconds(1.0f);
        }
        pacStudentController.movementAllowed = true;
        ghost1Movement.movementAllowed = true;
        ghost2Movement.movementAllowed = true;
        ghost3Movement.movementAllowed = true;
        ghost4Movement.movementAllowed = true;

        countdown.SetActive(false);
        isRunning = true;
    }

    public void UpdateTimerDisplay()
    {
        GameObject timerObject = GameObject.Find("Time");
        Text timerText = timerObject.GetComponent<Text>();
        timerText.text = FormatTimer();
    }

    public string FormatTimer()
    {
        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);
        int milliseconds = (int)((timer * 100) % 100);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
    }

    public void StopTime()
    {
        isRunning = false;
    }

}


