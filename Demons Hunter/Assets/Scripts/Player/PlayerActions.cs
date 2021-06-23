using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour, ITakingDamage
{

    //Params
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private int _maxAmmo = 50;

    private Dictionary<Color, int> _keyContainer = new Dictionary<Color, int>
    {
        [new Color(1f, 0f, 0f, 1f)] = 0,
        [new Color(1f, 0.922f, 0.016f, 1f)] = 0,
        [new Color(0f, 0f, 1f, 1f)] = 0,
        [new Color(0f, 1f, 0f, 1f)] = 0
    };
    private int _hp;
    private int _ammo;
    private int _leverCount = 0;
    private int _secretBossDamageModifer = 1;


    public Dictionary<Color, int> KeyContainer { get => _keyContainer; set => _keyContainer = value; }
    public int LeverCount { get => _leverCount; set => _leverCount = value; }
    public int SecretBossDamageModifer { get => _secretBossDamageModifer; set => _secretBossDamageModifer = value; }


    //Move Player
    [SerializeField] private float sensetivity;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedMult;
    [SerializeField] private float _jumpForce = 300f;
    [SerializeField] private float _gravity = 9.18f;
    [SerializeField] private Transform _head;
    private bool _isGrounded;
    private Rigidbody _rb;
    private Vector3 _direction;
    private float mouseLookX = 0f;
    private float mouseLookY = 0f;
    private float xRotation = 0f;

    //Weapons and Shoting
    [SerializeField] private GameObject _mgPref;
    [SerializeField] private GameObject _sgPref;
    [SerializeField] private Transform _weaponPosition;
    [SerializeField] private GameObject _bombPref;
    [SerializeField] private Transform _bombStartPosition;
    [SerializeField] private GameObject _minePref;
    [SerializeField] private Transform _mineStartPosition;
    [SerializeField] private int _mineCount = 5;
    [SerializeField] private int _bombCount = 5;
    private GameObject _weaponPref;
    private GameObject weapon;
    private IWeapon w;
    private float _trowTime = 0f;
    private float _mineTime = 0f;


    private void Awake()
    {
        _hp = _maxHP;
        _weaponPref = _mgPref;
        weapon = Instantiate(_weaponPref, _weaponPosition.position, transform.rotation);
        weapon.transform.parent = _weaponPosition;
        w = weapon.GetComponent<MachineGun>();
        Cursor.lockState = CursorLockMode.Locked;

        _rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //float f = 0f; // ��������� ����� =)
        //while (f < 100000f)
        //{
        //    f+=(0.5f * Time.deltaTime);
        //}
        //f = 0f;

        PlayerLook(); 


        if (Input.GetButton("Weapon1"))
        {
            w.DestroyWeapon();
            _weaponPref = _mgPref;
            weapon = Instantiate(_weaponPref, _weaponPosition.position, _head.rotation);
            weapon.transform.parent = _weaponPosition;
            w = weapon.GetComponent<MachineGun>();
        }
        else if (Input.GetButton("Weapon2"))
        {
            w.DestroyWeapon();
            _weaponPref = _sgPref;
            weapon = Instantiate(_weaponPref, _weaponPosition.position, _head.rotation);
            weapon.transform.parent = _weaponPosition;
            w = weapon.GetComponent<ShotGun>();
        }

        if (Input.GetAxis("Fire1") == 1f && w.IsReload)
        {
            w.Fire(_secretBossDamageModifer);  
        }  


        if (Input.GetAxis("Fire2") == 1f)
        {
            if (_trowTime == 0 && _bombCount > 0)
            {
                TrowBomb();
                _bombCount--;
            }

            _trowTime += Time.deltaTime;


            if (_trowTime > 1000F)
            {
                _trowTime = 0;
            }
        }
        else _trowTime = 0;


        if (Input.GetAxis("Fire3") == 1f)
        {
            if (_mineTime == 0 && _mineCount > 0)
            {
                TrowMine();
                _mineCount--;
            }

            _mineTime += Time.deltaTime;

            if (_mineTime > 1000F)
            {
                _mineTime = 0;
            }
        }
        else _mineTime = 0;
    }


    private void FixedUpdate()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        Vector3 speed;
        if (Input.GetButton("Sprint"))
        {
            speed = _direction * (_speed * _speedMult);
        }
        else
        {
            speed = _direction * _speed;
        }

        JumpLogic();
        MovementLogic(speed);

    }

    private void PlayerLook()
    {
        mouseLookX = Input.GetAxis("Mouse X") * sensetivity * Time.deltaTime;
        mouseLookY = Input.GetAxis("Mouse Y") * sensetivity * Time.deltaTime;

        transform.Rotate(0, mouseLookX, 0);

        //xRotation += mouseLookY; // ����� ���������� ����������, �� ����� ���, � ������ ���� ���������� ������� �����, �� xRotation ���������� ����������

        //if (xRotation <= 45f && xRotation >= -45)
        //{

        //    _head.Rotate(mouseLookY, 0, 0);
        //    _weaponPosition.Rotate(mouseLookY, 0, 0);
        //}

        xRotation += mouseLookY; // ������, �� ������ ������ ���������� �������� ������ � ������, �� ��� ���� ��������
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);
        _head.localRotation = Quaternion.Euler(xRotation, 0, 0);

        _weaponPosition.localRotation = _head.localRotation;
        _bombStartPosition.localRotation = _head.localRotation;
    }

    private void MovementLogic(Vector3 speed)
    {
        _rb.AddForce(transform.forward * speed.z, ForceMode.Impulse);
        _rb.AddForce(transform.right * speed.x, ForceMode.Impulse);
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * _jumpForce * 100);
            }
        }

        if (!_isGrounded)
        {
            _rb.AddForce(Vector3.down * _gravity * 200);
        }
    }

    void OnCollisionEnter(Collision collision) // ������������� ���� Ground ��� �����������
    {
        IsGroundedUpate(collision, true);
    }

    void OnCollisionExit(Collision collision) // ������� ���� Ground ��� ������
    {
        IsGroundedUpate(collision, false);
    }

    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            _isGrounded = value;
        }
    }

    private void TrowBomb()
    {
        var bomb = Instantiate(_bombPref, _bombStartPosition.position, transform.rotation);
        bomb.GetComponent<Rigidbody>().AddForce(_bombStartPosition.forward * 20, ForceMode.Impulse);
        var b = bomb.GetComponent<Bomb>();
        b.Init();
    }

    private void TrowMine()
    {
        var mine = Instantiate(_minePref, _mineStartPosition.position, transform.rotation);
    }


    public void TakingDamage(int damage)
    {
        _hp -= damage;
        Debug.Log("Auch!");
        Debug.Log(_hp);
        if (_hp <= 0)
        {
            Death();
        }
    }

    public void TakingBombDamage(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            Invoke("Death", 1f);
        }
    }

    private void Death()
    {
        Application.Quit();
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
        LeverCount += 1;
        Debug.Log("Add Lever. Now " + LeverCount);
    }

    public void AddKey(Color color)
    {

        if (_keyContainer.TryGetValue(color, out int value))
        {
            _keyContainer[color] = 1;
        }

        foreach (var item in _keyContainer)
        {
            Debug.Log(item.Key + ": " + item.Value);
        }
    }
}
