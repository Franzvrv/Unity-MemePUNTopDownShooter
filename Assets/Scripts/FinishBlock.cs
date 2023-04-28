using System.Collections;
using System.Collections.Generic;
using PlayFab;
using Photon.Pun;
using UnityEngine;
using PlayFab.ClientModels;

public class FinishBlock : MonoBehaviourPunCallbacks
{
    private bool finished = false;
    private int winnerId;
    private GameObject _endScreen;

    [SerializeField] PlayerInfo _playerInfo;

    private void OnTriggerEnter(Collider collider) {
        if(!finished && collider.GetComponent<PhotonView>()) {
            PhotonView _photonView = collider.GetComponent<PhotonView>();

            finished = true;
            // if(PhotonNetwork.LocalPlayer.ActorNumber == winnerId) {
            //     ShowWinScreen();
            //     _playerInfo.AddCurrency(PlayerInfo.VirtualCurrency.CO, 5);
            // } else {
            //     ShowLoseScreen();
            //     _playerInfo.SubtractCurrency(PlayerInfo.VirtualCurrency.HP, 1);
            // }
            _photonView.RPC(nameof(WinCondition), RpcTarget.All);
        }
    }

    private void ShowWinScreen() {
        _endScreen = Instantiate(Resources.Load("Endscreen", typeof(GameObject))) as GameObject;
        _endScreen.GetComponent<EndScreen>().Initialize(null);
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
