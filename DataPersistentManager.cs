using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class DataPersistentManager : MonoBehaviour
{
    public GameData gameData;
    public DataInitializer DataInit;
    public static DataPersistentManager instance { get; private set;}
    private List<IDataPersistence> dataPersistenceObjects;

    private void Awake()
    {
        if (instance != null)
        {
            // Debug.LogError("Found more than one Data Persistence Manager in the scene.");
            Destroy(this.transform.parent.gameObject);
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        // DataInit = gameObject.GetComponent<DataInitializer>();
        // DontDestroyOnLoad(this.gameObject);
    }

    // private void Start()
    // {
    //     LoadGame();
    // }

    public void NewGame()
    {
        this.gameData = new GameData(DataInit);
    }

    public void LoadGame()
    {
        //ToDo - Load any saved data from a file using the data handler
        // if no data can be loaded, initialize to a new game
        if(this.gameData.gameDay == 0)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
        else
        {
            Debug.Log("Found GameData object");
            Debug.Log("Game Day " + gameData.gameDay);
            Debug.Log("coins: " + gameData.coins.ToString());
            Debug.Log("points: " + gameData.points.ToString());
        }
        
        UpdateData();
        
        Debug.Log("LoadGame is working, loading all the dataPersistenceObj in the scene...");
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            Debug.Log("persistence located in: " + dataPersistenceObj);
            dataPersistenceObj.LoadData(gameData);
        }

        //ToDo - push the loaded data to all other scripts that need it
    }

    public void SaveGame()
    {
        if(this.gameData == null)
        {
            Debug.LogWarning("no data was found!! A new game needs to be started before data can be saved");
            return;
        }
        Debug.Log("SaveGame is working");
        //ToDo - pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        //ToDo - save that data to a file using the data handler.

    }

    //update all level-related informations including the unlockable items and settings/configurations
    public void UpdateData()
    {
        UpdateBuyableItem();
    }

    public void UpdateBuyableItem()
    {
        foreach(KeyValuePair<ItemObject, bool> item in gameData.BuyableItem.ToList())
        {
            if(item.Key.minUnlockedLevel >= gameData.gameDay)
            {
                gameData.BuyableItem[item.Key] = true;
            }
        }
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        Debug.Log("OnSceneLoaded is working");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded is working");
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
