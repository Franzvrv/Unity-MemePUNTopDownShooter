using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class InventoryManager : MonoBehaviour
{
    private List<ItemInstance> _currentInventory = new List<ItemInstance>();

    public static InventoryManager Instance;

    void Awake() {
        if(!Instance) {
            Instance = this;
            return;
        }

        if (Instance && Instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    public void UpdateInventory(List<ItemInstance> items)
    {
        _currentInventory = items;
    }

    public void UseItem(Item itemId, int itemCount, Action successCallback)
    {
        var itemToUse = _currentInventory.Find(s => s.ItemId == itemId.ToString());
        
        if (itemToUse == null)
        {
            Debug.LogError($"Item: {itemId} not found");
            return;
        }


        
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest()
        {
            ItemInstanceId = itemToUse.ItemInstanceId,
            ConsumeCount = itemCount
        }, (success) =>
        {   
            Debug.Log("Item successfully used");
            successCallback?.Invoke();
            for( var i = 0; i < _currentInventory.Count; i++)
            {
                if (_currentInventory[i].ItemInstanceId == success.ItemInstanceId)
                {
                    _currentInventory[i].RemainingUses = success.RemainingUses;
                    break;
                }
            }
        }, (fail) =>
        {
            Debug.Log("Item Use Failed");
        });
    }

    public enum Item {
        AM,
        MK
    }
}
