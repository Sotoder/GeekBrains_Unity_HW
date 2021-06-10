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

    public void Init()
    {
        Debug.Log("MachineGun is create");
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
