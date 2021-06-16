using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject _bulletPref;
    [SerializeField] private Transform[] _bulletStartPosition;
    [SerializeField] public float _fireRate = 1000f;

    public float FireRate { get => _fireRate; }

    public void Fire()
    {
        for (int i = 0; i < _bulletStartPosition.Length; i++)
        {
            Quaternion fractQuat = new Quaternion((this.transform.rotation.x + Random.Range(-0.15f, 0.15f)), 
                                                  (this.transform.rotation.y + Random.Range(-0.15f, 0.15f)), 
                                                  (this.transform.rotation.z + Random.Range(-0.15f, 0.15f)), 
                                                  (this.transform.rotation.w + Random.Range(-0.15f, 0.15f)));
            var fractions = Instantiate(_bulletPref, _bulletStartPosition[i].position, fractQuat);
        }
    }

    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }
}
