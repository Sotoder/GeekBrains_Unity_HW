using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private bool _isMuted;
    private float _volume;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetSettings(float soundVolume, bool mute)
    {
        _isMuted = mute;
        _volume = soundVolume;
    }
}
