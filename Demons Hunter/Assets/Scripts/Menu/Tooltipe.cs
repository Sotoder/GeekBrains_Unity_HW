using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltipe : MonoBehaviour
{
    [SerializeField] Button _btnOk;
    [SerializeField] PlayerView _player;

    private void Awake()
    {
        _btnOk.onClick.AddListener(Close);
        _player.PlayerAudioSource.Stop();
        _player.WeaponAudioSource.Stop();
    }

    private void Close()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;  
    }

    private void OnDestroy()
    {
        _btnOk.onClick.RemoveListener(Close);
    }
}
