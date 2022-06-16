using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameHUD : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI roomClearText;
    private Shakeable roomClearTextShake;
    private UIFade roomClearTextFade;

    [SerializeField] private TextMeshProUGUI successText;
    private Shakeable successTextShake;
    private UIFade successTextFade;

    [SerializeField] private TextMeshProUGUI gameOverText;
    private Shakeable gameOverTextShake;
    private UIFade gameOverTextFade;
    [SerializeField] private TextMeshProUGUI killedByText;
    private Shakeable killedByTextShake;
    private UIFade killedByTextFade;
    [SerializeField] private RawImage blackHole;
    private Animator blackHoleAnim;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe methods to game manager
        GameManager.GetInstance().OnRoomClear += RoomClear;
        GameManager.GetInstance().OnHostageRescued += HostageRescued;
        GameManager.GetInstance().OnGameOver += GameOver;

        // Fetch roomClearText elements
        roomClearText.gameObject.TryGetComponent<Shakeable>(out roomClearTextShake);
        roomClearText.gameObject.TryGetComponent<UIFade>(out roomClearTextFade);

        // Fetch successText elements
        successText.gameObject.TryGetComponent<Shakeable>(out successTextShake);
        successText.gameObject.TryGetComponent<UIFade>(out successTextFade);

        // Fetch gameOverText elements
        gameOverText.gameObject.TryGetComponent<Shakeable>(out gameOverTextShake);
        gameOverText.gameObject.TryGetComponent<UIFade>(out gameOverTextFade);

        // Fetch killedByText elements
        killedByText.gameObject.TryGetComponent<Shakeable>(out killedByTextShake);
        killedByText.gameObject.TryGetComponent<UIFade>(out killedByTextFade);

        // Fetch blackHole elements
        blackHole.gameObject.TryGetComponent<Animator>(out blackHoleAnim);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RoomClear()
    {
        Debug.Log("Room Cleared!");

        if (roomClearTextShake)
        {
            roomClearTextShake.DoShake(1f, 5f);
        }

        if (roomClearTextFade)
        {
            roomClearTextFade.SetAlpha(1f);
            roomClearTextFade.DoFadeOut(1f, 3f);
        }
    }
    private void HostageRescued()
    {
        Debug.Log("Hostage Rescued!");

        if (successTextFade)
        {
            successTextFade.SetAlpha(1f);
            successTextFade.DoBlink(3, 0.5f, 0.25f);
            successTextFade.DoFadeOut(1f, 5f);
        }
    }
    private void GameOver(GameObject killer)
    {
        Debug.Log("Game Over");

        if (gameOverText)
        {
            gameOverTextFade.DoFadeIn(0.5f);
            gameOverTextFade.DoFadeOut(1f, 5f);
        }

        if (killedByText)
        {
            // Set killer name
            killedByText.text = $"Killed by: {killer.name}";

            // Set banned phrases
            string[] bannedPhrases =
            {
                "(Clone)",
                "Enemy"
            };

            // Remove banned phrases
            // This is to clean up enemy names, such as Enemy Pizza(Clone) -> Pizza
            foreach (string bp in bannedPhrases)
            {
                killedByText.text = killedByText.text.Replace(bp, "");
            }

            killedByTextFade.DoFadeIn(0.5f);
            killedByTextFade.DoFadeOut(1f, 5f);
        }

        if (blackHole)
        {
            blackHoleAnim.SetTrigger("Entrance");
        }
    }
}
