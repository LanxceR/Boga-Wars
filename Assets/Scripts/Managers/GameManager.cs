using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;

    [Header("Cinemachine Camera")]
    [SerializeField] private CinemachineTargetGroup GameVcamTargetGroup;

    [Header("Player Prefabs")]
    [SerializeField] private GameObject PlayerPrefab;
    public GameObject ActivePlayer;

    [Header("Player Spawn")]
    public Transform SpawnPoint;
    [SerializeField] private bool spawnPointSet = false;

    [Header("States")]
    public bool IsPlaying = false; // Bool to determine if player is in menu or playing the game

    // Subbed at: InGameMenuUI.cs
    public UnityAction OnPauseAction;
    // Subbed at: InGameMenuUI.cs
    public UnityAction OnResumeAction;
    // Subbed at: InGameHUD.cs
    public UnityAction<GameObject> OnGameOver;
    // Subbed at: InGameHUD.cs
    public UnityAction OnRoomClear;
    // Subbed at: InGameHUD.cs
    public UnityAction OnHostageRescued;
    // Subbed at: DungeonGnereator.cs
    public UnityAction<float> OnSceneChange;

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

        FindPlayer();
        if (ActivePlayer)
        {
            // If there is one, also add the player to camera's target group
            SetVcamTargetGroup(ActivePlayer);
        }
    }

    // Get singleton instance
    public static GameManager GetInstance()
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

    public void StartGame()
    {
        IsPlaying = true;
        SetSpawn();
        SpawnPlayer(SpawnPoint);
    }

    // Set Spawn
    public void SetSpawn()
    {
        RoomState spawnRoom;
        var rooms = FindObjectsOfType<RoomState>();

        // Find the player spawn room
        foreach (var room in rooms)
        {
            if (room.playerSpawnRoom)
            {
                // Set spawn point to the middle of that room
                spawnRoom = room;
                SpawnPoint.position = spawnRoom.transform.position;
                break;
            }
        }
    }

    // Spawn player
    public void SpawnPlayer()
    {
        SpawnPlayer(PlayerPrefab.transform);
    }
    public void SpawnPlayer(Transform spawnPoint)
    {
        if (!ActivePlayer)
        {
            ActivePlayer = Instantiate(PlayerPrefab, spawnPoint.position, Quaternion.identity);
            SetVcamTargetGroup(ActivePlayer);
        }
        else
        {
            ActivePlayer.SetActive(true);
            ActivePlayer.transform.position = spawnPoint.position;
            foreach (var behaviour in ActivePlayer.GetComponents<Behaviour>())
            {
                behaviour.enabled = true;
            }
        }
    }

    private void FindPlayer()
    {
        // Find a player object if there's already one in scene from the start
        var activePlayer = FindObjectOfType<PlayerMovement>();

        if (activePlayer)
        {
            ActivePlayer = activePlayer.gameObject;
        }
    }

    public void SetVcamTargetGroup(GameObject player)
    {
        FollowMouse aimHelper = player.GetComponentInChildren<FollowMouse>();

        GameVcamTargetGroup.AddMember(ActivePlayer.transform, 0.70f, 0f);
        GameVcamTargetGroup.AddMember(aimHelper.transform, 0.30f, 0f);
    }

    // OnCancel listener from InputAction "PlayerInput.inputaction"
    // Mapped in UI
    void OnCancel(InputValue moveValue)
    {
        PauseAndResumeGame();
    }

    public void PauseGame()
    {
        IsPlaying = false;
        Time.timeScale = 0f;
        OnPauseAction?.Invoke();
    }
    public void ResumeGame()
    {
        IsPlaying = true;
        Time.timeScale = 1f;
        OnResumeAction?.Invoke();
    }
    public void PauseAndResumeGame()
    {
        if (IsPlaying)
        {
            // Pause the game
            PauseGame();
        }
        else
        {
            // Resume the game
            ResumeGame();
        }
    }
    public void RoomClear()
    {
        OnRoomClear?.Invoke();
    }
    public void HostageRescued()
    {
        OnHostageRescued?.Invoke();
    }
    public void GameOver(GameObject killer)
    {
        OnGameOver?.Invoke(killer);
    }
    public void SceneChange(float delay)
    {
        OnSceneChange?.Invoke(delay);
    }
}
