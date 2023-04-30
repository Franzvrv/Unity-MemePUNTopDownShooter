using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public event Action CheckBalanceSuccessEvent;
    private int _coAmount;


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

    void OnEnable() {
        CheckBalance();
    }

    public void CheckBalance()
    {
        PlayFabClientAPI.GetUserInventory( new GetUserInventoryRequest(), (success) =>
        {
            foreach (var keyValuePair in success.VirtualCurrency)
            {
                Init(keyValuePair.Key, keyValuePair.Value);
            }
            CheckBalanceSuccessEvent?.Invoke();
            InventoryManager.Instance.UpdateInventory(success.Inventory);
        }, (failure) =>
        {
            
        });
    }
    
    public void Init(string currencyCode, int balance) 
    { 
        switch (currencyCode)
        {
            case "CO":
                _coAmount = balance;
                break;
        }

    }

    public void AddCurrency(VirtualCurrency virtualCurrency, int amountToAdd)
    {
        PlayFabClientAPI.AddUserVirtualCurrency(new PlayFab.ClientModels.AddUserVirtualCurrencyRequest()
        {
            Amount = amountToAdd,
            VirtualCurrency = virtualCurrency.ToString()
        }, OnSuccessfulModifyCurrency, OnFailedModifyCurrency);
    }

    public void SubtractCurrency(VirtualCurrency virtualCurrency, int amountToAdd)
    {
        PlayFabClientAPI.SubtractUserVirtualCurrency(new PlayFab.ClientModels.SubtractUserVirtualCurrencyRequest()
        {
            Amount = amountToAdd,
            VirtualCurrency = virtualCurrency.ToString()
        }, OnSuccessfulModifyCurrency, OnFailedModifyCurrency);
    }

    private void OnSuccessfulModifyCurrency(ModifyUserVirtualCurrencyResult result)
    {
          switch (result.VirtualCurrency)
        {
                case "CO":
                    _coAmount = result.Balance;
                break;
        }   
    }

    private void OnFailedModifyCurrency(PlayFabError error)
    {
        Debug.LogError(error.ToString());
    }

    public enum VirtualCurrency
    {
        CO
    }
}
