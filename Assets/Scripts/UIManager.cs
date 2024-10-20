using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadLevel1()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
    }

    public void loadStart()
    {
        SceneManager.LoadScene(0);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1) // Check if the loaded scene is Level 1
        {
            GameObject buttonObject = GameObject.FindGameObjectWithTag("QuitButton");
            if (buttonObject != null) // Ensure the button exists
            {
                Button button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(loadStart); // Add listener
            }
        }
    }
}
