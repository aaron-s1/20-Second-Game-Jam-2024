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

    [SerializeField] GameObject restartScreen;    
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI secondsPassedDisplay;

    public bool spawnsAllowed = true;
    public bool gameIsOver;


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

    #region GAME STATES.
    public void StartingGame() =>
        StartCoroutine(SecondsCounter());
    

    public void ReloadGame() =>
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        


    void EndGame()
    {
        BobMovement.instance.gameObject.GetComponent<BobRotates>().rotateSpeed = -90f;
        gameIsOver = true;
        BobMovement.instance.KillMovement();

        StartGame.instance.secondsCountUI.SetActive(false);
        restartScreen.SetActive(true);

        score.text = ReturnScore().ToString();
    }
    #endregion


    IEnumerator SecondsCounter()
    {
        if (int.Parse(secondsPassedDisplay.text) <= 0)
        {
            EndGame();
            yield break;
        }
                
        yield return new WaitForSeconds(1f);

        int newSeconds = int.Parse(secondsPassedDisplay.text) - 1;
        secondsPassedDisplay.text = newSeconds.ToString();

        StartCoroutine(SecondsCounter());
    }


    public void AdjustingScore(float adjustment) =>
        score.text = (ReturnScore() + adjustment).ToString();

    public float ReturnScore() =>
        float.Parse(score.text);


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (instance == null)
            instance = this;
    }

    void OnDestroy() =>
        SceneManager.sceneLoaded -= OnSceneLoaded;
}
