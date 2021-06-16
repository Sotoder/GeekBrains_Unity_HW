using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private int _healCount = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerActions>().GetHeal(_healCount);
            Debug.Log("GetHeal!!");
            Destroy(gameObject);
        }
    }
}
