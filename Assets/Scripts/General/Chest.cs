using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isDone;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable(){
        spriteRenderer.sprite = isDone?openSprite:closeSprite;
    }
    public void TriggerAction()
    {
        Debug.Log("Chest opened!");
        if(isDone) return;
        OpenChest();
    }

    private void OpenChest()
    {
        isDone = true;
        GetComponent<SpriteRenderer>().sprite = openSprite;
        this.gameObject.tag = "Untagged";
    }
}
