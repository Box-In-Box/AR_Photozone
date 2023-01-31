using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance = null;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(DataManager)) as DataManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _instance;
        }
    }

    string SettingDataFileName = "SettingData.json";

    public Data data = new Data();
    public AnimalData animalData = new AnimalData();

    public void LoadSettingData() 
    {
        string filePath = Application.persistentDataPath + "/" + SettingDataFileName;

        if(File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            Debug.Log("Load Data");
        }
    }

    public void SavaSettingData()
    {
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + SettingDataFileName;

        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("Save Data");
    }
}