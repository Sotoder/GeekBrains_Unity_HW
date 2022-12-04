using PlayFab;
using System;
using UnityEngine;

[Serializable]
public class GameInitalizatorModel
{
    [SerializeField] private TimerModel _timerModel;

    public TimerModel TimerModel => _timerModel;
}
