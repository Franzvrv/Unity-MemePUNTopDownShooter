using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVicinity : MonoBehaviour
{
    public bool gettingUp = false;
    [SerializeField] public PlayerMovement playerMovement;
    [SerializeField] private float getUpTime = 5f; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gettingUp) {
            getUpTime -= Time.deltaTime;
            if (getUpTime <= 0) {
                //playerMovement.GetUp();
            }
        } else {
            getUpTime = 5;
        }
    }
}
