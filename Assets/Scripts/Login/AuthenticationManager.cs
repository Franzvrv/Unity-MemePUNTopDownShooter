using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Photon.Pun;
using UnityEngine.SceneManagement;
using static ExperienceManager;

public class AuthenticationManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private LoginPanel _login;
    [SerializeField] private GameObject _popup;
    [SerializeField] private RegistrationPanel _registration;
    [Space(20)]
    [Header("Managers")]
    [SerializeField] private ExperienceManager _experienceManager;
    [SerializeField] private CurrencyManager _currencyManager;
    [Space(20)]
    [Header("Login Config")]
    [SerializeField] private GetPlayerCombinedInfoRequestParams _infoRequestParams;
    
    public string PlayFabId { get; private set; }

    private void OnEnable()
    {
        _login.LoginEvent += OnLoginCalled;
        _registration.RegisterEvent += OnRegisterCalled;
    }

    private void OnDisable()
    {
        _login.LoginEvent -= OnLoginCalled;
        _registration.RegisterEvent -= OnRegisterCalled;
    }

    private void OnLoginCalled(string userName , string password )
    {
        PlayFabClientAPI.LoginWithEmailAddress(
                new LoginWithEmailAddressRequest()
                {
                    Email = userName,
                    Password = password,
                    TitleId = PlayFabSettings.TitleId,
                    InfoRequestParameters = _infoRequestParams
                },(loginResult) => {
                    Debug.Log("Successfully Logged in");
                    if (loginResult.InfoResultPayload != null)
                    {
                        InitializeConfigValues(loginResult.InfoResultPayload);
                    }
                    else
                    {
                        Debug.LogError("Info Result Payload is null");
                    }
                    PhotonNetwork.NickName = userName;
                    SceneManager.LoadScene("Lobby");
                },(error) => {
                    ErrorPopup($"Failed to login: {error.ErrorMessage}");
                }
            );
    }

    private void OnRegisterCalled(string userName, string password)
    {
        PlayFabClientAPI.LoginWithCustomID(
            new LoginWithCustomIDRequest
            {
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true,
                TitleId = PlayFabSettings.TitleId
            }, (loginSuccess) =>
            {
                PlayFabClientAPI.AddUsernamePassword(new AddUsernamePasswordRequest
                {
                    Email = userName,
                    Password = password,
                    Username = loginSuccess.PlayFabId
                }, (updateSuccess) =>
                    {
                        Debug.Log("Sucessfully Registered");
                        PhotonNetwork.NickName = userName;
                        SceneManager.LoadScene("Lobby");
                    }, (updateFail) =>
                {
                    var msg = "";
                        foreach (var VARIABLE in updateFail.ErrorDetails)
                        {
                            msg += VARIABLE.Key + "\n";
                            foreach (var item in VARIABLE.Value)
                            {
                                msg += $"#{item}";
                            }
                        }
                        ErrorPopup($"Failed to Register: {updateFail.Error}\n {updateFail.ErrorMessage} \n {msg}");
                    });

            }, (loginFailure) =>
            {
                ErrorPopup($"Unable to Login with custom Id: {loginFailure.ErrorMessage}");
            });
    }

    private void InitializeConfigValues(GetPlayerCombinedInfoResultPayload payload)
    {
        if (payload.TitleData == null) return;
        var titleData = payload.TitleData;


        if(titleData.TryGetValue("DropAndExpRate", out var totalRates))
        {
            var configRate = JsonUtility.FromJson<DropAndExpRate>(totalRates);
            _experienceManager.Init(configRate);
        }

        if(payload.UserVirtualCurrency != null)
        {
            foreach (var item in payload.UserVirtualCurrency)
            {
                _currencyManager.Init(item.Key, item.Value);
                Debug.Log($"{item.Key}: {item.Value}");
            }
        } else {
            Debug.Log("No virtual currency");
        }

        if(payload.AccountInfo != null)
        {
            PlayFabId = payload.AccountInfo.PlayFabId;
        }
    }

    public void ErrorPopup(string text) {
        var popup = Instantiate(_popup, transform.position, Quaternion.identity).GetComponent<Popup>();
        popup.Initialize(text);
        popup.transform.SetParent(this.gameObject.transform, false);
    }
    
}
