using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Singleton instance
    private static Spawner instance;

    private Vector2 spawnPos;
    private bool hit;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Spawner GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnEnemy(CompositeCollider2D spawnArea, Transform enemyParent, SpawnableObject[] enemies)
    {
        hit = false;

        // Generate a random point until it's inside the spawn area collider
        do
        {
            spawnPos = GetRandomPosition(spawnArea);
            hit = spawnArea.OverlapPoint(spawnPos);
        } 
        while (!hit);

        // Instantiate enemy
        return Instantiate(GetEnemy(enemies), spawnPos, transform.rotation, enemyParent);
    }

    public List<GameObject> SpawnEnemies(int amountToSpawn, bool randomizeAmount, CompositeCollider2D spawnArea, Transform enemyParent, SpawnableObject[] enemies)
    {
        // Create a list to store enemies
        List<GameObject> enemyList = new List<GameObject>();

        // If randomize amount, randomize the amount to spawmn
        if (randomizeAmount)
            amountToSpawn = Random.Range(1, amountToSpawn + 1);

        // Spawn enemy amountToSpawn times
        for (int i = 0; i < amountToSpawn; i++)
        {
            enemyList.Add(SpawnEnemy(spawnArea, enemyParent, enemies));
        }

        return enemyList;
    }

    private Vector2 GetRandomPosition(CompositeCollider2D area)
    {
        float xPos = Random.Range(area.bounds.min.x, area.bounds.max.x);
        float yPos = Random.Range(area.bounds.min.y, area.bounds.max.y);

        return new Vector2(xPos, yPos);
    }

    private GameObject GetEnemy(SpawnableObject[] enemies)
    {
        // Calculate max limit of chances
        int limit = 0;
        foreach (SpawnableObject enemy in enemies)
        {
            limit += enemy.Rate;
        }

        // Generate a random number from 0 to limit
        int random = Random.Range(0, limit);

        // Return an enemy based on generated number
        foreach (SpawnableObject enemy in enemies)
        {
            if (random < enemy.Rate)
            {
                return enemy.Prefab;
            }
            else
            {
                random -= enemy.Rate;
            }
        }

        return null;
    }

    //public void ClearEnemyList()
    //{
    //    // Clear enemy instances list
    //    enemyList.Clear();
    //}

    //public void DestroyEnemies()
    //{
    //    // Fetch all current enemy clones in scene
    //    foreach (GameObject enemy in enemyList)
    //    {
    //        Destroy(enemy);
    //    }

    //    // Clear enemy instances list
    //    enemyList.Clear();
    //}
}
