using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤无敌时间")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;


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
        // Debug.Log(attacker.damage);
        if (invulnerable) 
            return;

        // 防止当前血量数值显示异常
        if (currentHealth - attacker.damage > 0) {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
        } else {
            currentHealth = 0;
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
