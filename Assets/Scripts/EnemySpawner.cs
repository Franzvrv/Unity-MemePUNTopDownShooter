using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    [SerializeField] private float spawnRate = 3;
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient) {
            StartCoroutine(SpawnRoutine());
        }
        
    }

    [SerializeField] public void Spawn(int id) {
        switch (id) {
            case 1:
                PhotonNetwork.Instantiate("Prefabs/Enemy1", transform.position, Quaternion.identity);
                break;
            case 2:
                PhotonNetwork.Instantiate("Prefabs/Enemy2", transform.position, Quaternion.identity);
                break;
            default:
                Debug.Log("Tried to spawn enemy with invalid id");
                break;
        }
    }

    IEnumerator SpawnRoutine() {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            Spawn(Random.Range(1, 3));
        }
    }
}
