using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private int _healCount = 10;
    [SerializeField] private AudioSource _healAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerView>().GetHeal(_healCount);
            _healAudioSource.Play();
            Destroy(gameObject);
        }
    }
}
