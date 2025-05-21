using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public Condition health { get; private set; }
    public Condition stamina { get; private set; }
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    public TextMeshProUGUI promptText {get; private set;}
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        promptText = transform.Find("PromptText").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        Transform condition = transform.Find("Conditions");
        health = condition.Find("Health").GetComponent<Condition>();
        stamina = condition.Find("Stamina").GetComponent <Condition>();
    }
}
