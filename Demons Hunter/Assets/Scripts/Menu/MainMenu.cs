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


    private bool _isMuted;
    private float _volume;

    private void Awake()
    {
        SoundsMute(_tglSoundsMute.isOn);
        _btnStart.onClick.AddListener(StartGame);
        _btnExit.onClick.AddListener(ExitGame);
        _tglSoundsMute.onValueChanged.AddListener(SoundsMute);
        _slrSoundsVolume.onValueChanged.AddListener(Volume);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        //SceneManager.LoadScene("SampleScene");
        SceneManager.LoadScene("FirstLevel");
        _settings.GetComponent<Settings>().SetSettings(_volume, _isMuted);
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

    private void Volume(float value)
    {
        _volume = value;
        _txtVolumePercent.text = value.ToString();
    }
}
