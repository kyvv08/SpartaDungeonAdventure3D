using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController playerController;
    private void Awake()
    {
        CharacterManager.Instance.SetPlayer(this);
        playerController = GetComponent<PlayerController>();
    }
    
    public void AddJumpForce(float jumpForce)
    {
        Debug.Log(jumpForce);
        GetComponent<Rigidbody>().AddForce(Vector2.up*jumpForce,ForceMode.Impulse);
    }

    public void InteractItem(ItemData item)
    {
        for (int i = 0; i < item.consumables.Length; ++i)
        {
            switch (item.consumables[i].type)
            {
                case ConsumableType.Speed:
                {
                    playerController.AddSpeed(item.consumables[i].value, item.consumables[i].time);
                    break;
                }
                case ConsumableType.Health:
                {
                    playerController.AddHealth(item.consumables[i].value);
                    break;
                }
            }
        }
    }
}
