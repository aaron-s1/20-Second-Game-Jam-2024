using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class AdjustScore : MonoBehaviour
{
    // public static AdjustScore instance;

    // void Awake()
    // {
    //     if (instance == null)
    //         instance = this;
    //     else if (instance != this)
    //         Destroy(gameObject);
    // }


    TextMeshProUGUI score;

    
    void Start()
    {
        score = GameObject.Find("age count").GetComponent<TextMeshProUGUI>();        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            int adjustment = 1;

            if (gameObject.name == "a bad food")
                adjustment *= -1;

            // GameManager.instance.AdjustingScore(adjustment);
            ChangeScore(adjustment);
        }
    }
    
    public void ChangeScore(int adjustment) =>
        score.text = (ReturnScore() + adjustment).ToString();

    public int ReturnScore() =>
        int.Parse(score.text);
}
