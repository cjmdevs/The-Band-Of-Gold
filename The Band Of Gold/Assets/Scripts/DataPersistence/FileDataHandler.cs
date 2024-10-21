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

   public GameData Load(string profileId)
   {
    // use Path.Combine to account for different OS's having different path seperators
    string fullPath = Path.Combine(dataDirPath, profileId, dataFilename);
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

   public void Save(GameData data, string profileId)
   {
    // use Path.Combine to account for different OS's having different path seperators
    string fullPath = Path.Combine(dataDirPath, profileId, dataFilename);
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

   public Dictionary<string, GameData> LoadAllProfiles()
   {
    Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

    // Loop over all directory names in the data directory path
    IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
    foreach (DirectoryInfo dirInfo in dirInfos)
    {
        string profileId = dirInfo.Name;

        // defensive programming - check if the data file exists
        // if it doesn't, then this folder isn't a profile and should be skipped
        string fullPath = Path.Combine(dataDirPath, profileId, dataFilename);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: "
                + profileId);
            continue;
        }

        // Load the game data for this profile and put it in the dictionary
        GameData profileData = Load(profileId);
        // defensive programming - ensure the profile data isn't null,
        // because if it is then something went wrong and we should let ourselves know
        if (profileData != null)
        {
            profileDictionary.Add(profileId, profileData);
        }
        else
        {
            Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
        }
    }

    return profileDictionary;
   }


    // the below is a simple implementation of XOR encryption
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
