using System.Collections;
using System.Collections.Generic;
using PlayFab;
using Photon.Pun;
using UnityEngine;
using PlayFab.ClientModels;

public class FinishBlock : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerInfo playerWonInfo = new PlayerInfo();
    [SerializeField] private bool finished = false;
    private int winnerId;

    private void OnTriggerEnter(Collider collider) {
        if(!finished && collider.GetComponent<PhotonView>().IsMine && collider.GetComponent<PlayerInfo>()) {
            PhotonView _photonView = collider.GetComponent<PhotonView>();
            playerWonInfo = collider.GetComponent<PlayerInfo>();
            UpdatePlayerData();
            finished = true;
        }

    }

    public void UpdatePlayerData() {
        var request = new GetPlayerStatisticsRequest();
        PlayFabClientAPI.GetPlayerStatistics(
            request, OnSuccess, OnError
        );
    }

    private void OnSuccess(GetPlayerStatisticsResult result)
    {
        int Wins = 1, initialKills = 0, initialTime = 0;
        if (result != null)
        {
            foreach (var statistic in result.Statistics)
            {
                switch(statistic.StatisticName) {
                    case "Wins":
                        Wins = statistic.Value + 1;
                    break;
                    case "Kills":
                        initialKills = statistic.Value;
                    break;
                    case "Time":
                        initialTime = statistic.Value;
                    break;
                    default:
                        Debug.Log("Unknown statistic " + statistic.StatisticName);
                    break;
                }
            }
            playerWonInfo.FinishGame(initialTime, initialKills, Wins, true);
        } else {
            Debug.Log("No player statistics found");
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error getting user data: " + error.ErrorMessage);
    }

    [PunRPC]
    private void WinCondition()
    {
        PlayFabClientAPI.UpdatePlayerStatistics( new UpdatePlayerStatisticsRequest()
        {
            Statistics  = { new StatisticUpdate() {StatisticName = "Wins", Value = 1, Version = 1}}
        }, (updateSuccess) =>
        {
            Debug.Log("Player Won");
        }, (updateFailure) => { 
            Debug.Log("Statistic Update failed");
        });      
    }
}
