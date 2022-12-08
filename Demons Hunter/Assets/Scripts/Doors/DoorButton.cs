using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] Door _door;
    [SerializeField] GameObject _respectTextGameObject;

    private bool _isOpen;
    private PlayerView _player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (_player == null)
            {
                _player = other.GetComponent<PlayerView>();
            }

            if (!_isOpen)
            {
                _respectTextGameObject.SetActive(true);
                _player.OnGiveRespect += OpenDoor;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _respectTextGameObject.SetActive(false);
            _player.OnGiveRespect -= OpenDoor;
        }
    }

    private void OpenDoor()
    {
        _door.Open();
        _isOpen= true;
        _respectTextGameObject.SetActive(false);
        _player.OnGiveRespect -= OpenDoor;
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnGiveRespect -= OpenDoor;
        }
    }
}
