using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAim : MonoBehaviourPun
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LayerMask aimMask;
    private Rigidbody rb;
    [SerializeField] float bulletForce = 10f;
    [SerializeField] private GameObject _cameraPrefab;
    public Camera mainCamera;


    Vector3 mousepos;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        if (photonView.IsMine) {
            GameObject camera = new GameObject();
            camera = Instantiate(_cameraPrefab, _cameraPrefab.transform);
            camera.transform.SetParent(playerTransform, false);
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (photonView.IsMine) {
            Aim();
            if (Input.GetMouseButtonDown(0))
            {
                //SoundManager.Playsound("Bang");
                Shoot();
            }
        }
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

    [PunRPC]
    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate("Prefabs/Bullet", firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}
