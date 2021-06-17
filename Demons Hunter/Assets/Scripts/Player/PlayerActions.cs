using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedMult;
    [SerializeField] private GameObject _mgPref;
    [SerializeField] private GameObject _sgPref;
    [SerializeField] private Transform _weaponPosition;


    [SerializeField] private GameObject _bombPref;
    [SerializeField] private Transform _mineStartPosition;
    [SerializeField] private float sensetivity;
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private int _maxAmmo = 50;


    private Vector3 _direction;
    private GameObject _weaponPref;
    private GameObject weapon;
    private IWeapon w;
    private float mouseLookX = 0f;
    private float mouseLookY = 0f;
    private float xRotation = 0f;
    private float _shotTime = 0f;
    private float _trowTime = 0f;
    public int _leverCount = 0;

    private int _hp;
    private int _ammo;

    private void Awake()
    {
        _hp = _maxHP;
        _weaponPref = _mgPref;
        weapon = Instantiate(_weaponPref, _weaponPosition.position, transform.rotation);
        weapon.transform.parent = _weaponPosition;
        w = weapon.GetComponent<MachineGun>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //float f = 0f; // Генератор лагов =)
        //while (f < 100000f)
        //{
        //    f+=(0.5f * Time.deltaTime);
        //}
        //f = 0f;

        PlayerMove();
        PlayerLook();


        if (Input.GetButton("Weapon1"))
        {
            w.DestroyWeapon();
            _weaponPref = _mgPref;
            weapon = Instantiate(_weaponPref, _weaponPosition.position, transform.rotation);
            weapon.transform.parent = _weaponPosition;
            w = weapon.GetComponent<MachineGun>();
        }
        else if (Input.GetButton("Weapon2"))
        {
            w.DestroyWeapon();
            _weaponPref = _sgPref;
            weapon = Instantiate(_weaponPref, _weaponPosition.position, transform.rotation);
            weapon.transform.parent = _weaponPosition;
            w = weapon.GetComponent<ShotGun>();
        }

        if (Input.GetAxis("Fire1") == 1f)
        {
            if (_shotTime == 0) w.Fire();  

            _shotTime += Time.deltaTime; 
            if (_shotTime > w.FireRate)  
            {
                _shotTime = 0;
            }
        }
        else _shotTime = 0;     


        if (Input.GetAxis("Fire2") == 1f)
        {
            if (_trowTime == 0) TrowBomb();

            _trowTime += Time.deltaTime;
            if (_trowTime > 1000F)
            {
                _trowTime = 0;
            }
        }
        else _trowTime = 0;
    }

    private void PlayerMove()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");
    }

    private void PlayerLook()
    {

        mouseLookX = Input.GetAxis("Mouse X") * sensetivity * Time.deltaTime;
        mouseLookY = Input.GetAxis("Mouse Y") * sensetivity * Time.deltaTime;

        transform.Rotate(0, mouseLookX * sensetivity * Time.deltaTime, 0);

        xRotation -= mouseLookY;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);
        transform.Find("Head").localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Find("WeaponPosition").localRotation = transform.Find("Head").localRotation;

    }

    private void TrowBomb()
    {
        var mine = Instantiate(_bombPref, _mineStartPosition.position, transform.rotation);
        mine.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
        var m = mine.GetComponent<Bomb>(); 
        m.Init();
    }

    private void FixedUpdate()
    {
        Vector3 speed;
        if (Input.GetButton("Sprint"))
        {
            speed = _direction * (_speed * _speedMult) * Time.fixedDeltaTime;
        }
        else
        {
            speed = _direction * _speed * Time.fixedDeltaTime;
        }

        transform.Translate(speed);
    }



    public void TakingDamage(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void GetHeal(int healCount)
    {
        Debug.Log("Was " + _hp);
        _hp += healCount;
        if (_hp > _maxHP) _hp = _maxHP;
        Debug.Log("Became " + _hp);
    }

    public void GetAmmo(int ammoCount)
    {
        _ammo += ammoCount;
        if (_ammo > _maxAmmo) _ammo = _maxAmmo;
    }

    public void AddLeverCount()
    {
        _leverCount += 1;
        Debug.Log("Add Lever. Now " + _leverCount);
    }
}
