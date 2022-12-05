using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button _btnResume;
    [SerializeField] Button _btnExit;
    [SerializeField] Button _btnRestart;
    [SerializeField] GameObject _hpBar;
    [SerializeField] SettingsInGame _settings;

    private void Awake()
    {        
        _btnResume.onClick.AddListener(ResumeGame);
        _btnRestart.onClick.AddListener(ReastartGame);
        _btnExit.onClick.AddListener(ExitGame);
        _settings.LoadPlayerSettings();
    }

    private void ReastartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    private void ResumeGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        _btnResume.onClick.RemoveListener(ResumeGame);
        _btnRestart.onClick.RemoveListener(ReastartGame);
        _btnExit.onClick.RemoveListener(ExitGame);
    }
}
