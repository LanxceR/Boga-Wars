using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // Save the game
    public static void SaveGame(string saveName, List<Gallery> unlockedGalleries)
    {
        // Prepare a playerData object
        PlayerData save = new PlayerData(unlockedGalleries);
        save.unlockedGalleries = unlockedGalleries;


        // Setup a BinaryFormatter object
        BinaryFormatter formatter = new BinaryFormatter();
        // Setup the filepath location
        string path = Application.persistentDataPath + $"/{saveName}.save";
        // Create a new filestream to create a savefile (or overwrite if one already exists)
        FileStream stream = new FileStream(path, FileMode.Create);


        // Write data into a file, then close stream
        formatter.Serialize(stream, save);
        stream.Close();

        Debug.Log($"Game saved in {path}");
    }

    // Save the game
    public static PlayerData LoadGame(string saveName)
    {
        // Setup the filepath location
        string path = Application.persistentDataPath + $"/{saveName}.save";


        // Look for file
        if (File.Exists(path))
        {
            // Setup a BinaryFormatter object
            BinaryFormatter formatter = new BinaryFormatter();
            // Create a new filestream to open a savefile
            FileStream stream = new FileStream(path, FileMode.Open);

            // Read data from file, then close stream
            PlayerData save = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            string debug = $"Game Loaded! \n" +
                           $"Unlocked Galleries: ";
            foreach (Gallery g in save.unlockedGalleries) debug += $"{g} ";
            Debug.Log(debug);
            return save;
        }
        else
        {
            Debug.Log($"No {saveName}.save file found in {path}");
            return null;
        }
    }
}
