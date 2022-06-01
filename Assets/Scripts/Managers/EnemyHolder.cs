using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    private List<GameObject> enemyObjects;

    [Header("Room Status")]
    public bool roomIsCleared = false;

    [Header("Spawn Settings")]
    [SerializeField] private int amountToSpawn;
    [SerializeField] private bool randomizeSpawnAmount = true;
    [SerializeField] private int reinforcements;
    [SerializeField] private bool randomizeReinforcements = true;

    [Header("Spawn Area")]
    [SerializeField] private CompositeCollider2D spawnArea;

    [Header("Enemy Settings")]
    [SerializeField] private SpawnableObject[] enemyTypes;
    [SerializeField] private List<HealthSystem> enemyList;

    // Start is called before the first frame update
    void Start()
    {
        enemyList = new List<HealthSystem>();
        enemyObjects = new List<GameObject>();

        // Randomize reinforcements if neccesary
        if (randomizeReinforcements)
            reinforcements = Random.Range(1, reinforcements + 1);

        // Spawn enemies at start
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRoomStatus();
    }

    // Spawn Enemies
    private void Spawn()
    {
        // Call Spawner to spawn enemies and store in this gameobject
        enemyObjects = Spawner.GetInstance().SpawnEnemies(amountToSpawn, randomizeSpawnAmount, spawnArea, transform, enemyTypes);
        UpdateEnemyList();
    }

    // Check if there's reinforcements available or set room as cleared
    private void UpdateRoomStatus()
    {
        if (AreAllEnemiesDead())
        {
            if (reinforcements > 0)
            {
                // When all enemies are dead, spawn reinforcements (if there are any)
                Spawn();

                // Count down reinforcements left count
                reinforcements--;
            }
            else
            {
                roomIsCleared = true;
            }
        }
    }

    // Update enemy list
    private void UpdateEnemyList()
    {
        // Store enemy list type as HealthSystem (for better performance when checking for enemy's health/alive status)
        enemyList.Clear();
        foreach (GameObject enemyObject in enemyObjects)
        {
            enemyObject.TryGetComponent<HealthSystem>(out HealthSystem health);
            enemyList.Add(health);
        }
    }

    // Check if all spawned enemies are dead yet
    private bool AreAllEnemiesDead()
    {
        foreach (HealthSystem enemy in enemyList)
        {
            if (!enemy.isDead)
            {
                return false;
            }
        }
        return true;
    }
}
