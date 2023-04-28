using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    public static EnemySpawner Instance;
    [SerializeField] private float spawnRate = 3;
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private int enemyCount = 0;

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

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient) {
            StartCoroutine(SpawnRoutine());
        }
    }

    public void Spawn(int id) {
        switch (id) {
            case 1:
                PhotonNetwork.Instantiate("Prefabs/Enemy1", transform.position, Quaternion.identity);
                enemyCount++;
                break;
            case 2:
                PhotonNetwork.Instantiate("Prefabs/Enemy2", transform.position, Quaternion.identity);
                enemyCount++;
                break;
            default:
                Debug.Log("Tried to spawn enemy with invalid id");
                break;
        }
    }

    public void ReduceEnemyCount() {
        enemyCount--;
    }

    IEnumerator SpawnRoutine() {
        while (true)
        {
            if(enemyCount <= maxEnemies) {
                Spawn(Random.Range(1, 3));
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
