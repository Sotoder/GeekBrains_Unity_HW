using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SettingsWindow: MonoBehaviour
{
    [SerializeField] private Toggle _tglSoundsMute;
    [SerializeField] private Slider _slrSoundsVolume;
    [SerializeField] private Text _txtVolumePercent;
    [SerializeField] private Slider _slrMusicVolume;
    [SerializeField] private Text _txtMusicVolumePercent;
    [SerializeField] private Button _btnSaveSettings;
    [SerializeField] private Dropdown _ddQuality;
    [SerializeField] private AudioSource _musicSource;

    private int _qualityLevel;
    private bool _isMuted;
    private float _volume;
    private float _musicVolume;

    private void Awake()
    {
        _btnSaveSettings.onClick.AddListener(SaveSettings);
        _tglSoundsMute.onValueChanged.AddListener(SoundsMute);
        _slrSoundsVolume.onValueChanged.AddListener(ChangeVolumeText);
        _slrMusicVolume.onValueChanged.AddListener(ChangeMusicVolumeText);
    }

    public void SetSettings(float globalVolume, float musicVolume, int graphic)
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
        _musicVolume = _musicSource.volume * 100;
        _slrMusicVolume.value = _musicVolume;
        ChangeMusicVolumeText(_musicVolume);

        AudioListener.volume = globalVolume;
        _volume = AudioListener.volume * 100;
        _slrSoundsVolume.value = _volume;
        ChangeVolumeText(_volume);

        QualitySettings.SetQualityLevel(graphic, true);
        _ddQuality.value = graphic;
    }

    private void SaveSettings()
    {
        QualitySettings.SetQualityLevel(_ddQuality.value, true);
        _qualityLevel = _ddQuality.value;
        _volume = _slrSoundsVolume.value;
        _musicVolume= _slrMusicVolume.value;

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
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error.ToString());
    }

    public void SoundsMute(bool value)
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

    public void ChangeVolumeText(float value)
    {
        _txtVolumePercent.text = value.ToString();
    }

    private void ChangeMusicVolumeText(float value)
    {
        _txtMusicVolumePercent.text = value.ToString();
    }
}