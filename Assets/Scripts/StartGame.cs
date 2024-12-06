using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameExplanationUIs;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject ageCountUI;
    [SerializeField] GameObject secondsCountUI;

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
