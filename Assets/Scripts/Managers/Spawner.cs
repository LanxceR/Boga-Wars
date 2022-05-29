using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<GameObject> enemyList;
    private Vector2 spawnPos;
    private bool hit;

    [SerializeField] private CompositeCollider2D spawnArea;

    [Header("Enemy Settings")]
    [SerializeField] private SpawnableObject[] enemies;
    [SerializeField] private Transform enemyParent;

    // Start is called before the first frame update
    void Start()
    {
        enemyList = new List<GameObject>();
        SpawnEnemies(5, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies()
    {
        hit = false;

        // Generate a random point until it's inside the spawn area collider
        do
        {
            spawnPos = GetRandomPosition(spawnArea);
            hit = spawnArea.OverlapPoint(spawnPos);
        } 
        while (!hit);

        // Instantiate enemy and add to enemy list
        enemyList.Add(Instantiate(GetEnemy(), spawnPos, transform.rotation, enemyParent));
    }
    public void SpawnEnemies(int amountToSpawn, bool randomizeAmount)
    {
        // If randomize amount, randomize the amount to spawmn
        if (randomizeAmount)
            amountToSpawn = Random.Range(1, amountToSpawn + 1);

        // Spawn enemy amountToSpawn times
        for (int i = 0; i < amountToSpawn; i++)
        {
            SpawnEnemies();
        }
    }

    private Vector2 GetRandomPosition(CompositeCollider2D area)
    {
        float xPos = Random.Range(area.bounds.min.x, area.bounds.max.x);
        float yPos = Random.Range(area.bounds.min.y, area.bounds.max.y);

        return new Vector2(xPos, yPos);
    }

    private GameObject GetEnemy()
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

    public void FillEnemies(List<GameObject> enemyList)
    {
        // Add all enemies to list
        foreach (GameObject enemy in enemyList)
        {
            enemyList.Add(enemy);
        }
    }

    public void ClearEnemies()
    {
        // Fetch all current enemy clones in scene
        foreach (GameObject enemy in enemyList)
        {
            Destroy(enemy);
        }

        // Clear enemy instances list
        enemyList.Clear();
    }
}
