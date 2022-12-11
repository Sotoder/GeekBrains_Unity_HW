using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIAudioControllerModel
{
    [SerializeField] private List<ImprovedButton> _buttonList;
    [SerializeField] private AudioSource _menuAudioSource;
    [SerializeField] private AudioClip _onSelectClip;
    [SerializeField] private AudioClip _onClickClip;

    public List<ImprovedButton> ButtonList => _buttonList;
    public AudioSource MenuAudioSource => _menuAudioSource;
    public AudioClip OnSelectClip => _onSelectClip;
    public AudioClip OnClickClip => _onClickClip;
}
