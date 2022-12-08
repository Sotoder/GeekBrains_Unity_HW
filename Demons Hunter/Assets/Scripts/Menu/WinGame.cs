using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinGame : MonoBehaviour
{
    [SerializeField] PlayerView _player;
    [SerializeField] Button _btnExit;
    [SerializeField] Button _btnRestart;
    [SerializeField] Text _leadersNameText;
    [SerializeField] Text _leadersTimeText;
    [SerializeField] Text _selfPosition;

    public Text LeadersNameText => _leadersNameText;
    public Text SelfPosition => _selfPosition;
    public Text LeadersTimeText => _leadersTimeText;
    private void Awake()
    {
        _btnExit.onClick.AddListener(QuitGame);
        _btnRestart.onClick.AddListener(Restart);
    }

    public void Show()
    {
        gameObject.SetActive(true);

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
