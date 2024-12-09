using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public static StartGame instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }       

    [SerializeField] GameObject player;
    [SerializeField] GameObject gameExplanationUIs;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject ageCountUI;
    public GameObject secondsCountUI;

    public void BeginGame()
    {
        gameExplanationUIs.SetActive(false);
        startButton.SetActive(false);
        ageCountUI.SetActive(true);
        secondsCountUI.SetActive(true);
        player.SetActive(true);
        StartCoroutine(player.GetComponent<BobFacesPlayer>().StareAtPlayer());
    }    
}
