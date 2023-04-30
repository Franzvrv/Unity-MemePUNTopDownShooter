using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAim : MonoBehaviourPun
{
    [Header("Gun Mechanics")]
    [SerializeField] float bulletForce = 10f;
    [SerializeField] int magazineCapacity = maxMagazineCapacity;
    [SerializeField] public const int maxMagazineCapacity = 20;
    [SerializeField] private int ammoCapacity;
    [SerializeField] private bool reloading = false;
    [SerializeField] private float reloadTimeLeft;
    [SerializeField] public const float reloadTime = 5f;

    [Header("Essentials")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject _cameraPrefab, _playerUIPrefab;
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private LayerMask aimMask;
    [SerializeField] private PhotonView playerPhotonView;
    [SerializeField] private PlayerMovement playerMovement;
    public PlayerUI playerUI;
    private Rigidbody rb;
    public Camera mainCamera;

    Vector3 mousepos;

    public int MagazineCapacity { get => magazineCapacity; }
    public int AmmoCapacity { get => ammoCapacity; set => ammoCapacity = value; }

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
            if (Input.GetMouseButtonDown(0) && magazineCapacity > 0 && !reloading)
            {
                //SoundManager.Playsound("Bang");
                Shoot();
            }
            if (Input.GetKeyDown(KeyCode.R) && magazineCapacity < maxMagazineCapacity && ammoCapacity > 0 && !playerInfo.IsDown && !reloading) {
                StartCoroutine(reloadCoroutine());
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

    IEnumerator reloadCoroutine() {
        Debug.Log("Reloading");
        reloading = true;
        while (reloading) {
            yield return new WaitForSeconds(1);
            reloadTimeLeft--;
            if (reloadTimeLeft <= 0) {
                reloading = false;
                InventoryManager.Instance.UseItem(InventoryManager.Item.AM, 1, () => {
                    magazineCapacity = maxMagazineCapacity;
                    playerInfo.ResetInventory();
                    reloading = false;
                });
            }
        }
        Debug.Log("Reload Done");
        yield break;
    }

    [PunRPC]
    private void Shoot()
    {
        magazineCapacity--;
        GameObject bullet = PhotonNetwork.Instantiate("Prefabs/Bullet", firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<Bullet>().Init(playerPhotonView.ViewID);
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        playerUI.SetAmmo(magazineCapacity, ammoCapacity);
        
    }
}
