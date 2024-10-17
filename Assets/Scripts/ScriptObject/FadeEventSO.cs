using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color,float,bool> FadeEvent;
    // 黑屏
    public void FadeIn(float duration){
        RaiseEvent(Color.black,duration,true);
    }
    //透明
    public void FadeOut(float duration){
        RaiseEvent(Color.clear,duration,false);
    }

    public void RaiseEvent(Color target,float duration,bool fadeIn){
        FadeEvent?.Invoke(target,duration,fadeIn);
    }
}
