using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private PlayerActions _player;

    private void OnTriggerEnter(Collider other)
    {
        _player.IsGroundedUpate(other, true);
    }

    private void OnTriggerStay(Collider other)
    {
        _player.IsGroundedUpate(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        _player.IsGroundedUpate(other, false);
    }
}
