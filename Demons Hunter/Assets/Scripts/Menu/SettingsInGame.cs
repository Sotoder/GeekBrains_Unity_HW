using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsInGame : MonoBehaviour
{
    [SerializeField] private Toggle _tglSoundsMute;
    [SerializeField] private Slider _slrSoundsVolume;
    [SerializeField] private Text _txtVolumePercent;
    [SerializeField] private Slider _slrMusicVolume;
    [SerializeField] private Text _txtMusicVolumePercent;
    [SerializeField] private Button _btnSaveSettings;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Dropdown _ddQuality;
    [SerializeField] private AudioSource _musicSource;

    private int _qualityLevel;
    private bool _isMuted;
    private float _volume;
    private float _musicVolume;
    private bool _isGameLoaded;

    private void Awake()
    {
        LoadPlayerSettings();

        _btnSaveSettings.onClick.AddListener(SaveSettings);
        _tglSoundsMute.onValueChanged.AddListener(SoundsMute);
        _slrSoundsVolume.onValueChanged.AddListener(ChangeGlobalVolume);
        _slrMusicVolume.onValueChanged.AddListener(ChangeMusicVolumeText);
    }

    public void OnGameLoaded()
    {
        _isGameLoaded= true;
        AudioListener.volume = _volume * 0.01f;
    }

    public void LoadPlayerSettings()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = new List<string> { PlayFabConstants.GLOBAL_VOLUME, PlayFabConstants.MUSIC_VOLUME, PlayFabConstants.GRAPHIC_SETTINGS },
        }, OnDataGet, OnError);
    }

    private void OnDataGet(GetUserDataResult result)
    {
        var volume = float.Parse(result.Data[PlayFabConstants.GLOBAL_VOLUME].Value);
        var musicVolume = float.Parse(result.Data[PlayFabConstants.MUSIC_VOLUME].Value);
        var graphic = int.Parse(result.Data[PlayFabConstants.GRAPHIC_SETTINGS].Value);
        SetSettings(volume, musicVolume, graphic);
    }

    private void SetSettings(float globalVolume, float musicVolume, int graphic)
    {
        if (globalVolume > 0)
        {
            _tglSoundsMute.isOn = false;
            SoundsMute(false);

        }
        else
        {
            _tglSoundsMute.isOn = true;
            SoundsMute(true);
        }

        _musicSource.volume = musicVolume;
        _musicVolume = musicVolume * 100;
        _slrMusicVolume.value = _musicVolume;
        ChangeMusicVolumeText(_musicVolume);

        _volume = globalVolume * 100;
        _slrSoundsVolume.value = _volume;
        ChangeVolumeText(_volume);

        if (_isGameLoaded)
        {
            AudioListener.volume = globalVolume;
        }

        QualitySettings.SetQualityLevel(graphic, true);
        _ddQuality.value = graphic;
    }

    public void ChangeVolumeText(float value)
    {
        _txtVolumePercent.text = value.ToString();
    }


    private void SaveSettings()
    {
        QualitySettings.SetQualityLevel(_ddQuality.value, true);
        _qualityLevel = _ddQuality.value;
        _musicVolume = _slrMusicVolume.value;
        _volume = _slrSoundsVolume.value;

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>
            {
                [PlayFabConstants.GLOBAL_VOLUME] = (_volume * 0.01f).ToString(),
                [PlayFabConstants.MUSIC_VOLUME] = (_musicVolume * 0.01f).ToString(),
                [PlayFabConstants.GRAPHIC_SETTINGS] = _qualityLevel.ToString()
            },
            Permission = UserDataPermission.Private
        }, result => Debug.Log("Data updated"), OnError);

        AudioListener.volume = _volume * 0.01f;
        _musicSource.volume = _musicVolume * 0.01f;

        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(true);
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error.ToString());
    }

    private void SoundsMute(bool value)
    {
        _isMuted = value;
        if (_isMuted)
        {
            _slrSoundsVolume.value = 0;
            _slrSoundsVolume.interactable = false;
        }
        else
        {
            _slrSoundsVolume.interactable = true;
        }
    }

    private void ChangeGlobalVolume(float value)
    {
        _txtVolumePercent.text = value.ToString();
    }

    private void ChangeMusicVolumeText(float value)
    {
        _txtMusicVolumePercent.text = value.ToString();
    }
}
