 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Level2Controller : MonoBehaviour
{
    string[] positiveEffects = new string[] { "Super Speed", "Shield", "Increased Pellet Range" };
    string[] negativeEffects = new string[] { "Reverse controls", "Snail Speed", "No Pellets" };
    public GameObject luckyBlock;
    bool blockActive = false;
    float time = 0;
    float effectTime = 0;

    public string currentEffect = "No Effect";
    float randomTime = 0.0f;
    public Text effectText;
    public Text effectTimeText;


    // Start is called before the first frame update
    void Start()
    {
        

    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if ((int)time == 20)
        {
            luckyBlock.SetActive(true);
        }

        if (currentEffect !=  "No Effect") // if an effect is active
        {
            
            effectTime += Time.deltaTime;
            float timeLeft = randomTime - effectTime;
            effectTimeText.text = timeLeft.ToString("F2");
            if (timeLeft < 0)
            {
                currentEffect = "No Effect";
                effectTimeText.text = "";
            }
        }
        if (ArrayContains(positiveEffects, currentEffect))
        {
            effectText.color = Color.green;
            effectTimeText.color = Color.green;
        }
        else if (ArrayContains(negativeEffects, currentEffect))
        {
            effectText.color = Color.red;
            effectTimeText.color = Color.red;
        } else
        {
            effectText.color = new Color(1f, 0.9415839f, 0.5801887f);
        }
        effectText.text = currentEffect;
    }

    void GenerateEffect()
    {
        int effectStatus = Random.Range(0, 2);
        if (effectStatus == 0)
        {
            currentEffect = positiveEffects[Random.Range(0, positiveEffects.Length)];
        }
        else
        {
            currentEffect = negativeEffects[Random.Range(0, negativeEffects.Length)];
        }
        randomTime = Random.Range(5, 10);
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "LuckyBlock")
        {
            if (currentEffect == "No Effect")
            {
                GenerateEffect();
            }
            time = 0;
            effectTime = 0;
            luckyBlock.SetActive(false);
            blockActive = false;
        }
    }

    bool ArrayContains(string[] array, string value)
    {
        foreach (string item in array)
        {
            if (item == value)
            {
                return true;
            }
        }
        return false;
    }
}

