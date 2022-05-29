using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Gallery { KUE_LUMPUR, ODADING, RONDE }
[System.Serializable]
public class PlayerData
{
    /* This class is for serializing savefiles
     * Store datas in this script
     */

    // Unlocked Food Galleries
    public List<Gallery> unlockedGalleries = new List<Gallery>();


    // Constructors
    public PlayerData()
    {
        this.unlockedGalleries = null;
    }
    public PlayerData(List<Gallery> unlockedGalleries)
    {
        this.unlockedGalleries = unlockedGalleries;
    }
}
