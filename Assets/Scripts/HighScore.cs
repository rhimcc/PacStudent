using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    GameObject HighScoreObject;
    Text HighScoreText;
    GameObject TimeObject;
    Text TimeText;

    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveScore(int score, string time)
    {
        int currentHighScore = PlayerPrefs.GetInt("Highscore");
        string currentTime = PlayerPrefs.GetString("Time");
        
        if (score > currentHighScore || (score == currentHighScore && timeToInt(currentTime) < timeToInt(time)))
        {
            PlayerPrefs.SetInt("Highscore", score);
            PlayerPrefs.SetString("Time", time);
            SetText();

        }
    }

    private int timeToInt(string time)
    {
        int minutes = int.Parse(time.Substring(0, 2));
        int seconds = int.Parse(time.Substring(3, 2));
        int milliseconds = int.Parse(time.Substring(6, 2));
        return minutes * 60 * 100 + seconds * 100 + milliseconds;
    }

    private void SetText()
    {
        HighScoreObject = GameObject.Find("Score");
        HighScoreText = HighScoreObject.GetComponent<Text>();
        HighScoreText.text = "" + PlayerPrefs.GetInt("Highscore");
        TimeObject = GameObject.Find("Time");
        TimeText = TimeObject.GetComponent<Text>();
        TimeText.text = "" + PlayerPrefs.GetString("Time");
    }
}
