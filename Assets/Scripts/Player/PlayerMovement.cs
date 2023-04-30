using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviourPun
{
    //[SerializeField]private float _gravity = 0.1f;
    //[SerializeField] private float _rayDistance = 1.2f;
    [SerializeField] private float _moveSpeed = 5f;
    private Rigidbody rb;
    [SerializeField] private List<Color> _playerAssignmentColors;
    //[SerializeField] private TMP_Text _playerName;

    [SerializeField] private bool downed = false;
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private PlayerInfo playerVicinity;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Color assignment =
        _playerAssignmentColors[Mathf.Min(photonView.Controller.ActorNumber - 1, _playerAssignmentColors.Count - 1)];
        _spriteRenderer.color = assignment;
        //_playerName.text = photonView.Controller.NickName;
    }

    private void Update()
    {
        if (!photonView.IsMine ||  downed) return;
        
        //Gravity();

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("I'm pressing space");
            photonView.RPC(nameof(MessageTest), RpcTarget.All, photonView.Controller.NickName);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            if (playerVicinity != null) {
                if (playerVicinity.IsDown == true) {
                    playerVicinity.GetUp();
                }
            }
        }
        transform.position += new Vector3(horizontal * _moveSpeed * Time.deltaTime, 0, vertical * _moveSpeed * Time.deltaTime);
    }

    private void onTriggerEnter(Collider collider) {
        if (collider.GetComponent<PlayerInfo>()) {
            playerVicinity = collider.GetComponent<PlayerInfo>();
        }
    }

    private void onTriggerExit(Collider collider) {
        if (collider.GetComponent<PlayerInfo>()) {
            playerVicinity = null;
        }
    }

    [PunRPC]
    private void MessageTest(string playerName)
    {
        Debug.Log($"{playerName}:Are you ok?");
    }

    private void GetPlayerUp() {

    }

    private void UnGetPlayerUp() {

    }






}
