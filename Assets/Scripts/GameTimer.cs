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
    bool isRunning = false;
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
            timer += Time.deltaTime; // Increase timer by the time since the last frame
            UpdateTimerDisplay();
        }
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
}
