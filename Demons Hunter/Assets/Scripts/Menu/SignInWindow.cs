using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;

    protected override void SubscriptionsElementsUI()
    {
        base.SubscriptionsElementsUI();

        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        HideErrorText();
        _isLogginInProgress = true;
        StartConnectionCorutine();

        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password,
        }, Success, Fail); 
    }

    private void Success(LoginResult result)
    {
        _isLogginInProgress = false;
        OnLogin?.Invoke();
        CloseWindow();
        HideStatusImage();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _signInButton.onClick.RemoveAllListeners();
    }
}
