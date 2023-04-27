using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnLocation;
    private static GameObject localPlayer;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = PhotonNetwork.Instantiate("Player", this.transform.position, Quaternion.identity);
        GameManager.Instance.AddPlayer(player);
    }
}
