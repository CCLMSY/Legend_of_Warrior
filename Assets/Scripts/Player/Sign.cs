using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    private PlayerInputControl playerInput;
    private Animator anim;
    public Transform playerTrans;
    public GameObject signSprite;
    private IInteractable targetItem;
    private bool canPress;
    private void Awake(){
        anim = signSprite.GetComponent<Animator>();

        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }

    private void OnEnable(){
        InputSystem.onActionChange += OnActionChange;
        playerInput.GamePlay.Confirm.started += OnConfirm;
    }

    private void OnDisable(){
        canPress = false;
    }


    private void Update(){
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playerTrans.localScale;
    }
    
    private void OnConfirm(InputAction.CallbackContext context)
    {
        if(canPress){
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudio();
        }
    }
    private void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("Interactable")) {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
        
    }

    private void OnTriggerExit2D(Collider2D other){
        canPress = false;
    }
    
    //设备输入切换
    private void OnActionChange(object obj, InputActionChange change)
    {
        if(change==InputActionChange.ActionStarted){
            // Debug.Log(((InputAction)obj).activeControl.name);
            var d = ((InputAction)obj).activeControl.device;
            switch(d.device){
                case Keyboard:
                    anim.Play("keyboard");
                    break;
                case Gamepad:
                    anim.Play("ps");
                    break;
            }
        }
    }

}
