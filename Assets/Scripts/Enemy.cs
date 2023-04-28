//using System
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;

public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] private int health = 100;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private int targetPlayerID = 0;
    [SerializeField] private int attack = 5;
    private NavMeshAgent agent;
    private LayerMask ground, isPlayer;
    private Vector3 walkPoint;
    
    public float WPSetTimer = 7;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        StartCoroutine(playerSearch());
    }

    private void Update()
    {
        



        //Debug.Log(GameManager.Instance.players.Count + " - " + closestDistance);
    }

    void FixedUpdate() {
        if (targetPlayerID != 0) {
            agent.SetDestination(PhotonView.Find(targetPlayerID).gameObject.transform.position);
        }
    }

    IEnumerator playerSearch() {
        while (true) {
        float closestDistance = 9999999999;
            foreach (int playerID in GameManager.Instance.playerIDs) {
                GameObject player = PhotonView.Find(playerID).gameObject;
                if (Vector3.Distance(player.transform.position, transform.position) < closestDistance) {
                    targetPlayerID = playerID;
                    closestDistance = Vector3.Distance(player.transform.position, transform.position);
                }
            }
            yield return new WaitForSeconds(1.1f);
        }
    }

    public void DamageEnemy(int damage, int playerID) {
        photonView.RPC(nameof(DamageEnemyPun), RpcTarget.All, damage, playerID);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.GetComponent(typeof(PlayerInfo)))
        {
            PlayerInfo player = collision.transform.GetComponent<PlayerInfo>();
            player.DamagePlayer(attack);
        }
    }

    [PunRPC]
    public void DamageEnemyPun(int damage, int playerID)
    {
        health -= damage;
        if (health <= 0)
        {
            PhotonView.Find(playerID).gameObject.GetComponent<PlayerInfo>().IncreaseKills();
            PhotonNetwork.Instantiate("Prefabs/Coin", transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
