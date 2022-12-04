using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TimerModel
{
    [SerializeField] private Boss _boss;
    [SerializeField] private Text _timerText;
    [SerializeField] private WinGame _winGamePanel;

    public Boss Boss => _boss;
    public Text TimerText => _timerText;
    public WinGame WinGamePanel => _winGamePanel;
}
