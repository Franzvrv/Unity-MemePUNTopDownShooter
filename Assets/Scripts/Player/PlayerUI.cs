using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerUI : MonoBehaviourPun
{
    public static PlayerUI Instance;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text splashText;
    [SerializeField] private Slider healthSlider;

    void Awake() {
        if (photonView.IsMine) {
            Instance = this;
        }
    }

    // Update is called once per frame
    public void SetAmmo(int magazine, int ammo) {
        ammoText.text = magazine + " / " + ammo;
    }

    public void SetHealth(int health) {
        healthSlider.value = (health / PlayerInfo.maxHealth) / 100;
    }

    public void SetSplashText(string text) {

    }
}
