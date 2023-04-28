using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;
    private List<CatalogItem> _catalog = new List<CatalogItem>();

    public event Action PurchaseSuccessfulEvent;

    private const string catalogversion = "1";

    public void Awake() {
        if(!Instance) {
            Instance = this;
            Init();
            return;
        }

        if (Instance && Instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    public void Init()
    {
        PlayFabClientAPI.GetCatalogItems( new GetCatalogItemsRequest()
        {
            CatalogVersion = catalogversion
        }, (success) =>
        {
            _catalog = success.Catalog;


            if (success.Catalog == null)
            {
                Debug.LogError( success.ToJson());
            }
            else
            {
                foreach (var item in success.Catalog)
                {
                    var currencyValues = "";

                    if (item.VirtualCurrencyPrices != null)
                    {
                        currencyValues = item.VirtualCurrencyPrices.Aggregate(currencyValues, (current, keyValuePair) => current + $"\nCurrency:{keyValuePair.Key} Price:{keyValuePair.Value}");
                    }

                    Debug.Log($"Name:{item.DisplayName} \nDescription:{item.Description} {currencyValues}");
                }
            }

        }, (fail) =>
        {
            
        });
    }
    
    public void PurchaseItem(string itemId, CurrencyManager.VirtualCurrency currency = CurrencyManager.VirtualCurrency.CO)
    {
        var itemPurchase = _catalog.FirstOrDefault(s => s.ItemId == itemId);
        
        if (itemPurchase == null)
        {
            Debug.LogError($"Item: {itemId} not found");
            return;
        }

        //PlayfabClientAPI.PurchaseItemRequest

        var currencyToUse = currency.ToString();
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest()
        {
            CatalogVersion = catalogversion,
            ItemId = itemPurchase.ItemId,
            Price = (int)itemPurchase.VirtualCurrencyPrices[currencyToUse],
            VirtualCurrency = currencyToUse
        }, (success) =>
        {
            Debug.Log("Purchase Successful");
            PurchaseSuccessfulEvent?.Invoke();
            
            
        }, (fail) =>
        {
            Debug.Log("Purchase Failed");
        });
    }

    public void PurchaseAmmo() {
        PurchaseItem("AM", CurrencyManager.VirtualCurrency.CO);
    }
    public void Purchase100Ammo() {
        PurchaseItem("AM100", CurrencyManager.VirtualCurrency.CO);
    }

    public void PurchaseMedkit() {
        PurchaseItem("MK", CurrencyManager.VirtualCurrency.CO);
    }
}
