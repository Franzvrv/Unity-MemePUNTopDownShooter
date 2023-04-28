using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun, IPunObservable
{
    public static GameManager Instance;
    [SerializeField] public List<int> playerIDs = new List<int>();

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

    public void AddPlayer(int viewID) {
        playerIDs.Add(viewID);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(playerIDs);
        } else
        {
            //playerIDs = (List<int>)stream.ReceiveNext();
        }
    }
}
