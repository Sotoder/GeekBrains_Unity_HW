using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button _btnResume;
    [SerializeField] Button _btnExit;
    [SerializeField] GameObject _hpBar;

    Settings _settings;
    private void Awake()
    {
        
        _btnResume.onClick.AddListener(ResumeGame);
        _btnExit.onClick.AddListener(ExitGame);

    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void ResumeGame()
    {
        gameObject.SetActive(false);
        _hpBar.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
