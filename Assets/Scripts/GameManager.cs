using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.U2D.IK;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool spawnsAllowed = true;

    float finalAgeNumber;
    [SerializeField] TextMeshProUGUI score;

    [SerializeField] TextMeshProUGUI secondsPassedDisplay;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
            Destroy(gameObject);
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }    

    public void StartingGame()
    {
        // Debug.Log("game manager hit StartGame()");
        StartCoroutine(SecondsCounter());
    }
    
    void StopGame()
    {

    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        
    }

    public bool gameIsOver;
    [SerializeField] GameObject restartScreen;

    void EndGame()
    {
        gameIsOver = true;
        BobMovement.instance.KillMovement();

        StartGame.instance.secondsCountUI.SetActive(false);
        restartScreen.SetActive(true);

        finalAgeNumber = ReturnScore();
        score.text = ReturnScore().ToString();
    }

    IEnumerator SecondsCounter()
    {
        if (int.Parse(secondsPassedDisplay.text) <= 0)
        {
            EndGame();
            yield break;
        }
                
        yield return new WaitForSeconds(1f);

        // Debug.Log("start result = " + secondsPassedDisplay.text);
        int newSeconds = int.Parse(secondsPassedDisplay.text) - 1;
        // Debug.Log("adjusted = " + newSeconds);
        secondsPassedDisplay.text = newSeconds.ToString();
        // Debug.Log("end result = " + secondsPassedDisplay.text);

        StartCoroutine(SecondsCounter());
    }


    public void AdjustingScore(float adjustment) =>
        score.text = (ReturnScore() + adjustment).ToString();

    public float ReturnScore() =>
        float.Parse(score.text);


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // if (SpawnFood.instance == null)
            // SpawnFood.instance = null;

        // StartGame.instance = null;
        // BobMovement.instance = null;
        if (instance == null)
            instance = this;
        // instance = null;
    }        
}
