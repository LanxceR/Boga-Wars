using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageState : MonoBehaviour
{
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
            GameManager.GetInstance().HostageRescued();
            hasBeenRescued = true;
        }
        else
            return;
    }
}
