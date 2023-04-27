using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerInfo : MonoBehaviourPun, IPunObservable
{
    [SerializeField] public const int maxHealth = 100;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _currentAmmo;
    [Header("Prefabs")]
    [SerializeField] private PlayerUI playerUI;
    void Awake() {
        if (photonView.IsMine) {
            playerUI.gameObject.SetActive(true);
        }
        _currentHealth = maxHealth;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_currentHealth);
            stream.SendNext(_currentAmmo);
        } else
        {
            _currentHealth = (int)stream.ReceiveNext();
            _currentAmmo = (int)stream.ReceiveNext();
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            playerUI.SetHealth(_currentHealth);
            //playerUI
        }
    }

    public void HealPlayer(int amount) {
        _currentHealth += amount;
        if (_currentHealth > maxHealth) {
            _currentHealth = maxHealth;
        }
    }

    public void DamagePlayer(int amount) {
        _currentHealth -= amount;
        if (_currentHealth < 0) {
            // down the player
        }
    }
}
