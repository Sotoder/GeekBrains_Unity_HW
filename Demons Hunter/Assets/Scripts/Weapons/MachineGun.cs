using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject _bulletPref;
    [SerializeField] private Transform _bulletStartPosition;


    public void Fire()
    {
        var bullet = Instantiate(_bulletPref, _bulletStartPosition.position, Quaternion.identity);
        var b = bullet.GetComponent<Bullet>();
        b.Init(5f);

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
