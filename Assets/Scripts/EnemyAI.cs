using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isGround, isPlayer;
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

        if (Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
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

    // public IEnumerator startJumpscare()
    // {
    //     AudioManager.Instance.PlayAudio("EnemyJumpscare");

    //     jumpScare.SetActive(true);
    //     yield return new WaitForSeconds(1f);
    //     gameOver.SetActive(true);
    //     firstPersonControllerScript.enabled = false;
    //     playerScript.enabled = false;
    //     Cursor.visible = true;
    //     Cursor.lockState=CursorLockMode.None;
    // }

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
