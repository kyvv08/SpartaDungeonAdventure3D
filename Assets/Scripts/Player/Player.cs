using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    
    public void AddJumpForce(float jumpForce)
    {
        Debug.Log(jumpForce);
        GetComponent<Rigidbody>().AddForce(Vector2.up*jumpForce,ForceMode.Impulse);
    }
}
