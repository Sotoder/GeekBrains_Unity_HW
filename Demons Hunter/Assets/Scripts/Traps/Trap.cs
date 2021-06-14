using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int _damage = 50;
    [SerializeField] private float _activationTime = 3f;
    private bool _isPressed = false;
    private float _stayTime = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {

            Debug.Log("Mine is near");
            _stayTime += Time.deltaTime;

            if (_stayTime >= _activationTime)
            {
                _stayTime = 0f;
                other.gameObject.GetComponent<PlayerActions>().TakingDamage(_damage);
                Destroy(gameObject);
            }

            if (_isPressed)
            {
                Destroy(gameObject);
                _isPressed = false;
                Debug.Log("BOOOM!!");
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        _stayTime = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerActions>().TakingDamage(_damage);
            _isPressed = true;
        }
    }
}
