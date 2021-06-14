using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractions : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage = 5;


    public void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<RegEnemy>().TakingDamage(_damage);
            }
            if (!other.CompareTag("Bullets"))
            {
                Destroy(gameObject);
            }
        }
    }
}
