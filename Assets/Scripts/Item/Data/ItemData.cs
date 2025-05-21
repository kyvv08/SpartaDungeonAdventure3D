using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    Health,
    Speed
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
    public float time;
}

[CreateAssetMenu(fileName ="Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string description;
    public Sprite icon;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}