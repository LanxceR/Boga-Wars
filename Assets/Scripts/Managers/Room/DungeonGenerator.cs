using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Direction
{
    NORTH, WEST, EAST, SOUTH
}
public class DungeonGenerator : MonoBehaviour
{
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();
    private List<Vector2Int> dungeonRooms;

    [Header("Prefab Settings")]
    [SerializeField] private List<RoomState> startRoomPrefabs = new List<RoomState>();
    [SerializeField] private List<RoomState> roomPrefabs = new List<RoomState>();
    [SerializeField] private List<RoomState> finishRoomPrefabs = new List<RoomState>();

    [Header("Generation Settings")]
    [SerializeField] private int numberOfCrawlers;
    [SerializeField] private int roomsMin;
    [SerializeField] private int roomsMax;

    [Header("Room Controller Settings")]
    [SerializeField] private RoomController roomController;

    // Define each Direction enum to a Vector2 value and store it in this map
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        { Direction.NORTH, Vector2Int.up },
        { Direction.WEST, Vector2Int.left },
        { Direction.EAST, Vector2Int.right },
        { Direction.SOUTH, Vector2Int.down }
    };

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        dungeonRooms = GenerateDungeon(numberOfCrawlers, roomsMin, roomsMax);
        SpawnRooms(dungeonRooms, startRoomPrefabs, roomPrefabs, finishRoomPrefabs);

        // Subscribe methods to game manager
        GameManager.GetInstance().OnSceneChange += FlushRooms;
    }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        GameManager.GetInstance().StartGame();
    }

    public List<Vector2Int> GenerateDungeon(int numberOfCrawlers, int roomsMin, int roomsMax)
    {
        List<DungeonCrawler> crawlers = new List<DungeonCrawler>();

        for (int i = 0; i < numberOfCrawlers; i++)
        {
            crawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }

        // Add 0,0 to positions visited by default
        positionsVisited.Add(new Vector2Int(0, 0));
        
        int roomTotal = Random.Range(roomsMin, roomsMax + 1);
        int roomCount = 0;

        while (roomCount <= roomTotal)
        {
            foreach (DungeonCrawler c in crawlers)
            {
                // Generate random direction
                Direction randomDir = (Direction)Random.Range(0, directionMovementMap.Count);

                Vector2Int newPos = c.GetNextPosition(directionMovementMap, randomDir);

                if (PositionIsEmpty(newPos))
                {
                    c.MoveTo(newPos);
                    positionsVisited.Add(newPos);
                    roomCount++;
                }
            }
        }        

        return positionsVisited;
    }

    private void SpawnRooms(List<Vector2Int> rooms, List<RoomState> startRoomPrefabs, List<RoomState> roomPrefabs, List<RoomState> finishRoomPrefabs)
    {
        RoomState roomPrefabToAdd;

        // Add player spawn room first at 0,0
        roomPrefabToAdd = startRoomPrefabs[Random.Range(0, startRoomPrefabs.Count)];
        roomController.AddRoomIntoQueue(roomPrefabToAdd, 0, 0);

        // Add rooms
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            roomPrefabToAdd = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
            roomController.AddRoomIntoQueue(roomPrefabToAdd, rooms[i].x, rooms[i].y);
        }

        // Add end area room last
        roomPrefabToAdd = finishRoomPrefabs[Random.Range(0, finishRoomPrefabs.Count)];
        roomController.AddRoomIntoQueue(roomPrefabToAdd, rooms[rooms.Count - 1].x, rooms[rooms.Count - 1].y);
    }

    private bool PositionIsEmpty(Vector2Int position)
    {
        bool posIsEmpty = true;

        foreach (Vector2Int pos in positionsVisited)
        {
            if (pos.Equals(position))
            {
                posIsEmpty = false;
            }
        }

        return posIsEmpty;
    }

    public void FlushRooms(float delay)
    {
        StartCoroutine(FlushRoomsCoroutine(delay));
    }
    private IEnumerator FlushRoomsCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        positionsVisited.Clear();
    }
}
