 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Level2Controller : MonoBehaviour
{
    string[] effects = new string[] { "Super Speed", "Shield", "Reverse controls", "Snail Speed", "Increased Pellet Range", "No Pellets" };
    GameObject luckyBlock;
    bool blockActive = false;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (!blockActive)
        {
            time += Time.deltaTime;
            if ((int)time % 20 == 0)
            {
                luckyBlock.SetActive(true);
                blockActive = true;
            }
        }
    }

    void GenerateEffect()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "LuckyBlock")
        {
            print(effects[Random.Range(0, effects.Length)]);
        }
    }
}

