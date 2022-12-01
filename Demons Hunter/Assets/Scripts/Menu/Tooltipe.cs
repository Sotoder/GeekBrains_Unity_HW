using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltipe : MonoBehaviour
{
    [SerializeField] Button _btnOk;

    private void Awake()
    {
        _btnOk.onClick.AddListener(Close);
    }

    private void Close()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;  
    }
}
