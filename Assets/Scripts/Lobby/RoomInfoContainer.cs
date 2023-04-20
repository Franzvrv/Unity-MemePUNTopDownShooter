using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoContainer : MonoBehaviour
{
    [SerializeField] private Text _roomStats;
    [SerializeField] private Button _connectBtn;

    private RoomInfo info;

    public Button ConnectBtn { get => _connectBtn; set => _connectBtn = value; }

    public void UpdateRoomInfo(RoomInfo info)
    {
        _roomStats.text = $"{info.Name} | {info.PlayerCount}/{info.MaxPlayers}";
        ConnectBtn.onClick.RemoveAllListeners();
        ConnectBtn.onClick.AddListener(() => { PhotonNetwork.JoinRoom(info.Name); });
    }
}
