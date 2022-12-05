using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] private InputField _emailField;
    [SerializeField] private Button _createAccountButton;

    private string _email;

    protected override void SubscriptionsElementsUI()
    {
        base.SubscriptionsElementsUI();

        _emailField.onValueChanged.AddListener(UpdateEmail);
        _createAccountButton.onClick.AddListener(CreateAccount);
    }

    private void UpdateEmail(string email)
    {
        _email = email;
    }

    private void CreateAccount()
    {
        HideErrorText();
        _isLogginInProgress = true;
        StartConnectionCorutine();

        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username,
            DisplayName = _username,
            Email = _email,
            Password = _password
        }, Success, Fail);
    }

    protected void Success(RegisterPlayFabUserResult result)
    {
        _isLogginInProgress = false;
        OnLogin?.Invoke();
        CloseWindow();
        HideStatusImage();
    }

    protected override void CloseWindow()
    {
        base.CloseWindow();
        _emailField.text = "";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _emailField.onValueChanged.RemoveAllListeners();
        _createAccountButton.onClick.RemoveAllListeners();
    }
}
