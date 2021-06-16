using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject _bulletPref;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] public float _fireRate = 0.2f;

    public float FireRate { get => _fireRate; }

    public void Fire()
    {
        var bullet = Instantiate(_bulletPref, _bulletStartPosition.position, transform.rotation);
    }

    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }


}
