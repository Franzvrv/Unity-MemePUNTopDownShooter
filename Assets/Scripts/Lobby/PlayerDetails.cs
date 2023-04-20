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
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _healthRechargeText;

    public Text HealthText { get => _healthText;}

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
        PlayFabClientAPI.GetPlayerCombinedInfo(
            request, OnSuccess, OnError 
        );
    }

    private void OnSuccess(GetPlayerCombinedInfoResult result)
    {
        foreach (var item in result.InfoResultPayload.UserVirtualCurrencyRechargeTimes)
        {
            switch(item.Key) {
                case "HP":
                    _healthRechargeText.text = item.Value.SecondsToRecharge.ToString();
                    break;
                default:
                    Debug.Log(item.Key + " currency not found");
                    break;
            }
        }

        foreach (var item in result.InfoResultPayload.UserVirtualCurrency)
        {
            //_currencyManager.Init(item.Key, item.Value);
            Debug.Log($"{item.Key}: {item.Value}");
            switch(item.Key) {
                case "CO":
                    _coinText.text = item.Value.ToString();
                    break;
                case "HP":
                    _healthText.text = item.Value.ToString();
                    if (item.Value == 5) {
                        _healthRechargeText.text = "";
                    }
                    break;
                default:
                    Debug.Log(item.Key + " currency not found");
                    break;
            }
        }


    }

    IEnumerator refreshCoroutine() {
        while(true) {
            GetPlayerData();
            //Debug.Log("a");
            yield return new WaitForSecondsRealtime(1);
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error getting user data: " + error.ErrorMessage);
    }
}
