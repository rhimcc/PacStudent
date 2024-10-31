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
        HighScoreObject = GameObject.Find("Score");
        HighScoreText = HighScoreObject.GetComponent<Text>();
        HighScoreText.text = "" + PlayerPrefs.GetInt("Highscore");
        TimeObject = GameObject.Find("Time");
        TimeText = TimeObject.GetComponent<Text>();
        TimeText.text = "" + PlayerPrefs.GetString("Time");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveScore(int score, string time)
    {
        if (score > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", score);
            PlayerPrefs.SetString("Time", time);
        
        }
    }
}
