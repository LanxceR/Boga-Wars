using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe methods to game manager
        GameManager.GetInstance().OnRoomClear += RoomClear;
        GameManager.GetInstance().OnHostageRescued += HostageRescued;

        // Fetch roomClearText elements
        roomClearText.gameObject.TryGetComponent<Shakeable>(out roomClearTextShake);
        roomClearText.gameObject.TryGetComponent<UIFade>(out roomClearTextFade);

        // Fetch successText elements
        successText.gameObject.TryGetComponent<Shakeable>(out successTextShake);
        successText.gameObject.TryGetComponent<UIFade>(out successTextFade);
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
}
