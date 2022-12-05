using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int _sgAmmoCount = 10;
    [SerializeField] private int _mgAmmoCount = 100;
    [SerializeField] private AudioSource _ammoAudioSource; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerActions>().GetAmmo(_sgAmmoCount, _mgAmmoCount);
            _ammoAudioSource.Play();
            Destroy(gameObject);
        }
    }
}
