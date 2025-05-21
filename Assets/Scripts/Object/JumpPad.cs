using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpForce;

    public void OnCollisionEnter(Collision collision)
    {    
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().AddJumpForce(jumpForce);
        }
    }
}
