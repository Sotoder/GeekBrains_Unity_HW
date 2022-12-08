using UnityEngine;

public class LeverAxis : MonoBehaviour
{
    [SerializeField] private GameObject _destroibleObject;
    [SerializeField] private GameObject _respectTextGameObject;

    private Animator animator;
    private PlayerView _player;
    private bool _isNotRotate = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player == null)
            {
                _player = other.GetComponent<PlayerView>();
            }

            if(_isNotRotate)
            {
                _respectTextGameObject.SetActive(true);
                _player.OnGiveRespect += TryActivate;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _respectTextGameObject.SetActive(false);
            _player.OnGiveRespect -= TryActivate;
        }
    }

    private void TryActivate()
    {
        animator.SetBool("Down", true);
        _player.AddLeverCount();
        _isNotRotate = false;

        if (_player.LeverCount >= 4)
        {
            _destroibleObject.SetActive(false);
            Debug.Log("Secret Door is open");
        }

        _respectTextGameObject.SetActive(false);
        _player.OnGiveRespect -= TryActivate;
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnGiveRespect -= TryActivate;
        }
    }
}
