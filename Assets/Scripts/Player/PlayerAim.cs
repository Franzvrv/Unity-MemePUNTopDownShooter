using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAim : MonoBehaviourPun
{
    [Header("Gun Mechanics")]
    [SerializeField] float bulletForce = 10f;
    [SerializeField] int magazineCapacity;
    [SerializeField] public const int maxMagazineCapacity = 20;
    [SerializeField] int ammoCapacity;
    [Header("Essentials")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject _cameraPrefab, _playerUIPrefab;
    [SerializeField] private LayerMask aimMask;
    [SerializeField] private PhotonView playerPhotonView;
    [SerializeField] private PlayerMovement playerMovement;
    public PlayerUI playerUI;
    private Rigidbody rb;
    public Camera mainCamera;

    Vector3 mousepos;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        if (photonView.IsMine) {
            GameObject camera = Instantiate(_cameraPrefab, _cameraPrefab.transform);
            camera.transform.SetParent(playerTransform, false);
            GameObject _playerUI = Instantiate(_playerUIPrefab, _playerUIPrefab.transform);
            _playerUI.transform.SetParent(playerTransform, false);
            playerUI = _playerUI.GetComponent<PlayerUI>();
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (photonView.IsMine) {
            Aim();
            if (Input.GetMouseButtonDown(0) && ammoCapacity > 0)
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
            //Debug.Log("Aim unsuccessful");
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

    private void Reload() {
        
    }

    [PunRPC]
    private void Shoot()
    {
        ammoCapacity -= 1;
        GameObject bullet = PhotonNetwork.Instantiate("Prefabs/Bullet", firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<Bullet>().Init(playerPhotonView.ViewID);
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        
    }
}
