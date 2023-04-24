using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient) {
            StartCoroutine(SpawnRoutine());
        }
        
    }

    // public void Spawn(int id) {
    //     switch (id) {
    //         case 0:
    //             PhotonNetwork.Instantiate("Enemy", transform.position, Quaternion.identity);
    //             break;
    //         // case 1:
    //         //     Instantiate(quickmelee, transform.position, Quaternion.identity);
    //         //     break;
    //         // case 2:
    //         //     Instantiate(kamikaze, transform.position, Quaternion.identity);
    //         //     break;
    //         default:
    //             Debug.Log("Tried to spawn enemy with invalid id");
    //             break;
    //     }
    // }

    IEnumerator SpawnRoutine() {
        //while (GameManager.Instance.canSpawn) 
        //while (true)
        //{
            // if (GameManager.Instance.zombieCount < zombieLimit + (GameManager.Instance.GetCoins() * 2)) {
            //     int zombieID = Random.Range(0,3);
            //     GameManager.Instance.AddZombie();
            //     Spawn(zombieID);
            //     yield return new WaitForSeconds(spawnRate);
                
            // }
            // else {
            //     yield return new WaitForSeconds(20);
            // }
            PhotonNetwork.Instantiate("Prefabs/Enemy", transform.position, Quaternion.identity);
        //}
        yield return new WaitForSeconds(20);
        //yield break;
    }
}
