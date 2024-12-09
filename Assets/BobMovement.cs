using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BobMovement : MonoBehaviour
{
    public GameObject badFoodTarget; // temp public
    public List<GameObject> badFoodList;
    public static BobMovement instance;
    // public GameObject centreOfPlayer;

    float yOffset = -3.5f; // for finding proper centre of player



    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        badFoodList = new List<GameObject>();
    }

    public bool movementCausedByPlayer;
    public bool playerOverridesMovement; // temp public.
    public bool playerLaunchedBob; // temp public.

    void Update()
    {
        // grab mouse input. inside collider.
        // playerOverridesMovement = true;
        // player overrides movement 
    }

    public float moveSpeed;
    bool alreadyMoving;

    void FixedUpdate()
    {
        if (badFoodList.Count <= 0)
            return;

        if (alreadyMoving)
            return;

        // if mouse clicks pass....
        if (Input.GetMouseButtonDown(1))
        {
            StopCoroutine(FlyTowardsFood(badFoodList[0], movementCausedByPlayer));     
            Debug.Log("killed coroutine");       
            PlayerCausedToFlySomeDistance();
            // StopCoroutine(FlyTowardsFood(badFoodList[0], movementCausedByPlayer));
        }

        // check for movement.

        
        if (!alreadyMoving)
            StartCoroutine(FlyTowardsFood(badFoodList[0], movementCausedByPlayer));

        // transform.position = badFoodList[0].transform.position;
    }

    IEnumerator PlayerCausedToFlySomeDistance()
    {
        alreadyMoving = true;
        movementCausedByPlayer = true;
        Debug.Log("player triggered fly distance");
        yield break;
    }
    

    IEnumerator FlyTowardsFood(GameObject target, bool playerCausedMovement)
    {
        alreadyMoving = true;
        movementCausedByPlayer = false;

        Vector3 targetPos = new Vector3 (target.transform.position.x, target.transform.position.y + yOffset, target.transform.position.z);
        

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPos, 
                moveSpeed * Time.deltaTime);

            yield return null;
        }

        alreadyMoving = false;
    }

    


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "food")
        {
            GameObject food = col.gameObject;

            // Can only eat good foods if moved by player.
            if (food.name == "a good food" && movementCausedByPlayer)
                GameManager.instance.AdjustingScore(1);
            
            // Can only eat target bad food normally, or all bad foods if moved by player.
            if (food.name == "a bad food")
            {
                if (food == badFoodList[0] || movementCausedByPlayer)
                {
                    GameManager.instance.AdjustingScore(-1);
                    badFoodList.RemoveAt(badFoodList.Count - 1);
                }
            }
           

            food.SetActive(false);
        }
    }
}
