using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;

    public static CharacterManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }

            return instance;
        }
    }

    private Player player;

    public Player Player
    {
        get { return player; }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }
    
}
