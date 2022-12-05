using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AccountDataWindowBase : MonoBehaviour
{
    public Action OnLogin;

    [SerializeField] private InputField _usernameField;
    [SerializeField] private InputField _passwordField;
    [SerializeField] private Button _backButton;
    [SerializeField] private Text _errorText;
    [SerializeField] protected Image _statusImage;
    [SerializeField] protected Sprite _loadSprite;

    protected string _username;
    protected string _password;
    protected bool _isLogginInProgress;

    private void Start()
    {
        SubscriptionsElementsUI();
    }

    protected virtual void SubscriptionsElementsUI()
    {
        _usernameField.onValueChanged.AddListener(UpdateUsername);
        _passwordField.onValueChanged.AddListener(UpdatePassword);
        _backButton.onClick.AddListener(CloseWindow);
    }

    protected virtual void CloseWindow()
    {
        this.gameObject.SetActive(false);
        _usernameField.text = "";
        _passwordField.text = "";
        HideErrorText();
        HideStatusImage();
    }

    private void UpdatePassword(string password)
    {
        _password = password;
    }

    private void UpdateUsername(string username)
    {
        _username = username;
    }

    protected void Fail(PlayFabError error)
    {
        _isLogginInProgress = false;
        HideStatusImage();
        _errorText.text = $"Fail: {error.ErrorMessage}";
        _errorText.color = Color.red;
    }

    protected void HideStatusImage()
    {
        _statusImage.transform.rotation = Quaternion.identity;
        _statusImage.sprite = null;
        _statusImage.color = new Color(0, 0, 0, 0);
    }

    protected void HideErrorText()
    {
        _errorText.text = "";
        _errorText.color = new Color(0, 0, 0, 0);
    }

    public void StartConnectionCorutine()
    {
        _statusImage.sprite = _loadSprite;
        _statusImage.color = Color.white;
        StartCoroutine(LoginProgressCoroutine());
    }

    protected IEnumerator LoginProgressCoroutine()
    {
        while (_isLogginInProgress)
        {
            _statusImage.transform.Rotate(Vector3.forward * Time.deltaTime * 100);
            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual void OnDestroy()
    {
        _usernameField.onValueChanged.RemoveAllListeners();
        _passwordField.onValueChanged.RemoveAllListeners();
        _backButton.onClick.RemoveListener(CloseWindow);
    }
}
