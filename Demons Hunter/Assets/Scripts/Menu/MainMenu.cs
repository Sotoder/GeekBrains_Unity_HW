using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _btnStart;
    [SerializeField] Button _btnExit;
    [SerializeField] Toggle _tglSoundsMute;
    [SerializeField] Slider _slrSoundsVolume;
    [SerializeField] Text _txtVolumePercent;
    [SerializeField] GameObject _settings;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] Button _btnSaveSettings;
    [SerializeField] Dropdown _ddQuality;

    private int _qualityLevel;
    private bool _isMuted;
    private float _volume;

    private void Awake()
    {
        SoundsMute(_tglSoundsMute.isOn);
        _volume = 50;
        _slrSoundsVolume.value = _volume;
        ChangeVolumeText(_volume);
        QualitySettings.SetQualityLevel(1, true);
        _ddQuality.value = 1;


        _btnStart.onClick.AddListener(StartGame);
        _btnExit.onClick.AddListener(ExitGame);
        _btnSaveSettings.onClick.AddListener(SaveSettings);
        _tglSoundsMute.onValueChanged.AddListener(SoundsMute);
        _slrSoundsVolume.onValueChanged.AddListener(ChangeVolumeText);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        //SceneManager.LoadScene("SampleScene");
        SceneManager.LoadScene("FirstLevel");
    }

    private void SaveSettings()
    {
        QualitySettings.SetQualityLevel(_ddQuality.value, true);
        _qualityLevel = _ddQuality.value;
        _volume = _slrSoundsVolume.value;
        _settings.GetComponent<Settings>().SetSettings(_volume, _isMuted, _qualityLevel);
        _settingsPanel.SetActive(false);
    }

    private void SoundsMute(bool value)
    {
        _isMuted = value;
        if(_isMuted)
        {
            _slrSoundsVolume.value = 0;
            _slrSoundsVolume.interactable = false;
        } else
        {
            _slrSoundsVolume.interactable = true;
        }
    }

    private void ChangeVolumeText(float value)
    {
        _txtVolumePercent.text = value.ToString();
    }
}
