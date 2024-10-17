using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
   private string dataDirPath = "";
   private string dataFilename = "";
   private bool useEncryption = false;
   private readonly string encryptionCodeWord = "word";

   public FileDataHandler(string dataDirPath, string dataFilename, bool useEncryption)
   {
    this.dataDirPath = dataDirPath;
    this.dataFilename = dataFilename;
    this.useEncryption = useEncryption;
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
            // load the serialize data from the file
            string dataToLoad = "";
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
            }

            // optioinally decrypt the data
            if (useEncryption)
            {
                dataToLoad = EncryptDecrypt(dataToLoad);
            }

            // deserailize the data from Json back into the C# object
            loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
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

        // optionally encrypt the data
        if (useEncryption)
        {
            dataToStore = EncryptDecrypt(dataToStore);
        }

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

    // the beliw is a simple implementation of XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char) (data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
