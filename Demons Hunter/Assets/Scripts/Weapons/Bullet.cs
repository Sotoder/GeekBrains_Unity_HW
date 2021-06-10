using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;

    public void Init(float _maxLifeTime)
    {
        Destroy(gameObject, _maxLifeTime);
    }

    public void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}
