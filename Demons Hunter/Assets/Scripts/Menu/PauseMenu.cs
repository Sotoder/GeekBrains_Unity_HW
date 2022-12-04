using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button _btnResume;
    [SerializeField] Button _btnExit;
    [SerializeField] GameObject _hpBar;
    [SerializeField] SettingsInGame _settings;

    private void Awake()
    {        
        _btnResume.onClick.AddListener(ResumeGame);
        _btnExit.onClick.AddListener(ExitGame);
        _settings.LoadPlayerSettings();
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
}
