using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤无敌时间")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    // 启用事件
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;


    private void Start() 
    {
        currentHealth = maxHealth;
        
    }

    private void Update() 
    {
        if( invulnerable ) 
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0) 
            {
                invulnerable = false;
            }
        }
    }

    public void TakeDamage(Attack attacker) 
    {
        // Debug.Log("TakeDamage: " + attacker.damage);
        if (invulnerable) 
            return;

        // 防止当前血量数值显示异常
        if (currentHealth - attacker.damage > 0) {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            // TODO 执行受伤相关
            OnTakeDamage?.Invoke(attacker.transform);
            
        } else {
            // TODO 触发死亡相关
            currentHealth = 0;
            OnDead?.Invoke();
        }
        
    }

    public void TriggerInvulnerable()
    {
        if(!invulnerable) 
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
