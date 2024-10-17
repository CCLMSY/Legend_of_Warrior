using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,IInteractable
{
    [Header("广播")]
    public VoidEventSO saveGameEvent;

    [Header("参数")]
    public SpriteRenderer spriteRenderer;
    public GameObject lightObj;
    public Sprite darkSprite;
    public Sprite lightSprite;

    public bool isDone = false;
    private void OnEnable(){
        spriteRenderer.sprite = isDone?lightSprite:darkSprite;
        lightObj.SetActive(isDone);
    }

    public void TriggerAction(){
        if(isDone) return;
        isDone = true;
        spriteRenderer.sprite = lightSprite;
        lightObj.SetActive(true);
        saveGameEvent.RaiseEvent();
        this.gameObject.tag = "Untagged";
    }
}
