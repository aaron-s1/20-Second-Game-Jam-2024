using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SpawnFood : MonoBehaviour
{
    public static SpawnFood instance;

    [SerializeField] float spawnRate;
    [SerializeField] GameObject baseFoodPrefab;
    [SerializeField] List<Sprite> badFoodSprites;
    [SerializeField] List<Sprite> goodFoodSprites;
    [SerializeField] List<GameObject> allFoodSpawns;

    [SerializeField] float globalSpawnDeadZone;

    bool badFoodTime = true;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }


    public void StartSpawns() 
    {
        ForceAGoodFoodSpawn(); ForceAGoodFoodSpawn(); ForceAGoodFoodSpawn(); ForceAGoodFoodSpawn(); ForceAGoodFoodSpawn();
        InvokeRepeating("Spawn", 0, spawnRate);
    }


    public void ForceAGoodFoodSpawn()
    {
        List<Sprite> spriteListToUse;
        spriteListToUse = goodFoodSprites;
        Vector3 spawnPosition = GetRandomSpawnPosition(globalSpawnDeadZone);
        GameObject spawnedFood = Instantiate(baseFoodPrefab, spawnPosition, Quaternion.identity);
        RandomizeSprite(spawnedFood, spriteListToUse);

        spawnedFood.SetActive(true);    
        allFoodSpawns.Add(spawnedFood);
    }

    void Spawn()
    {        
        if (GameManager.instance.gameIsOver)
        {
            CancelInvoke("Spawn");

            foreach (GameObject a_food in allFoodSpawns)
            {
                if (a_food.activeInHierarchy)
                    a_food.SetActive(false);
            }
            return;
        }

        List<Sprite> spriteListToUse;
            
        if (badFoodTime)
            spriteListToUse = badFoodSprites;
        else spriteListToUse = goodFoodSprites;

        Vector3 spawnPosition = GetRandomSpawnPosition(globalSpawnDeadZone);
        GameObject spawnedFood = Instantiate(baseFoodPrefab, spawnPosition, Quaternion.identity);
        RandomizeSprite(spawnedFood, spriteListToUse);

        spawnedFood.SetActive(true);

        if (badFoodTime)
            BobMovement.instance.badFoodList.Add(spawnedFood);
        
        allFoodSpawns.Add(spawnedFood);
        badFoodTime = !badFoodTime;
    }


    Vector3 GetRandomSpawnPosition(float globalDeadZone)
    {
        Vector2 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)); // bottom left
        Vector2 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)); // top right

        // Calculate the dead zone margins
        float xMin = Mathf.Lerp(screenMin.x, screenMax.x, globalDeadZone);
        float xMax = Mathf.Lerp(screenMin.x, screenMax.x, (1f - globalDeadZone));
        float yMin = Mathf.Lerp(screenMin.y, screenMax.y, globalDeadZone);
        float yMax = Mathf.Lerp(screenMin.y, screenMax.y, (1f - globalDeadZone));

        // generate a random position within the allowed  area
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);

        return new Vector3(randomX, randomY, 0);
    }


    void RandomizeSprite(GameObject food, List<Sprite> spriteList)
    {
        if (spriteList == badFoodSprites)
            food.name = "a bad food";
        else
            food.name = "a good food";
      
        int randomIndex = Random.Range(0, spriteList.Count);        
        food.GetComponent<SpriteRenderer>().sprite = spriteList[randomIndex];
    }


    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }   
}
