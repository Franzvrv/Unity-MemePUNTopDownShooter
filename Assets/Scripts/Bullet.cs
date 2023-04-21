using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
     private void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
        GameObject Player = gameObject.GetComponent<GameObject>();

        // if (collision.transform.GetComponent(typeof(Enemy)))
        // {
        //     Enemy enemy = collision.transform.GetComponent<Enemy>();
        //     enemy.DamageEnemy(3);
        // }
      
    }
}
