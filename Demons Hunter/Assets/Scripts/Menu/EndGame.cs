using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField] Text _txtInEnd;
    [SerializeField] GameObject _player;
    [SerializeField] Button _btnExit;
    [SerializeField] Button _btnRestart;

    private void Awake()
    {
        _btnExit.onClick.AddListener(QuitGame);
        _btnRestart.onClick.AddListener(Restart);
        if (_player.GetComponent<PlayerActions>().IsDead)
        {
            _txtInEnd.text = "GAME OVER";
        } else
        {
            _txtInEnd.text = "YOU WIN!!!";
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }
    
    private void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
