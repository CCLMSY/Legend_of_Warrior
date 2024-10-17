using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, ISaveable
{
    [Header("事件监听")]
    public VoidEventSO newGameEventSO;

    [Header("基本属性")]
    public float maxHealth;
    public float curHealth;
    public float maxPower;
    public float curPower;
    public float powerRecoverSpeed;

    [Header("受伤无敌")]
    public float invincibleTime;
    [HideInInspector] public float invincibleCounter;
    public bool isInvincible;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;

    private void NewGame()
    {
        OnHealthChange?.Invoke(this); //初始化血条显示
    }
    private void OnEnable(){
        curHealth = maxHealth;
        curPower = maxPower;
        newGameEventSO.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable(){
        newGameEventSO.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnregisterSaveData();
    }
    private void Update()
    {
        if (isInvincible)
        {
            invincibleCounter -= Time.deltaTime;
            if (invincibleCounter <= 0)
            {
                isInvincible = false;
            }
        }
        if(curPower<maxPower){
            curPower += powerRecoverSpeed * Time.deltaTime;
            if(curPower>maxPower){
                curPower = maxPower;
            }
            OnHealthChange?.Invoke(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Water")){
            if(curHealth<=0) return;
            curHealth = 0;
            OnHealthChange?.Invoke(this);
            OnDie?.Invoke();
        }
    }
    public void TakeDamage(Attack attacker)
    {
        if (isInvincible) return;
        if (curHealth > attacker.damage)
        {
            curHealth -= attacker.damage;
            TriggerInvincible();
            // 受伤处理
            OnTakeDamage?.Invoke(attacker.transform);
            TriggerInvincible();
        }
        else
        {
            curHealth = 0;
            // 死亡处理
            OnDie?.Invoke();
        }
        // 血量变化处理
        OnHealthChange?.Invoke(this);
    }
    private void TriggerInvincible()
    {
        if (!isInvincible)
        {
            isInvincible = true;
            invincibleCounter = invincibleTime;
        }
    }

    public void OnSlide(int cost){
        curPower -= cost;
        OnHealthChange?.Invoke(this);
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if(data.characterPositions.ContainsKey(GetDataID().ID)){
            data.characterPositions[GetDataID().ID] = new SerializeVector3(transform.position);
            data.floatValues[GetDataID().ID+"health"] = curHealth;
            data.floatValues[GetDataID().ID+"power"] = curPower;
        }else{
            data.characterPositions.Add(GetDataID().ID, new SerializeVector3(transform.position));
            data.floatValues.Add(GetDataID().ID+"health", curHealth);
            data.floatValues.Add(GetDataID().ID+"power", curPower);
        }
    }

    public void LoadData(Data data)
    {
        if(data.characterPositions.ContainsKey(GetDataID().ID)){
            transform.position = data.characterPositions[GetDataID().ID].ToVector3();
            curHealth = data.floatValues[GetDataID().ID+"health"];
            curPower = data.floatValues[GetDataID().ID+"power"];

            OnHealthChange?.Invoke(this);
        }else{
            Debug.Log("No position data found for " + gameObject.name);
        }
    }

}
