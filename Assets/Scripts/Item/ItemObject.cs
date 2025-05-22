using System.Collections;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.itemName}\n{data.description}";
        return str;
    }   

    public void OnInteract()
    {
        CharacterManager.Instance.Player.InteractItem(data);
        Destroy(gameObject);
    }
}
