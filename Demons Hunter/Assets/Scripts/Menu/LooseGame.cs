using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LooseGame : MonoBehaviour
{
    [SerializeField] PlayerActions _player;
    [SerializeField] Button _btnExit;
    [SerializeField] Button _btnRestart;

    private void Awake()
    {
        _btnExit.onClick.AddListener(QuitGame);
        _btnRestart.onClick.AddListener(Restart);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;

        _player.PlayerAudioSource.Stop();
        _player.WeaponAudioSource.Stop();

    }

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }
    
    private void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        _btnExit.onClick.RemoveListener(QuitGame);
        _btnRestart.onClick.RemoveListener(Restart);
    }
}
