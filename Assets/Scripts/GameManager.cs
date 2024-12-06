using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.U2D.IK;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool spawnsAllowed = true;

    int finalAge;
    [SerializeField] TextMeshProUGUI score;

    TextMeshProUGUI secondsPassedDisplay;
    float secondsDisplay;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void StartGame()
    {
        StartCoroutine(SecondsCounter(secondsDisplay));
        // begin 20 second counter
    }
    
    void StopGame()
    {

    }

    void EndGame()
    {
        finalAge = ReturnScore();
    }

    IEnumerator SecondsCounter(float secondsPassed)
    {
        float timeCount = 0;
        while (timeCount < 20f)
        {
            timeCount += Time.deltaTime;
            secondsPassedDisplay.text = timeCount.ToString("F2");
        }

        EndGame();
        yield break;
    }


    public void AdjustingScore(int adjustment) =>
        score.text = (ReturnScore() + adjustment).ToString();

    public int ReturnScore() =>
        int.Parse(score.text);    
}
