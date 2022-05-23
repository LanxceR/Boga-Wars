using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

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
    private bool playerIsSpawning;

    [Header("States")]
    public bool IsPlaying = false; // Bool to determine if player is in menu or playing the game

    // Subbed at: LevelMenuUI.cs
    public UnityAction OnPauseAction;
    // Subbed at: LevelMenuUI.cs
    public UnityAction OnResumeAction;
    // Subbed at: LevelMenuUI.cs
    public UnityAction OnLevelComplete;

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

        StartGame();
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
        SpawnPlayer(SpawnPoint);
    }
    public void CompleteLevel()
    {
        OnLevelComplete?.Invoke();
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
            FollowMouse aimHelper = ActivePlayer.GetComponentInChildren<FollowMouse>();

            GameVcamTargetGroup.AddMember(ActivePlayer.transform, 0.75f, 0f);
            GameVcamTargetGroup.AddMember(aimHelper.transform, 0.25f, 0f);
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
}
