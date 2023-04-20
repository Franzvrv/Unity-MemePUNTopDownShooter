using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

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
                    Debug.Log($"CO:{result.Balance}");
                break;
            case "HP":
                Debug.Log($"HP:{result.Balance}");
                break;
        }   
    }

    private void OnFailedModifyCurrency(PlayFabError error)
    {
        Debug.LogError(error.ToString());
    }

    public enum VirtualCurrency
    {
        HP,
        CO
    }
}
