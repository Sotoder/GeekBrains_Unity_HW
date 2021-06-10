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
            b.Init(2f);
        }


    }

    public void Init()
    {
        Debug.Log("Pow");
    }

    public void ChangePosition(Vector3 position)
    {
        transform.Translate(position);
    }

    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }
}
