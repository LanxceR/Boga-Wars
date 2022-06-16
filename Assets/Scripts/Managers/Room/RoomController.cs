using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    // Singleton instance
    private static RoomController instance;

    [Header("Containers")]
    public List<RoomState> loadedRooms = new List<RoomState>();

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

    // Get singleton instance
    public static RoomController GetInstance()
    {
        return instance;
    }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        foreach (RoomState room in loadedRooms)
        {
            room.SetEntrances();
        }

        // Recalculate aStar whole graph
        AstarPath.active.Scan();
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {

    }

    public void AddRoomIntoQueue(RoomState room, int x, int y)
    {
        // If another room already exists in (x, y), then stop
        if (DoesRoomExist(x, y)) return;

        room.transform.localPosition = new Vector3(x * room.width, y * room.height);
        room.name = SetRoomName(room);
        room.idX = x;
        room.idY = y;

        LoadRoom(room);
    }

    public void LoadRoom(RoomState room)
    {
        RoomState loadedRoom = Instantiate(room, transform);

        loadedRoom.name = SetRoomName(loadedRoom);

        // Add to loaded rooms list
        loadedRooms.Add(loadedRoom);
    }

    public string SetRoomName(RoomState room)
    {
        string prefix = "";
        if (room.playerSpawnRoom) prefix = "Start ";
        else if (room.hostageRoom || room.exitRoom) prefix = "Finish ";
        return
            $"{prefix}Room (" +
            $"{room.transform.localPosition.x / room.width},{room.transform.localPosition.y / room.height}" +
            $")";
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.idX == x && item.idY == y) != null;
    }

    public RoomState FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.idX == x && item.idY == y);
    }
}
