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
    public GameObject gameOver;
    public GameObject jumpScare;
    //public FirstPersonController firstPersonControllerScript;
    //public Player playerScript;

    public Vector3 walkPoint;
    bool isWPSet;
    public float walkPointRange;

    public float sightRange, attackRange;
    public bool playerInSight, playerInAttackRange, agro = false, jumpscare = false;
    public float WPSetTimer = 7;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void Update()
    {
        // playerInSight = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        // playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        // if (!playerInSight && !playerInAttackRange) Patrolling();
        // if (playerInSight && !playerInAttackRange) Chasing() ;
        //if (playerInSight && playerInAttackRange) Attacking();
        Chasing() ;
    }

    private void Patrolling()
    {
        agro = false;
        if (!isWPSet) SearchWalkPoint();

        if (isWPSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWP = transform.position - walkPoint;

        if (distanceToWP.magnitude < 1f || WPSetTimer <= 0) {
            isWPSet = false;
            WPSetTimer = 10;
            
        } else {
            WPSetTimer -= Time.deltaTime;
        }
            
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
            isWPSet = true;
    }

    private void Chasing()
    {
        // if (!agro) {
        //     //AudioManager.Instance.PlaySpatialAudio("EnemyDetection", transform.position);
        //     agro = true;
        // }
        agent.SetDestination(player.position);
    }
    private void Attacking()
    {
        if (!jumpscare) {
            jumpscare = true;
            agent.SetDestination(transform.position);
            //StartCoroutine(startJumpscare());
        }
        
        
        //Time.timeScale = 0f;
        //Debug.Log("You died");
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
