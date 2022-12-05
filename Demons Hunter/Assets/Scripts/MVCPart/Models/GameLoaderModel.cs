using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameLoaderModel
{
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private GameObject _loadScreen;
    [SerializeField] private Image _loadImage;
    [SerializeField] private SettingsInGame _settings;
    [SerializeField] private Button _continueButton;

    public AudioSource MusicAudioSource => _musicAudioSource;
    public GameObject LoadScreen => _loadScreen;
    public Image LoadImage => _loadImage;
    public SettingsInGame Settings => _settings;
    public Button ContinueButton => _continueButton;
}