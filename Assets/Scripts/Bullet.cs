using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    [SerializeField] private float lifetime = 2;
    [SerializeField] private int _playerID;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    public void Init(int playerID) {
        _playerID = playerID;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            photonView.RPC(nameof(DestroyThis), RpcTarget.All);
        }
    }
    

    private void OnTriggerEnter(Collider collider)
    {
        GameObject Player = gameObject.GetComponent<GameObject>();

        if (collider.transform.GetComponent(typeof(Enemy)))
        {
            Enemy enemy = collider.transform.GetComponent<Enemy>();
            enemy.DamageEnemy(10, _playerID);
        }

        photonView.RPC(nameof(DestroyThis), RpcTarget.All);
    }

    [PunRPC]
    private void DestroyThis() {
        Destroy(this.gameObject);
    }
}
