using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public PlayAudioEventSO playAudioEventSO;
    public AudioClip clip;
    public bool playOnEnable;

    private void OnEnable()
    {
        if(playOnEnable) PlayAudio();
    }

    public void PlayAudio()
    {
        playAudioEventSO.RaiseEvent(clip);
    }
}
