using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("事件监听")]
    public PlayAudioEventSO FXAudioEventSO;
    public PlayAudioEventSO BGMAudioEventSO;
    public FloatEventSO volumeEventSO;
    public VoidEventSO pauseEventSO;
    
    [Header("广播")]
    public FloatEventSO SyncVolumeEventSO;

    [Header("音频源")]
    public AudioSource BGMSource;
    public AudioSource FXSource;

    public AudioMixer audioMixer;
    
    private void OnEnable(){
        FXAudioEventSO.OnEventRaised += OnFXAudioEvent;
        BGMAudioEventSO.OnEventRaised += OnBGMAudioEvent;
        volumeEventSO.OnEventRaised += OnVolumeEvent;
        pauseEventSO.OnEventRaised += OnPauseEvent;
    }


    private void OnDisable(){
        FXAudioEventSO.OnEventRaised -= OnFXAudioEvent;
        BGMAudioEventSO.OnEventRaised -= OnBGMAudioEvent;
        volumeEventSO.OnEventRaised -= OnVolumeEvent;
        pauseEventSO.OnEventRaised -= OnPauseEvent;
    }

    private void OnPauseEvent()
    {
        float amount;
        audioMixer.GetFloat("MasterVolume", out amount);
        SyncVolumeEventSO.RaiseEvent(amount);
    }
    private void OnVolumeEvent(float amount)
    {
        audioMixer.SetFloat("MasterVolume", amount*100-80);
    }

    private void OnFXAudioEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }

    private void OnBGMAudioEvent(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }
}
