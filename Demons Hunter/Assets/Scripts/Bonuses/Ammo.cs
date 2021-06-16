using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int _ammoCount = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerActions>().GetAmmo(_ammoCount);
            Debug.Log("GetAmmo!!");
            Destroy(gameObject);
        }
    }
}
