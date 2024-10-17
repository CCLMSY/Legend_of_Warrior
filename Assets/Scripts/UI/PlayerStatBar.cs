using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    private Character curCharacter;
    public Image healthImg;
    public Image healthDelayImg;
    public Image powerImg;

    private bool isRecovering;

    private void Update(){
        if(healthDelayImg.fillAmount > healthImg.fillAmount){
            healthDelayImg.fillAmount -= Time.deltaTime;
        }

        if(isRecovering){
            float percentage = curCharacter.curPower / curCharacter.maxPower;
            powerImg.fillAmount = percentage;

            if(percentage >= 1){
                isRecovering = false;
                return ;
            }
        }
    }
    /// <summary>
    /// 接收HealthBar的百分比
    /// </summary>
    /// <param name="percentage">百分比</param>
    public void OnHealthChange(float percentage){
        healthImg.fillAmount = percentage;
    }

    public void OnPowerChange(Character character){
        isRecovering = true;
        curCharacter = character;
    }
}
