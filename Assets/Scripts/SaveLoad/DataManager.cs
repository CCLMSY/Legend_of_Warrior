using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using System.IO;

[DefaultExecutionOrder(-100)]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [Header("事件监听")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
    private string jsonFolder;

    private void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(this.gameObject);
        }
        saveData = new Data();
        jsonFolder = Application.persistentDataPath + "/SaveData/";
        ReadSavedData();
    }

    private void OnEnable(){
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }

    private void OnDisable(){
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }

    private void Update(){
        if(Keyboard.current.lKey.wasPressedThisFrame){
            Load();
        }
    }

    public void RegisterSaveData(ISaveable saveable){
        if(!saveableList.Contains(saveable)){
            saveableList.Add(saveable);
        }
    }

    public void UnregisterSaveData(ISaveable saveable){
        saveableList.Remove(saveable);
    }
    
    public void Save(){
        foreach (var saveable in saveableList){
            saveable.GetSaveData(saveData);
        }   

        var result_path = jsonFolder + "Data.sav";
        var json_data = JsonConvert.SerializeObject(saveData);

        if(!File.Exists(result_path)){
            Directory.CreateDirectory(jsonFolder);
        }
        File.WriteAllText(result_path, json_data);

        // foreach (var item in saveData.characterPositions){
        //     Debug.Log(item.Key+": "+item.Value);
        // }
    }

    public void Load(){
        foreach(var saveable in saveableList){
            saveable.LoadData(saveData);
        }
    }

    private void ReadSavedData(){
        var result_path = jsonFolder + "Data.sav";
        if(File.Exists(result_path)){
            var stringData = File.ReadAllText(result_path);
            var json_data = JsonConvert.DeserializeObject<Data>(stringData);
            saveData = json_data;
        }

    }
}
