using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public Action onTakeDamage;
    
    Condition health { get { return UIManager.Instance.health; } }
    Condition stamina { get { return UIManager.Instance.stamina; } }

    private void Update()
    {
        health.Add(health.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (health.curValue < 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Damaged(float amount)
    {
        onTakeDamage?.Invoke();
        health.Subtract(amount);
    }
    
    public void UseStamina(float amount)
    {
        stamina.Subtract(amount);
    }
    public void Die()
    {
        Debug.Log("플레이어 사망");
    }
}
