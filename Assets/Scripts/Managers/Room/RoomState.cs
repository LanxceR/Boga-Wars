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

    [Header("Room Identifier")]
    public bool playerSpawnRoom = false;
    public bool exitRoom = false;
    public bool hostageRoom = false;
    public int idX;
    public int idY;

    [Header("Room Status")]
    public bool roomIsInCombat = false;
    public bool roomHasBeenVisited = false;
    public bool roomIsCleared = false;

    [Header("Room Area")]
    [SerializeField] private CompositeCollider2D roomArea;
    [SerializeField] private LayerMask actorLayerMask = 1<<7;

    [Header("Room Prefab Setting")]
    public float width = 24f;
    public float height = 24f;

    [Header("Entrances")]
    [SerializeField] private List<GameObject> entranceWalls;
    [SerializeField] private List<GameObject> entranceDoors;

    // Start is called before the first frame update
    void Start()
    {
        enemyHolder = GetComponent<EnemyHolder>();
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

                if (!enemyHolder || 
                    !enemyHolder.enabled ||
                    enemyHolder.amountToSpawn <= 0 ||
                    roomIsCleared)
                {
                    // Room is automatically cleared if enemyHolder is not activated
                    ClearRoom();
                } 
                else if (enemyHolder.amountToSpawn != 0)
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
        }

        List<DoorAnimation> doorAnims = GetComponents<DoorAnimation>().Concat(GetComponentsInChildren<DoorAnimation>()).ToList();

        if (doorAnims != null)
        {
            foreach (DoorAnimation doorAnim in doorAnims)
                doorAnim.CloseDoor();
        }
    }

    public void OpenAllDoors()
    {
        foreach (GameObject door in entranceDoors)
        {
            door.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public void SetEntrances()
    {
        // First, deactivate all doors and activate all walls
        foreach (GameObject door in entranceDoors)
        {
            door.SetActive(false);
        }

        foreach (GameObject walls in entranceWalls)
        {
            walls.SetActive(true);
        }

        // Then, check for neighboring rooms
        // If there's one on a side, deactivate that side's wall and activate that side's door
        if (GetNeigboringRoom(RoomSide.NORTH))
        {
            entranceDoors[(int)RoomSide.NORTH].SetActive(true);
            entranceWalls[(int)RoomSide.NORTH].SetActive(false);
        }
        if (GetNeigboringRoom(RoomSide.WEST))
        {
            entranceDoors[(int)RoomSide.WEST].SetActive(true);
            entranceWalls[(int)RoomSide.WEST].SetActive(false);
        }
        if (GetNeigboringRoom(RoomSide.EAST))
        {
            entranceDoors[(int)RoomSide.EAST].SetActive(true);
            entranceWalls[(int)RoomSide.EAST].SetActive(false);
        }
        if (GetNeigboringRoom(RoomSide.SOUTH))
        {
            entranceDoors[(int)RoomSide.SOUTH].SetActive(true);
            entranceWalls[(int)RoomSide.SOUTH].SetActive(false);
        }
    }

    private RoomState GetNeigboringRoom(RoomSide side)
    {
        int x = 0;
        int y = 0;
        switch (side)
        {
            case RoomSide.NORTH:
                y = 1;
                break;
            case RoomSide.WEST:
                x = -1;
                break;
            case RoomSide.EAST:
                x = 1;
                break;
            case RoomSide.SOUTH:
                y = -1;
                break;
        }

        if (RoomController.GetInstance().DoesRoomExist(idX + x, idY + y))
        {
            return RoomController.GetInstance().FindRoom(idX + x, idY + y);
        }
        return null;
    }

    public void ClearRoom()
    {
        // Room is cleared
        roomIsCleared = true;

        // Mark this room not in active combat (because there is no enemies to begin with)
        roomIsInCombat = false;
    }
}
