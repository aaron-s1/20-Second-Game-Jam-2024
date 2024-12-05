using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SpawnFood : MonoBehaviour
{
    [SerializeField] GameObject baseFoodPrefab;
    [SerializeField] List<Sprite> badFoodSprites;
    [SerializeField] List<Sprite> goodFoodSprites;

    private bool badFoodTime = true; // start with bad food

    // void Start() =>
        // StartSpawns(2f);

    public void StartSpawns(float rate = 2f) => 
        InvokeRepeating("Spawn", 0, rate);


    public void Spawn()
    {
        List<Sprite> spriteListToUse = badFoodTime ? badFoodSprites : goodFoodSprites;

        GameObject spawnedFood = Instantiate(baseFoodPrefab, transform.position, Quaternion.identity);
        Sprite spriteToUse = GetRandomSprite(spriteListToUse, spawnedFood);

        spawnedFood.GetComponent<SpriteRenderer>().sprite = spriteToUse;
        
        badFoodTime = !badFoodTime;
    }



    Sprite GetRandomSprite(List<Sprite> spriteList, GameObject spawnedFood)
    {
        if (spriteList == badFoodSprites)
            spawnedFood.name = "a bad food";
        else
            spawnedFood.name = "a good food";
      
        if (spriteList.Count == 0)
            return null;

        int randomIndex = Random.Range(0, spriteList.Count);
        return spriteList[randomIndex];
    }
}
