using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomState))]
public class EnemyHolder : MonoBehaviour
{
    private RoomState roomState;
    private List<GameObject> enemyObjects;

    [Header("Spawn Settings")]
    public int amountToSpawn;
    [SerializeField] private bool randomizeSpawnAmount = true;
    public int reinforcements;
    [SerializeField] private bool randomizeReinforcements = true;

    [Header("Spawn Area")]
    [SerializeField] private CompositeCollider2D spawnArea;

    [Header("Enemy Settings")]
    [SerializeField] private SpawnableObject[] enemyTypes;
    [SerializeField] private List<HealthSystem> enemyList;

    // Start is called before the first frame update
    void Start()
    {
        roomState = GetComponent<RoomState>();
        enemyList = new List<HealthSystem>();
        enemyObjects = new List<GameObject>();

        // Randomize reinforcements if neccesary
        if (randomizeReinforcements && reinforcements != 0)
            reinforcements = Random.Range(0, reinforcements + 1);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatusOfEnemies();
    }

    // Spawn Enemies
    public void Spawn()
    {
        // Check if spawn area is set or not
        // IMPORTANT: this is to prevent not setting up spawn area leading to infinite loop when spawning
        if (spawnArea.bounds.size.magnitude <= 0)
        {
            Debug.LogError("Spawn Area is not set!");
            return;
        }

        // Call Spawner to spawn enemies and store in this gameobject
        enemyObjects = Spawner.GetInstance().SpawnEnemies(amountToSpawn, randomizeSpawnAmount, spawnArea, transform, enemyTypes);
        UpdateEnemyList();
    }

    // Check if there's reinforcements available or set room as cleared
    private void UpdateStatusOfEnemies()
    {
        if (!roomState.roomIsInCombat) return;

        if (AreAllEnemiesDead())
        {
            if (reinforcements > 0 && amountToSpawn > 0)
            {
                // When all enemies are dead, spawn reinforcements (if there are any)
                Spawn();

                // Count down reinforcements left count
                reinforcements--;
            }
            else 
            {
                // Play sfx
                AudioManager.GetInstance().PlayRoomClear();
                roomState.ClearRoom();

                // Invoke UnityAction OnRoomClear in GameManager
                GameManager.GetInstance().RoomClear();
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
