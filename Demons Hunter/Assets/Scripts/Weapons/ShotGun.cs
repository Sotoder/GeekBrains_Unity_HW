using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject _bulletPref;
    [SerializeField] private Transform[] _bulletStartPosition;

    public void Fire()
    {
        for (int i = 0; i < _bulletStartPosition.Length; i++)
        {
            var fractions = Instantiate(_bulletPref, _bulletStartPosition[i].position, Quaternion.Euler(Random.Range(-20f,20f), Random.Range(-20f, 20f), 0));
            var b = fractions.GetComponent<Fractions>();
            b.Init(1f);
        }


    }

    public void ChangePosition(Vector3 newPosition)
    {
        transform.Translate(newPosition);
    }

    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }
}
