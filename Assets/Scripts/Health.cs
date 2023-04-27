using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private Slider slider;
    void Awake()
    {
        slider = this.GetComponent<Slider>();
    }

    void Update()
    {
        //slider.value = GameManager.Instance.GetHealth();
    }
}