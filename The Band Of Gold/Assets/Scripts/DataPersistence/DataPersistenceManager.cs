using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

   public static DataPersistenceManager instance { get; private set; }

   private void Awake()
   {
        if (instance != null)
        {
            Debug.LogError("Found morer than one Data Persistence Manager in the scene.");
        }
        instance = this;
   }

    private void Start()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // TODO - Load any saved data from a file using data handler

        // if no data can be loaded, initialize to a new game
        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
        
        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log("Loaded death count = " + gameData.deathCount);
    }   

    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

         // TODO - save that data to a file using the data handler
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectOfType<MonoBehaviour>()
            .OfType<DataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
