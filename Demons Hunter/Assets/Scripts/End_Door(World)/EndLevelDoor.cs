using UnityEngine;

public class EndLevelDoor : MonoBehaviour
{
    private bool _isAllLeversDown = false;
    private bool _isPlayerNear = false;

    public bool IsAllLeversDown { get => _isAllLeversDown; }
    public bool IsPlayerNear { get => _isPlayerNear; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = true;

            if (LeversManager.Instance.DownLeversCount == 2)
            {
                _isAllLeversDown = true;
            } 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isPlayerNear = false;
    }

}
