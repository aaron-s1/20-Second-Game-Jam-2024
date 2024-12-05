using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int finalAge;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void StartGame()
    {
        
    }

    void EndGame()
    {
        // finalAge = AdjustScore.instance.ReturnScore();
    }
}
