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
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask ground, isPlayer;
    public Vector3 walkPoint;
    //bool isWPSet;
    //public float walkPointRange;

    //public float sightRange, attackRange;
    //public bool playerInSight, playerInAttackRange, agro = false, jumpscare = false;
    public float WPSetTimer = 7;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
    }

    private void Update()
    {
        Transform closestPlayer = null;
        float closestDistance = 9999999999;

        foreach (GameObject player in GameManager.Instance.players) {
            if (closestPlayer == null || Vector3.Distance(player.transform.position, transform.position) < closestDistance) {
                closestPlayer = player.transform;
                closestDistance = Vector3.Distance(player.transform.position, transform.position);
            }
        }

        agent.SetDestination(closestPlayer.position);
    }

    IEnumerator playerSearch() {
        yield break;
    }

    public void DamageEnemy(int damage) {
        photonView.RPC(nameof(DamageEnemyPun), RpcTarget.All, damage);
    }

    [PunRPC]
    public void DamageEnemyPun(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
