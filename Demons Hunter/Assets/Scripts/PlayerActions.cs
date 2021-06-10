using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedMult;
    [SerializeField] private GameObject _mgPref;
    [SerializeField] private GameObject _sgPref;
    [SerializeField] private Transform _weaponPosition;
    [SerializeField] private GameObject _minePref;
    [SerializeField] private Transform _mineStartPosition;

    private Vector3 _direction;
    private GameObject _weaponPref;
    private GameObject weapon;
    private IWeapon w;

    private void Awake()
    {
        _weaponPref = _mgPref;
        weapon = Instantiate(_weaponPref, _weaponPosition.position, Quaternion.identity);
        w = weapon.GetComponent<MachineGun>();
    }

    void Update()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");


        if (Input.GetAxis("Weapon1") == 1f)
        {
            w.DestroyWeapon();
            _weaponPref = _mgPref;
            weapon = Instantiate(_weaponPref, _weaponPosition.position, Quaternion.identity);
            w = weapon.GetComponent<MachineGun>();
        }
        else if (Input.GetAxis("Weapon2") == 1f)
        {
            w.DestroyWeapon();
            _weaponPref = _sgPref;
            weapon = Instantiate(_weaponPref, _weaponPosition.position, Quaternion.identity);
            w = weapon.GetComponent<ShotGun>();
        }

        if (Input.GetAxis("Fire1") == 1f)
        {
            w.Fire();
        }

        if (Input.GetAxis("Fire2") == 1f)
        {
            TrowBomb();
        }
    }

    private void TrowBomb()
    {
        var mine = Instantiate(_minePref, _mineStartPosition.position, Quaternion.identity);
        var m = mine.GetComponent<Bomb>();
        m.Init();
    }

    private void FixedUpdate()
    {
        Vector3 speed;
        if (Input.GetAxis("Sprint") == 1f)
        {
            speed = _direction * (_speed * _speedMult) * Time.fixedDeltaTime;
        } else
        {
            speed = _direction * _speed * Time.fixedDeltaTime;
        }
        
        transform.Translate(speed);
        w.ChangePosition(speed);
    }
}
