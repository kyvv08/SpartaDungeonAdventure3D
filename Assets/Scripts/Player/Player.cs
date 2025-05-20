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
}
