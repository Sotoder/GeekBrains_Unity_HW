using PlayFab;
using System;
using UnityEngine;

[Serializable]
public class GameInitalizatorModel
{
    [SerializeField] private TimerModel _timerModel;
    [SerializeField] private SpawnControllerModel _spawnControllerModel;

    public TimerModel TimerModel => _timerModel;
    public SpawnControllerModel SpawnControllerModel => _spawnControllerModel;
}
