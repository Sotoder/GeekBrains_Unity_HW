using UnityEngine;
using UnityEngine.UI;

public class Pedestal : MonoBehaviour
{
    [SerializeField] GameObject _sphere;
    [SerializeField] string _pcolor;
    [SerializeField] private GameObject _respectTextGameObject;

    public bool _onLight = false;

    private MeshRenderer _mr;
    private Color _color;
    private PlayerActions _player;

    private void Awake()
    {
        _mr = GetComponent<MeshRenderer>();
        _color = _mr.material.color;
    }
    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Player"))
        {
            if(_player == null)
            {
                _player = other.GetComponent<PlayerActions>();
            }

            if (_player.KeyContainer[_color] == 1)
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

    private void TryActivate ()
    {
        _sphere.GetComponent<Sphere>().ChangeColor(_pcolor);
        _onLight = true;

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
