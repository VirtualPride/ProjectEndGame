using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    [SerializeField] private Item.ItemType itemType;

    public Item.ItemType GetItemType()
    {
        return itemType;
    }

    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}
