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

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe methods to game manager
        GameManager.GetInstance().OnRoomClear += RoomClear;

        // Fetch roomClearText elements
        roomClearText.gameObject.TryGetComponent<Shakeable>(out roomClearTextShake);
        roomClearText.gameObject.TryGetComponent<UIFade>(out roomClearTextFade);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoomClear()
    {
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
}
