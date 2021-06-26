using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour, ITakingDamage
{

    //Params
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private int _maxAmmo = 50;
    [SerializeField] private GameObject _hpBar;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Image _hpBarImage;
    [SerializeField] private Text _hpText;

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
    private Animator animator;
    private bool _isDeath = false;


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
    [SerializeField] private Transform _weaponPositionAxie;
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
        UpdateHPBar();
        _weaponPref = _mgPref;
        weapon = Instantiate(_weaponPref, _weaponPositionAxie.position, transform.rotation);
        weapon.transform.parent = _weaponPositionAxie;
        w = weapon.GetComponent<MachineGun>();
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
        animator.SetBool("MGun", true);
        animator.SetBool("SGun", false);

        _rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //float f = 0f; // Генератор лагов =)
        //while (f < 100000f)
        //{
        //    f+=(0.5f * Time.deltaTime);
        //}
        //f = 0f;
        if (_isDeath) return;
        
        PlayerLook();

        if (Input.GetKeyDown(KeyCode.Y)) 
        {
            Death();
        }
        if (Input.GetButton("Weapon1"))
        {
            animator.SetBool("MGun", true);
            animator.SetBool("SGun", false);
            w.DestroyWeapon();
            _weaponPref = _mgPref;
            weapon = Instantiate(_weaponPref, _weaponPositionAxie.position, _head.rotation);
            weapon.transform.parent = _weaponPositionAxie;
            w = weapon.GetComponent<MachineGun>();
        }
        else if (Input.GetButton("Weapon2"))
        {
            animator.SetBool("MGun", false);
            animator.SetBool("SGun", true);
            w.DestroyWeapon();
            _weaponPref = _sgPref;
            weapon = Instantiate(_weaponPref, _weaponPositionAxie.position, _head.rotation);
            weapon.transform.parent = _weaponPositionAxie;
            w = weapon.GetComponent<ShotGun>();
        }

        if (Input.GetAxis("Fire1") == 1f && w.IsReload)
        {
            animator.SetBool("Fire", true);
            w.Fire(_secretBossDamageModifer);  
        }  else
        {
            animator.SetBool("Fire", false);
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

        if(Input.GetButton("Cancel"))
        {
            _hpBar.SetActive(false);
            _menuPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }


    private void FixedUpdate()
    {
        if (_direction != Vector3.zero)
        {
            animator.SetBool("Run", true);
        } else
        {
            animator.SetBool("Run", false);
        }
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

        //xRotation += mouseLookY; // более правильная реализация, но ловит баг, в случае если продолжать двигать мышку, то xRotation продолжает изменяться

        //if (xRotation <= 45f && xRotation >= -45)
        //{

        //    _head.Rotate(mouseLookY, 0, 0);
        //    _weaponPosition.Rotate(mouseLookY, 0, 0);
        //}

        xRotation += mouseLookY; // Старая, не совсем верная реализация поворота головы и оружия, но без бага поворота
        xRotation = Mathf.Clamp(xRotation, -40f, 25f);
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

    void OnCollisionEnter(Collision collision) // Устанавливаем флаг Ground при приземлении
    {
        IsGroundedUpate(collision, true);
    }

    void OnCollisionExit(Collision collision) // Снимаем флаг Ground при прыжке
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
        if (_isDeath) return;
        _hp -= damage;
        UpdateHPBar();
        Debug.Log("Auch!");
        Debug.Log(_hp);
        if (_hp <= 0)
        {
            Death();
        }
    }

    private void UpdateHPBar()
    {
        _hpText.text = _hp.ToString() + "/" + _maxHP;
        float fill = (((float)_hp * 100) / (float)_maxHP) / 100;
        _hpBarImage.fillAmount = fill;
    }

    public void TakingBombDamage(int damage)
    {
        if (_isDeath) return;
        _hp -= damage;
        if (_hp <= 0)
        {
            Invoke("Death", 1f);
        }
    }

    private void Death()
    {
        animator.SetTrigger("Death");
        _isDeath = true;
        Application.Quit();
    }

    public void GetHeal(int healCount)
    {
        Debug.Log("Was " + _hp);
        _hp += healCount;
        if (_hp > _maxHP) _hp = _maxHP;
        UpdateHPBar();
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
