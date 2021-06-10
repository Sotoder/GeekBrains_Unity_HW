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
            var bullet = Instantiate(_bulletPref, _bulletStartPosition[i].position, Quaternion.identity);
            var b = bullet.GetComponent<Bullet>();
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
