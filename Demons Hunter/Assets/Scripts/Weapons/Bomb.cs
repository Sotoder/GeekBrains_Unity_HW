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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<RegEnemy>().TakingDamage(_damage);
            Destroy(gameObject);
        }
    }
}
