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
        else if (instance != this)
            Destroy(gameObject);
        badFoodList = new List<GameObject>();
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }    

    public bool movementCausedByPlayer;
    // public bool playerOverridesMovement; // temp public.
    // public bool playerLaunchedBob; // temp public.



    public float moveSpeed;
    bool alreadyMoving;
    Coroutine flyTowardsFood;
    Coroutine playerCausedToFlySomeDistance;

    public void KillMovement()
    {
        moveSpeed = 0;

        if (badFoodList.Count > 1)
            StopCoroutine(flyTowardsFood);
            // StopCoroutine(FlyTowardsBadFood(badFoodList[0], movementCausedByPlayer));
        // StopCoroutine(PlayerCausedToFlySomeDistance());

        alreadyMoving = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedGoodFood = hit.collider.gameObject;

                if (clickedGoodFood.name == "a good food")
                {
                    StopCoroutine(flyTowardsFood);
                    flyTowardsFood = StartCoroutine(FlyTowardsFood(clickedGoodFood, true));
                }
            }
        }
    }

    void FixedUpdate()
    {
        // if (GameManager.instance.gameIsOver)
        // {
        //     KillMovement();
        //     return;
        // }

        if (badFoodList.Count <= 0)
            return;

        if (alreadyMoving)
            return;

        // if mouse clicks pass....
        // if (Input.GetMouseButtonDown(1))
        // {
        //     StopCoroutine(FlyTowardsBadFood(badFoodList[0], movementCausedByPlayer));     
        //     Debug.Log("killed coroutine");       
        //     StartCoroutine(PlayerCausedToFlySomeDistance());
        //     // StopCoroutine(FlyTowardsFood(badFoodList[0], movementCausedByPlayer));
        // }

        // check for movement.

        
        if (!alreadyMoving) // explicit.
            flyTowardsFood = StartCoroutine(FlyTowardsFood(badFoodList[0], false));

    }

    IEnumerator PlayerCausedToFlySomeDistance()
    {
        yield break;
        // StopCoroutine(flyTowardsFood);
        // alreadyMoving = true;
        // movementCausedByPlayer = true;
        // Debug.Log("player triggered fly distance");
    }
    

    IEnumerator FlyTowardsFood(GameObject target, bool playerCausedMovement)
    {
        alreadyMoving = true;
        movementCausedByPlayer = playerCausedMovement;

        Vector3 targetPos = new Vector3 (target.transform.position.x, target.transform.position.y + yOffset, target.transform.position.z);
        

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPos, 
                moveSpeed * Time.deltaTime);

            yield return null;
        }

        alreadyMoving = false;
        StopCoroutine(flyTowardsFood);
    }

    // void OnMouseDown()
    // {
    //     Debug.Log("mouse clicked");
    //     Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

    //     if (hit.collider != null)
    //     {
    //         Debug.Log("mouse hit something");
    //         GameObject clickedGoodFood = hit.collider.gameObject;

    //         if (clickedGoodFood.name == "a good food")
    //         {
    //             Debug.Log("clicked a good food");
    //             StopCoroutine(flyTowardsFood);
    //             flyTowardsFood = StartCoroutine(FlyTowardsFood(clickedGoodFood, true));
    //         }
    //     }
    // }    

    


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "food")
        {
            GameObject food = col.gameObject;

            // Can only eat good foods if moved by player.
            if (food.name == "a good food" && movementCausedByPlayer)
            {
                GameManager.instance.AdjustingScore(1.25f);
                food.SetActive(false);
            }
            
            // Can only eat target bad food normally, or all bad foods if moved by player.
            if (food.name == "a bad food")
            {
                if (food == badFoodList[0] || movementCausedByPlayer)
                {
                    GameManager.instance.AdjustingScore(-0.75f);

                    // badFoodList.RemoveAt(badFoodList.Count - 1);
                    if (badFoodList.Contains(food))
                        badFoodList.Remove(food);
                    food.SetActive(false);                    
                }
            }
        }
    }
}
