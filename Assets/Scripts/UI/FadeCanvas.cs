using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    [Header("事件监听")]
    public FadeEventSO fadeEvent;
    public Image fadeImage;

    private void OnFadeEvent(Color target,float duration){
        fadeImage.DOBlendableColor(target,duration);
    }

    private void OnEnable(){
        fadeEvent.FadeEvent += OnFadeEvent;
    }

    private void OnDisable(){
        fadeEvent.FadeEvent -= OnFadeEvent;
    }

    private void OnFadeEvent(Color target, float duration, bool fadeIn)
    {
        fadeImage.DOBlendableColor(target,duration);
    }
}
