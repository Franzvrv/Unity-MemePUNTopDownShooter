using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayerDetails : MonoBehaviour
{
    public static PlayerDetails instance;
    [SerializeField] private Text _coinText;
    [SerializeField] private Text _WinsText;
    [SerializeField] private Text _KillsText;
    [SerializeField] private Text _TimeText;
    [SerializeField] private Text _AmmoText;
    [SerializeField] private Text _MedkitText;

    private bool refreshing = false;

    void Start()
    {
        if (instance == null) {
            instance = this;
        }
    }

    void OnEnable() {
        if(!refreshing) {
            StartCoroutine(refreshCoroutine());
        }
    }

    public void GetPlayerData() {
        var request = new GetPlayerCombinedInfoRequest();
        request.InfoRequestParameters = new GetPlayerCombinedInfoRequestParams();
        request.InfoRequestParameters.GetUserVirtualCurrency = true;
        request.InfoRequestParameters.GetPlayerStatistics = true;
        request.InfoRequestParameters.GetUserInventory = true;
        PlayFabClientAPI.GetPlayerCombinedInfo(
            request, OnSuccess, OnError 
        );
    }

    private void OnSuccess(GetPlayerCombinedInfoResult result)
    {
        // foreach (var item in result.InfoResultPayload.UserVirtualCurrencyRechargeTimes)
        // {
        //     switch(item.Key) {
        //         case "CO":
        //             _healthRechargeText.text = item.Value.SecondsToRecharge.ToString();
        //             break;
        //         default:
        //             Debug.Log(item.Key + " currency not found");
        //             break;
        //     }
        // }

        foreach (var item in result.InfoResultPayload.UserVirtualCurrency)
        {
            switch(item.Key) {
                case "CO":
                    _coinText.text = item.Value.ToString();
                    break;
                default:
                    Debug.Log(item.Key + " currency not found");
                    break;
            }
        }

        if (result.InfoResultPayload.PlayerStatistics != null)
        {
            foreach (var statistic in result.InfoResultPayload.PlayerStatistics)
            {
                switch(statistic.StatisticName) {
                    case "Wins":
                        _WinsText.text =  "Wins: " + statistic.Value.ToString();
                    break;
                    case "Kills":
                        _KillsText.text = "Kills: " + statistic.Value.ToString();
                    break;
                    case "Time":
                        _TimeText.text = "Time: " + statistic.Value.ToString();
                    break;
                    default:
                        Debug.Log("Unknown statistic " + statistic.StatisticName);
                    break;
                }
            }
        } else {
            Debug.Log("No player statistics found");
        }

        if (result.InfoResultPayload.UserInventory != null)
        {
            foreach (var item in result.InfoResultPayload.UserInventory)
            {
                //Debug.Log("Item: " + item.ItemId + " Amount: " + item.RemainingUses);
                switch(item.ItemId) {
                    case "AM":
                        _AmmoText.text =  "Ammo: " + item.RemainingUses;
                    break;
                    case "MK":
                        _MedkitText.text = "Medkits: " + item.RemainingUses;
                    break;
                    default:
                        Debug.Log("Unknown Item " + item.ItemId);
                    break;
                }
            }
        } else {
            Debug.Log("No player inventory found");
        }
    }

    IEnumerator refreshCoroutine() {
        while(true) {
            GetPlayerData();
            yield return new WaitForSecondsRealtime(1);
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error getting user data: " + error.ErrorMessage);
    }
}
