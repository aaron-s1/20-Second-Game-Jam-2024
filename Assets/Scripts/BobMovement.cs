using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BobMovement : MonoBehaviour
{
    public static BobMovement instance;
    public float moveSpeed;

    public List<GameObject> badFoodList;

    GameObject badFoodTarget;
    Coroutine flyTowardsFood;

    float yOffset = -3.5f; // for finding proper centre of player

    bool alreadyMoving;

    bool movementWasCausedByPlayer;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        badFoodList = new List<GameObject>();
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
        if (badFoodList.Count <= 0)
            return;

        if (alreadyMoving)
            return;
        
        if (!alreadyMoving) // explicit.
            flyTowardsFood = StartCoroutine(FlyTowardsFood(badFoodList[0], false));

    }


    IEnumerator FlyTowardsFood(GameObject target, bool playerCausedMovement)
    {
        alreadyMoving = true;
        movementWasCausedByPlayer = playerCausedMovement;

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


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "food")
        {
            GameObject food = col.gameObject;

            // Can only eat good foods if moved by player.
            if (food.name == "a good food" && movementWasCausedByPlayer)
            {
                GameManager.instance.AdjustingScore(1.25f);
                food.SetActive(false);
            }
            
            // Can only eat target bad food normally, or all bad foods if moved by player.
            if (food.name == "a bad food")
            {
                if (food == badFoodList[0] || movementWasCausedByPlayer)
                {
                    GameManager.instance.AdjustingScore(-0.75f);

                    if (badFoodList.Contains(food))
                        badFoodList.Remove(food);
                    food.SetActive(false);                    
                }
            }
        }
    }


    public void KillMovement()
    {
        moveSpeed = 0;

        if (badFoodList.Count > 1)
            StopCoroutine(flyTowardsFood);

        alreadyMoving = true;
    }


    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }    

}
