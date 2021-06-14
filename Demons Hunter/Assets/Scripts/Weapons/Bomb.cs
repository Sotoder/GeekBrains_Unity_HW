using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _maxLifeTime = 10f;
    [SerializeField] private int _damage = 100;

    public void Init()
    {
        Debug.Log("BombIsOut");
        Destroy(gameObject, _maxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<RegEnemy>().TakingDamage(_damage);
        }
        Destroy(gameObject);
    }
}
