using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;

public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] int health = 100;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask ground, isPlayer;
    public Vector3 walkPoint;
    bool isWPSet;
    public float walkPointRange;

    public float sightRange, attackRange;
    public bool playerInSight, playerInAttackRange, agro = false, jumpscare = false;
    public float WPSetTimer = 7;

    private void Awake()
    {
        
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void Update()
    {
        // playerInSight = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        // playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        // if (!playerInSight && !playerInAttackRange) Patrolling();
        // if (playerInSight && !playerInAttackRange) Chasing() ;
        //if (playerInSight && playerInAttackRange) Attacking();
        if (player == null) {
            player = GameManager.Instance.players[0].transform;
        } else {
            Chasing() ;
        }
    }

    private void Chasing()
    {
        // if (!agro) {
        //     //AudioManager.Instance.PlaySpatialAudio("EnemyDetection", transform.position);
        //     agro = true;
        // }
        agent.SetDestination(player.position);
    }

    public void DamageEnemy(int damage)
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

    IEnumerator playerSearch() {
        
        yield break;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
