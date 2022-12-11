using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private SignInWindow _signInWindow;
    [SerializeField] private CreateAccountWindow _createAccountWindow;
    [SerializeField] private SettingsWindow _settingsWindow;
    [SerializeField] private Button _btnStart;
    [SerializeField] private Button _btnLogout;
    [SerializeField] private Button _btnCreateAccaunt;
    [SerializeField] private Button _btnLogin;
    [SerializeField] private Button _btnExit;
    [SerializeField] private GameObject _playFabButtonGroup;
    [SerializeField] private GameObject _menuButtonGroup;
    [SerializeField] private AudioSource _menuMusicAudioSorce;
    [SerializeField] private UIAudioControllerModel _uiAudioControllerModel;

    private UIAudioController _uIAudioController;

    private void Awake()
    {
        _uIAudioController = new UIAudioController(_uiAudioControllerModel);
        _uIAudioController.SiginButtons();

        _btnCreateAccaunt.onClick.AddListener(OpenCreateWindow);
        _btnLogin.onClick.AddListener(OpenLoginWindow);
        _btnStart.onClick.AddListener(StartGame);
        _btnLogout.onClick.AddListener(Logout);
        _btnExit.onClick.AddListener(ExitGame);


        _signInWindow.OnLogin += ShowMenuButtons;
        _createAccountWindow.OnLogin += ShowMenuButtons;

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            LoadPlayerSettings();
        } else
        {
            _menuButtonGroup.SetActive(false);
            _playFabButtonGroup.SetActive(true);
        }
    }

    private void ShowMenuButtons()
    {
        _playFabButtonGroup.SetActive(false);

        LoadPlayerSettings();
    }

    private void LoadPlayerSettings()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = new List<string> { PlayFabConstants.GLOBAL_VOLUME, PlayFabConstants.MUSIC_VOLUME, PlayFabConstants.GRAPHIC_SETTINGS },
        }, OnDataGet, OnError);
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error.ToString());
    }

    private void OnDataGet(GetUserDataResult result)
    {
        if(result.Data.Count < 3)
        {
            PlayFabClientAPI.UpdateUserData (new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>
                {
                    [PlayFabConstants.GLOBAL_VOLUME] = "0,3",
                    [PlayFabConstants.MUSIC_VOLUME] = "1",
                    [PlayFabConstants.GRAPHIC_SETTINGS] = "1"
                },
                Permission = UserDataPermission.Private
            }, OnDataCreated, OnError);
        } else
        {
            var volume = float.Parse(result.Data[PlayFabConstants.GLOBAL_VOLUME].Value);
            var musicVolume = float.Parse(result.Data[PlayFabConstants.MUSIC_VOLUME].Value);
            var graphic = int.Parse(result.Data[PlayFabConstants.GRAPHIC_SETTINGS].Value);
            _settingsWindow.SetSettings(volume, musicVolume, graphic);
            _menuButtonGroup.SetActive(true);
            _menuMusicAudioSorce.Play();
        }
    }

    private void OnDataCreated(UpdateUserDataResult result)
    {
        _menuButtonGroup.SetActive(true);
        _settingsWindow.SetSettings(0.3f,1f, 1);
        _menuMusicAudioSorce.Play();
    }

    private void OpenCreateWindow()
    {
        _createAccountWindow.gameObject.SetActive(true);
    }

    private void OpenLoginWindow()
    {
        _signInWindow.gameObject.SetActive(true);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void Logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        _menuMusicAudioSorce.Stop();
        _menuButtonGroup.SetActive(false);
        _playFabButtonGroup.SetActive(true);
    }

    private void StartGame()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SceneManager.LoadScene("FirstLevel");
        } else
        {
            _menuButtonGroup.SetActive(false);
            _playFabButtonGroup.SetActive(true);
        }

    }

    private void OnDestroy()
    {
        _btnCreateAccaunt.onClick.RemoveListener(OpenCreateWindow);
        _btnLogin.onClick.RemoveListener(OpenLoginWindow);
        _btnStart.onClick.RemoveListener(StartGame);
        _btnLogout.onClick.RemoveListener(Logout);
        _btnExit.onClick.RemoveListener(ExitGame);

        _signInWindow.OnLogin -= ShowMenuButtons;
        _createAccountWindow.OnLogin -= ShowMenuButtons;

        _uIAudioController.Clear();
    }
}
