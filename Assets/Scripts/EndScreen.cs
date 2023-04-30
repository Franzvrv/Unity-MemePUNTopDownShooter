using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] TMP_Text _kills;
    [SerializeField] TMP_Text _time;
    public void Initialize(bool winner, int kills, int time) {
        if (winner) {
            _text.text = "Winner Winner Chicken Adobo";
        } else {
            _text.text = "Better Call Luck Next Time";
        }
        _kills.text = "You Killed " + kills + " Enemies";
        _time.text = "And Survived For " + time + " Seconds";
    }
}
