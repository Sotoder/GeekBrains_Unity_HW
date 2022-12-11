using PlayFab;
using System;
using UnityEngine;

[Serializable]
public class GameInitalizatorModel
{
    [SerializeField] private TimerModel _timerModel;
    [SerializeField] private UIAudioControllerModel _uiAudioControllerModel;
    [SerializeField] private SpawnControllerModel _spawnControllerModel;

    public TimerModel TimerModel => _timerModel;
    public UIAudioControllerModel UIAudioControllerModel => _uiAudioControllerModel;
    public SpawnControllerModel SpawnControllerModel => _spawnControllerModel;
}
