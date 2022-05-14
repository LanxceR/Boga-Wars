using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedGalleriesTest : MonoBehaviour
{
    public List<Gallery> unlockedGalleries = new List<Gallery>();
    public string saveName;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        Load();
    }
    

    public void Save()
    {
        SaveSystem.SaveGame(saveName, unlockedGalleries);
    }

    public void Load()
    {
        unlockedGalleries = SaveSystem.LoadGame(saveName).unlockedGalleries;
    }
}
