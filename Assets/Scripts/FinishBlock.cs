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

    void Awake() {
        //endScreen = Resources.Load("Endscreen");
    }
    private void OnTriggerEnter2D(Collider2D collider) {
        if(!finished && collider.GetComponent<PlayerMovement>()) {

            //winnerId = collider.GetComponent<PlayerMovement>().photonView.ControllerActorNr;

            finished = true;
            if(PhotonNetwork.LocalPlayer.ActorNumber == winnerId) {
                ShowWinScreen();
                _playerInfo.AddCurrency(PlayerInfo.VirtualCurrency.CO, 5);
            } else {
                ShowLoseScreen();
                _playerInfo.SubtractCurrency(PlayerInfo.VirtualCurrency.HP, 1);
            }
        }
    }

    private void ShowWinScreen() {
        _endScreen = Instantiate(Resources.Load("Endscreen", typeof(GameObject))) as GameObject;
        _endScreen.GetComponent<EndScreen>().Initialize(null);
    }

    [PunRPC] 
    void ShowLoseScreen() {
        _endScreen = Instantiate(Resources.Load("Endscreen", typeof(GameObject))) as GameObject;
        _endScreen.GetComponent<EndScreen>().Initialize(PhotonNetwork.CurrentRoom.GetPlayer(winnerId).NickName);
    }
}
