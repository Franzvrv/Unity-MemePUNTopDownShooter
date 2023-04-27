using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FinishBlock : MonoBehaviourPunCallbacks
{
    private bool finished = false;
    private int winnerId;
    private GameObject _endScreen;

    [SerializeField] PlayerInfo _playerInfo;

    private void OnTriggerEnter2D(Collider2D collider) {
        if(!finished && collider.GetComponent<PlayerMovement>()) {

            //winnerId = collider.GetComponent<PlayerMovement>().photonView.ControllerActorNr;

            finished = true;
            if(PhotonNetwork.LocalPlayer.ActorNumber == winnerId) {
                ShowWinScreen();
                //_playerInfo.AddCurrency(PlayerInfo.VirtualCurrency.CO, 5);
            } else {
                //ShowLoseScreen();
                //_playerInfo.SubtractCurrency(PlayerInfo.VirtualCurrency.HP, 1);
            }
        }
    }

    private void ShowWinScreen() {
        _endScreen = Instantiate(Resources.Load("Endscreen", typeof(GameObject))) as GameObject;
        _endScreen.GetComponent<EndScreen>().Initialize(null);
    }

    // [PunRPC]
    // private void LoseCondition()
    // {
    //     PlayFabClientAPI.UpdatePlayerStatistics( new UpdatePlayerStatisticsRequest()
    //     {
    //         Statistics  = { new StatisticUpdate() {StatisticName = "Failures", Value = 1, Version = 0}}
    //     }, (updateSuccess) =>
    //     {
                    
    //     }, (updateFailure) => { });      
    // }
}
