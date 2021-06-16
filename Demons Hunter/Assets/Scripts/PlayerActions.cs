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


    [SerializeField] private GameObject _minePref;
    [SerializeField] private Transform _mineStartPosition;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private float sensetivity;
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private int _maxAmmo = 50;


    private Vector3 _direction;
    private GameObject _weaponPref;
    private GameObject weapon;
    private IWeapon w;
    private float mouseLook;
    private float _shotTime = 0f;
    private float _trowTime = 0f;
    public int _leverCount = 0;

    private int _hp;
    private int _ammo;

    private void Awake() // Есть подозрение что для спавна противников и т.д нужен пустой объект Level, при Awake которого спавнятся враги, но пока так)
    {
        _hp = _maxHP;
        _weaponPref = _mgPref;
        weapon = Instantiate(_weaponPref, _weaponPosition.position, _playerPosition.rotation);
        weapon.transform.parent = _weaponPosition;
        w = weapon.GetComponent<MachineGun>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {


        PlayerMove();
        PlayerLook();


        if (Input.GetButton("Weapon1"))
        {
            w.DestroyWeapon();
            _weaponPref = _mgPref;
            weapon = Instantiate(_weaponPref, _weaponPosition.position, _playerPosition.rotation);
            weapon.transform.parent = _weaponPosition;
            w = weapon.GetComponent<MachineGun>();
        }
        else if (Input.GetButton("Weapon2"))
        {
            w.DestroyWeapon();
            _weaponPref = _sgPref;
            weapon = Instantiate(_weaponPref, _weaponPosition.position, _playerPosition.rotation);
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
        mouseLook = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseLook * sensetivity * Time.deltaTime, 0);
    }

    private void TrowBomb()
    {
        var mine = Instantiate(_minePref, _mineStartPosition.position, _playerPosition.rotation);
        var m = mine.GetComponent<Bomb>();
        m.Init();
    }

    private void FixedUpdate()
    {
        Vector3 speed;
        if (Input.GetButton("Sprint"))
        {
            speed = _direction * (_speed * _speedMult) * Time.fixedDeltaTime;
        } else
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
