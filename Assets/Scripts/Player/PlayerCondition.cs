using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    Condition health { get { return UIManager.Instance.health; } }
    Condition stamina { get { return UIManager.Instance.stamina; } }

    private void Update()
    {
        health.Add(Time.deltaTime * 2f);
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

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }
}
