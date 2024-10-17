using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO afterSceneLoadedEventSO;
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource inpulseSource;
    public VoidEventSO cameraShakeEvent;
    private void Awake(){
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable(){
        cameraShakeEvent.OnEventRaised += OnCameraShake;
        afterSceneLoadedEventSO.OnEventRaised += OnAfterSceneLoaded;
    }

    private void OnDisable(){
        cameraShakeEvent.OnEventRaised -= OnCameraShake;
        afterSceneLoadedEventSO.OnEventRaised -= OnAfterSceneLoaded;
    }

    private void OnCameraShake()
    {
        inpulseSource.GenerateImpulse();
    }

    // private void Start(){
    //     GetNewCameraBound();    
    // }

    private void GetNewCameraBound(){
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if(obj == null){
            Debug.LogError("Can't find the object with tag 'Bounds'");
            return;
        }

        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        confiner2D.InvalidateCache();

    }
    
    private void OnAfterSceneLoaded()
    {    
        GetNewCameraBound();
    }
}
