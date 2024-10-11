using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
   private string dataDirPath = "";
   private string dataFilename = "";

   public FileDataHandler(string dataDirPath, string dataFilename)
   {
    this.dataDirPath = dataDirPath;
    this.dataFilename = dataFilename;
   }

   public GameData Load()
   {
    // use Path.Combine to account for different OS's having different path seperators
    string fullPath = Path.Combine(dataDirPath, dataFilename);
    GameData loadedData = null;
    if (File.Exists(fullPath))
    {
        try
        {

        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
        }
    }
    return loadedData;
   }

   public void Save(GameData data)
   {
    // use Path.Combine to account for different OS's having different path seperators
    string fullPath = Path.Combine(dataDirPath, dataFilename);
    try
    {
        // create the directory the file will be written to if it doesn't already exist
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        // serialize the C# game data object into Json
        string dataToStore = JsonUtility.ToJson(data, true);

        // write the serialize data to the file
        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(dataToStore);
            }
        }
    }
    catch (Exception e)
    {
        Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
    }
   }

}
