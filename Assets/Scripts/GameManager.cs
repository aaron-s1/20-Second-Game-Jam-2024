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
        score.text = ReturnScore().ToString();
    }

    IEnumerator SecondsCounter(float secondsPassed)
    {
        if (int.Parse(secondsPassedDisplay.text) >= 20)
        {
            EndGame();
            yield break;
        }
        
        yield return new WaitForSeconds(1f);
        secondsPassedDisplay.text = (int.Parse(secondsPassedDisplay.text) - 1).ToString();


    }


    public void AdjustingScore(int adjustment) =>
        score.text = (ReturnScore() + adjustment).ToString();

    public int ReturnScore() =>
        int.Parse(score.text);    
}
