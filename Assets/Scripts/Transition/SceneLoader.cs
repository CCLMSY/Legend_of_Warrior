using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISaveable
{
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    [Header("事件监听")]
    public SceneLoadEventSO sceneLoadEventSO;
    public VoidEventSO newGameEventSO;
    public VoidEventSO backToMenuEventSO;

    [Header("广播")]
    public VoidEventSO afterSceneLoadedEventSO;
    public FadeEventSO fadeEventSO;
    public SceneLoadEventSO sceneUnloadedEventSO;

    [Header("场景")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    [SerializeField] private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;

    private bool isLoading;
    public float fadeDuration = 1f;

    private void Awake(){
        // Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        // currentLoadedScene = firstLoadScene;
        // currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);

    }

    private void Start(){
        // NewGame();
        sceneLoadEventSO.RaiseLoadRequestEvent(menuScene,menuPosition,true);
    }

    private void OnEnable(){
        sceneLoadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEventSO.OnEventRaised += NewGame;
        backToMenuEventSO.OnEventRaised += OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable(){
        sceneLoadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEventSO.OnEventRaised -= NewGame;
        backToMenuEventSO.OnEventRaised -= OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.UnregisterSaveData();
    }

    private void NewGame(){
        sceneToLoad=firstLoadScene;
        // OnLoadRequestEvent(sceneToLoad,firstPosition,true);
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToLoad,firstPosition,true);
    }
    private void OnBackToMenuEvent()
    {
        sceneToLoad = menuScene;
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToLoad,menuPosition,true);
    }

    private void OnLoadRequestEvent(GameSceneSO LocationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if(isLoading) return ;
        isLoading = true;
        sceneToLoad = LocationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if(currentLoadedScene != null){
            StartCoroutine(UnLoadPreviousScene());
        }else{
            LoadNewScene();
        }
    }


    private IEnumerator UnLoadPreviousScene(){
        if(fadeScreen){
            fadeEventSO.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);

        sceneUnloadedEventSO.RaiseLoadRequestEvent(sceneToLoad,positionToGo,fadeScreen);

        yield return currentLoadedScene.sceneReference.UnLoadScene();

        playerTrans.gameObject.SetActive(false);
        
        LoadNewScene();
    }

    private void LoadNewScene(){
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadedScene = sceneToLoad;

        playerTrans.position = positionToGo;

        playerTrans.gameObject.SetActive(true);

        if(fadeScreen){
            fadeEventSO.FadeOut(fadeDuration);
        }

        isLoading = false;

        if(currentLoadedScene.sceneType == SceneType.Menu) return;
        afterSceneLoadedEventSO.RaiseEvent();
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadedScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefinition>().ID;
        if(data.characterPositions.ContainsKey(playerID)){
            positionToGo = data.characterPositions[playerID].ToVector3();
            sceneToLoad = data.GetSavedScene();

            OnLoadRequestEvent(sceneToLoad,positionToGo,true);
        }
    }
}
