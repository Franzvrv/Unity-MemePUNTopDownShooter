using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviourPun
{
    [SerializeField] private int value = 100;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.GetComponent(typeof(PlayerInfo)))
        {
            PlayerInfo playerInfo = collider.transform.GetComponent<PlayerInfo>();
            playerInfo.AddCurrency(PlayerInfo.VirtualCurrency.CO, value);
        }
    }
}
