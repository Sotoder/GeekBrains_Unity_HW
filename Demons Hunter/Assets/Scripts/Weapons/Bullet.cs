using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage = 20;

    public void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<RegEnemy>().TakingDamage(_damage);

        } else if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerActions>().TakingDamage(_damage);
        }


        if (!other.CompareTag("Bullets") && !other.CompareTag("Traps") && !other.CompareTag("Weapon"))
        {
            Debug.Log(other.gameObject.name);
            Destroy(gameObject);
        }    
    }
}
