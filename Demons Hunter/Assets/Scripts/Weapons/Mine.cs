using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private int _damage = 100;
    [SerializeField] private float _explosionTime = 2f;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _power = 1000f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Invoke("Explosion", _explosionTime);
        }
    }

    private void Explosion()
    {
        var colliders = Physics.OverlapSphere(transform.position, _radius);
        foreach (var hit in colliders)
        {
            if (hit.gameObject.CompareTag("Player") || hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Movable"))
            {
                Debug.Log(hit.gameObject.name);
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                IEnemy enemy = hit.GetComponent<IEnemy>();

                if (hit.gameObject.CompareTag("Movable"))
                {
                    AddExpForce(rb);
                } else
                {
                    if (rb != null)
                    {
                        if (rb.isKinematic == true && hit.gameObject.CompareTag("Enemy"))
                        {
                            rb.isKinematic = false;
                            AddExpForce(rb);
                            enemy.IsChangeKinematic = true;
                        }
                        else AddExpForce(rb);
                    }
                    hit.GetComponent<ITakingDamage>().TakingBombDamage(_damage);
                }
            }
        }
        Destroy(gameObject);
    }

    private void AddExpForce(Rigidbody rb)
    {
        rb.AddExplosionForce(_power, transform.position, _radius, 2.0F, ForceMode.Impulse);
        rb.AddRelativeTorque(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f), ForceMode.Impulse);
    }
}
