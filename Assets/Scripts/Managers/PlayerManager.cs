using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    private static PlayerManager Instance;
    [SerializeField] private Transform _spawnLocation;
    private static GameObject localPlayer;
    private GameObject player;
    
    // Start is called before the first frame update
    void Awake() {
        if(!Instance) {
            Instance = this;
            return;
        }

        if (Instance && Instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        player = PhotonNetwork.Instantiate("Player", _spawnLocation.transform.position, Quaternion.identity);
        int id = player.GetComponent<PhotonView>().ViewID;
        photonView.RPC(nameof(AddPlayerList), RpcTarget.All, id);
    }

    [PunRPC] 
    private void AddPlayerList(int playerID) {
        GameManager.Instance.AddPlayer(playerID);
    }
}
