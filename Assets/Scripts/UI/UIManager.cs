using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    [Header("事件监听")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public FloatEventSO syncVolumeEvent;

    [Header("广播")]
    public VoidEventSO pauseEvent;

    [Header("组件")]
    public GameObject gameOverPanel;
    public GameObject restartButton;
    public GameObject mobileTouch;
    public Button settingButton;
    public GameObject PausePanel;
    public Slider volumeSlider;

    private void Awake(){
        #if UNITY_STANDALONE
            mobileTouch.SetActive(false);
        #endif

        settingButton.onClick.AddListener(TogglePausePanel);
    }

    private void OnEnable(){
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent += OnUnloadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }

    private void OnDisable(){
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnUnloadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
    }

    private void TogglePausePanel(){
        if(PausePanel.activeInHierarchy){
            PausePanel.SetActive(false);
            Time.timeScale = 1;
        }else{
            pauseEvent.RaiseEvent();
            PausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100;
    }
    private void OnGameOverEvent()
    {
        Debug.Log("GameOver");
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartButton);
    }

    private void OnLoadDataEvent()
    {
        Debug.Log("Panel Off");
        gameOverPanel.SetActive(false);
    }
    private void OnUnloadEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        playerStatBar.gameObject.SetActive(sceneToLoad.sceneType != SceneType.Menu);
        
    }
    private void OnHealthEvent(Character character)
    {
        // 更新UI
        // 通过Character的属性计算百分比
        float percentage = character.curHealth / character.maxHealth;
        // 通过UIManager的引用更新UI
        playerStatBar.OnHealthChange(percentage);

        // 更新能量UI
        playerStatBar.OnPowerChange(character);
    }
}
