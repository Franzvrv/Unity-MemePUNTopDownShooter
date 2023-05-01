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
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text medkitText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private EndScreen endScreen;

    void Awake() {
        if (!Instance) {
            Instance = this;
            return;
        }

        if (Instance && Instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    // Update is called once per frame
    public void SetAmmo(string magazine, string ammo) {
        ammoText.text = magazine + "/" + ammo;
    }

    public void SetHealth(int health) {
        healthSlider.value = health * 100 / PlayerInfo.maxHealth;
    }

    public void SetCoinText(string text) {
        coinText.text = text;
    }

    public void SetMedkitText(string text) {
        medkitText.text = text;
    }

    public void InitEndScreen(bool win, int kills, int time) {
        endScreen.gameObject.SetActive(true);
        endScreen.Initialize(win, kills, time);
    }
}
