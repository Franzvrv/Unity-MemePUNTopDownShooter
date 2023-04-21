using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LayerMask aimMask;
    private Rigidbody rb;
    [SerializeField] float bulletForce = 10f;
    public Camera mainCamera;


    Vector3 mousepos;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        Aim();
        if (Input.GetMouseButtonDown(0))
        {
            //SoundManager.Playsound("Bang");
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }

    private (bool success, Vector3 position) GetMousePosition() {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimMask)) {
            return (success: true, position: hitInfo.point);
        } else {
            Debug.Log("Aim unsuccessful");
            return (success: false, position: Vector3.zero);
        }
    }

    private void Aim() {
        var (success, position) = GetMousePosition();
        if(success) {
            var direction = position - transform.position;
            transform.forward = direction;
        }
    }
}
