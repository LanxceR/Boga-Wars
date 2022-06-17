using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageState : MonoBehaviour
{
    [Header("States")]
    public bool hasBeenRescued = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RescueHostage()
    {
        if (!hasBeenRescued)
        {
            // Play sfx
            AudioManager.GetInstance().PlayFanfareSfx();
            GameManager.GetInstance().HostageRescued();
            GameSceneManager.GetInstance().GotoSceneWithDelay(5f);
            hasBeenRescued = true;
        }
        else
            return;
    }
}
