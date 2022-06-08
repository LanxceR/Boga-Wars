using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum RoomSide
{
    NORTH, WEST, EAST, SOUTH
}
public class RoomState : MonoBehaviour
{
    private EnemyHolder enemyHolder;

    [Header("Room Status")]
    public bool roomIsInCombat = false;
    public bool roomHasBeenVisited = false;
    public bool roomIsCleared = false;

    [Header("Room Area")]
    [SerializeField] private CompositeCollider2D roomArea;
    [SerializeField] private LayerMask actorLayerMask = 1<<7;

    [Header("Entrances")]
    [SerializeField] private List<GameObject> entranceWalls;
    [SerializeField] private List<GameObject> entranceDoors;

    // Start is called before the first frame update
    void Start()
    {
        enemyHolder = GetComponent<EnemyHolder>();

        // Visit all room once at start to detect where the player is at the start of the game
        VisitRoom();
    }

    // Update is called once per frame
    void Update()
    {
        if (roomIsCleared)
        {
            OpenAllDoors();
        }
    }

    public void VisitRoom()
    {
        // OverlapCollider settings (to detect if player is in room)
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(actorLayerMask);
        filter.useLayerMask = true;
        List<Collider2D> allOverlappingColliders = new List<Collider2D>();
        roomArea.OverlapCollider(filter, allOverlappingColliders);

        foreach (Collider2D col in allOverlappingColliders)
        {
            // If player has visited this room / is in this room
            if (col.CompareTag("Player"))
            {
                // Room has been visited
                roomHasBeenVisited = true;

                if ((!enemyHolder || !enemyHolder.enabled) || roomIsCleared)
                {
                    // Room is automatically cleared if enemyHolder is not activated
                    roomIsCleared = true;

                    // Mark this room not in active combat (because there is no enemies to begin with)
                    roomIsInCombat = false;
                } 
                else
                {
                    // If there is enemy, close all doors
                    LockAllDoors();

                    // Spawn enemies
                    enemyHolder.Spawn();

                    // Mark this room as currently active in combat
                    roomIsInCombat = true;
                }
            }
        }
    }

    public void LockAllDoors()
    {
        foreach (GameObject door in entranceDoors)
        {
            door.GetComponent<Collider2D>().isTrigger = false;

            List<DoorAnimation> doorAnims = GetComponents<DoorAnimation>().Concat(GetComponentsInChildren<DoorAnimation>()).ToList();

            if (doorAnims != null)
            {
                foreach (DoorAnimation doorAnim in doorAnims)
                    doorAnim.CloseDoor();
            }
        }
    }

    public void OpenAllDoors()
    {
        foreach (GameObject door in entranceDoors)
        {
            door.GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
