using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
        [Header("Panels")]
    [SerializeField] private LoginPanel _login;
    [SerializeField] private RegistrationPanel _registration;
    [Space(20)]
    [Header("Managers")]
    [SerializeField] private ExperienceManager _experienceManager;
    [SerializeField] private CurrencyManager _currencyManager;
    //[SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private StoreManager _storeManager;
    [Space(20)]
    [Header("Login Config")]
    [SerializeField] private GetPlayerCombinedInfoRequestParams _infoRequestParams;
    
    public static PlayfabManager Instance { get; private set; }
    
    public ExperienceManager ExperienceManager => _experienceManager;
    public CurrencyManager CurrencyManager => _currencyManager;
    //public InventoryManager InventoryManager => _inventoryManager;
    public StoreManager StoreManager => _storeManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
