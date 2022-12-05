using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour, ITakingDamage
{

    //Params
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private int _maxSGAmmo = 50;
    [SerializeField] private int _maxMGAmmo = 500;
    [SerializeField] private GameObject _hpBar;
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private Image _hpBarImage;
    [SerializeField] private Text _hpText;
    [SerializeField] private Text _ammoText;
    [SerializeField] private Text _minesBombsText;
    [SerializeField] private AudioClip _walkAudio;
    [SerializeField] private AudioClip _runAudio;
    [SerializeField] private AudioClip _jumpAudio;
    [SerializeField] private Text _expText;
    [SerializeField] private Image _redKeyImage;
    [SerializeField] private Image _blueKeyImage;
    [SerializeField] private Image _yellowKeyImage;
    [SerializeField] private Image _greenKeyImage;

    private Dictionary<Color, int> _keyContainer = new Dictionary<Color, int>
    {
        [new Color(1f, 0f, 0f, 1f)] = 0,
        [new Color(1f, 0.922f, 0.016f, 1f)] = 0,
        [new Color(0f, 0f, 1f, 1f)] = 0,
        [new Color(0f, 1f, 0f, 1f)] = 0
    };

    private Dictionary<Color, Image> _keyImages = new Dictionary<Color, Image>();


    private int _hp;
    private int _sgAmmo = 50;
    private int _mgAmmo = 500;
    private int _curentWeaponAmmo;
    private int _curentWeaponMaxAmmo;
    private int _leverCount = 0;
    private int _secretBossDamageModifer = 1;
    private Animator animator;
    private AudioSource _playerAudioSource;
    private AudioSource _weaponAudioSource;
    private bool _isDead = false;
    private bool _isRun = false;
    private bool _isWalk = true;
    private bool _isLevelLoad;
    private bool _isJumped;

    public bool IsDead { get => _isDead; }
    public AudioSource WeaponAudioSource { get => _weaponAudioSource; }
    public AudioSource PlayerAudioSource { get => _playerAudioSource; }
    public Dictionary<Color, int> KeyContainer { get => _keyContainer; set => _keyContainer = value; }
    public int LeverCount { get => _leverCount; set => _leverCount = value; }
    public int SecretBossDamageModifer { get => _secretBossDamageModifer; set => _secretBossDamageModifer = value; }


    //Move Player
    [SerializeField] private float sensetivity;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedMult;
    [SerializeField] private float _jumpForce = 300f;
    [SerializeField] private float _gravity = 9.18f;
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
    private GameObject _weapon;
    private IWeapon w;
    private float _trowTime = 0f;
    private float _mineTime = 0f;

    private void Awake()
    {
        _hp = _maxHP;
        UpdateHPBar();
        Cursor.lockState = CursorLockMode.Locked;
        PlayerPrefs.SetInt("_isShowTooltip", 0);

        _keyImages = new Dictionary<Color, Image>
        {
            [new Color(1f, 0f, 0f, 1f)] = _redKeyImage,
            [new Color(1f, 0.922f, 0.016f, 1f)] = _yellowKeyImage,
            [new Color(0f, 0f, 1f, 1f)] = _greenKeyImage,
            [new Color(0f, 1f, 0f, 1f)] = _blueKeyImage
        };

        _rb = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _playerAudioSource = GetComponent<AudioSource>();


        _weaponPref = _mgPref;
        _weapon = Instantiate(_weaponPref, _weaponPositionAxie.position, transform.rotation);
        _weapon.transform.parent = _weaponPositionAxie;
        w = _weapon.GetComponent<MachineGun>();
        _weaponAudioSource = _weapon.GetComponent<AudioSource>();
        _weaponAudioSource.Stop();
        _curentWeaponAmmo = _mgAmmo;
        _curentWeaponMaxAmmo = _maxMGAmmo;
        animator.SetBool("MGun", true);
        animator.SetBool("SGun", false);

        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest()
        {
            Amount = 0,
            VirtualCurrency = "EX"
        }, result => _expText.text = "Exp: " + result.Balance, error => Debug.Log(error.ToString()));
    }

    public void StartGame()
    {
        _isLevelLoad= true;
    }

    void Update()
    {
        if (_isDead) return;
        if (!_isLevelLoad) return;

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
            _weapon = Instantiate(_weaponPref, _weaponPositionAxie.position, _head.transform.rotation);
            _weapon.transform.parent = _weaponPositionAxie;
            w = _weapon.GetComponent<MachineGun>();
            _weaponAudioSource = _weapon.GetComponent<AudioSource>();
            _weaponAudioSource.Stop();
            _curentWeaponAmmo = _mgAmmo;
            _curentWeaponMaxAmmo = _maxMGAmmo;
        }
        else if (Input.GetButton("Weapon2"))
        {
            animator.SetBool("MGun", false);
            animator.SetBool("SGun", true);
            w.DestroyWeapon();
            _weaponPref = _sgPref;
            _weapon = Instantiate(_weaponPref, _weaponPositionAxie.position, _head.transform.rotation);
            _weapon.transform.parent = _weaponPositionAxie;
            w = _weapon.GetComponent<ShotGun>();
            _weaponAudioSource = _weapon.GetComponent<AudioSource>();
            _weaponAudioSource.Stop();
            _curentWeaponAmmo = _sgAmmo;
            _curentWeaponMaxAmmo = _maxSGAmmo;
        }

        if (Input.GetAxis("Fire1") == 1f && w.IsReload && _curentWeaponAmmo > 0)
        {
            animator.SetBool("Fire", true);
            w.Fire(_secretBossDamageModifer);
            _curentWeaponAmmo--;
            if (animator.GetBool("MGun"))
            {
                WeaponSoundStart();
                _mgAmmo--;
            }
            else _sgAmmo--;

        }  else if (Input.GetButtonUp("Fire1") || _curentWeaponAmmo == 0)
        {
            if (animator.GetBool("MGun")) WeaponSoundStop();
            animator.SetBool("Fire", false);
        } 


        if (Input.GetAxis("Fire2") == 1f)
        {
            if (_trowTime == 0 && _bombCount > 0)
            {
                _bombCount--;
                TrowBomb();
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
                _mineCount--;
                TrowMine();

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
            _menuPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale= 0f;


            _playerAudioSource.Stop();
            _weaponAudioSource.Stop();
        }

        _ammoText.text = "Ammo: " + _curentWeaponAmmo.ToString() + "/" + _curentWeaponMaxAmmo.ToString();

        IfIsFalling();
    }

    private void IfIsFalling()
    {
        if (transform.position.y < -0.8)
        {
            TakingDamage(100, transform);
        }
    }

    private void FixedUpdate()
    {
        if (!_isLevelLoad) return;

        if(!_isJumped)
        {
            if (_direction != Vector3.zero)
            {
                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Run", false);
                _playerAudioSource.Stop();
            }
        }

        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        Vector3 speed;

        if (Input.GetButton("Sprint"))
        {
            if(!_isJumped)
            {
                if (!_isRun) _playerAudioSource.Stop();

                if (!_playerAudioSource.isPlaying)
                {
                    _playerAudioSource.clip = _runAudio;
                    _playerAudioSource.loop = true;
                    _playerAudioSource.Play();
                    _isRun = true;
                    _isWalk = false;
                }
            }

            speed = _direction * (_speed * _speedMult) * Time.fixedDeltaTime;
        }
        else
        {
            if (!_isWalk) _playerAudioSource.Stop();
            if (!_playerAudioSource.isPlaying)
            {
                _playerAudioSource.clip = _walkAudio;
                _playerAudioSource.loop = true;
                _playerAudioSource.Play();
                _isRun = false;
                _isWalk = true;
            }
            speed = _direction * _speed * Time.fixedDeltaTime;
        }

        JumpLogic();
        MovementLogic(speed);
    }

    
    private void PlayerLook()
    {
        mouseLookX = Input.GetAxis("Mouse X") * sensetivity * Time.deltaTime;
        mouseLookY = Input.GetAxis("Mouse Y") * sensetivity * Time.deltaTime;

        transform.Rotate(0, mouseLookX, 0);

        xRotation += mouseLookY;
        xRotation = Mathf.Clamp(xRotation, -40f, 25f);
        _head.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        _weaponPosition.localRotation = _head.transform.localRotation;
        _bombStartPosition.localRotation = _head.transform.localRotation;
    }

    private void MovementLogic(Vector3 speed)
    {
        var v = transform.TransformDirection(speed);
        v.y = _rb.velocity.y;

        _rb.velocity = v;
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (_isGrounded && !_isJumped)
            {
                _rb.AddForce(Vector3.up * _jumpForce * 50);
                _isJumped = true;
                _playerAudioSource.clip = _jumpAudio;
                _playerAudioSource.loop = false;
                _playerAudioSource.Play();
            }
        }         
        else if(Input.GetAxis("Jump") == 0 && _isJumped)
        {
            _isJumped = false;
        }

        if (!_isGrounded)
        {
            _rb.AddForce(Vector3.down * _gravity * 100);
        }
    }

    public void IsGroundedUpate(Collider collider, bool isOngraund)
    {
        if (collider.gameObject.tag == ("Ground") || collider.gameObject.tag == ("Vision"))
        {
            _isGrounded = isOngraund;
        }
    }

    private void TrowBomb()
    {
        var bomb = Instantiate(_bombPref, _bombStartPosition.position, transform.rotation);
        bomb.GetComponent<Rigidbody>().AddForce(_bombStartPosition.forward * 20, ForceMode.Impulse);
        var b = bomb.GetComponent<Bomb>();
        b.Init();
        _minesBombsText.text = "Mines: " + _mineCount.ToString() + " Bombs: " + _bombCount.ToString();
    }

    private void TrowMine()
    {
        var mine = Instantiate(_minePref, _mineStartPosition.position, transform.rotation);
        _minesBombsText.text = "Mines: " + _mineCount.ToString() + " Bombs: " + _bombCount.ToString();
    }


    public void TakingDamage(int damage, Transform sourceDamage)
    {
        if (_isDead) return;
        _head.GetComponent<AudioSource>().Play();
        _head.GetComponent<ParticleSystem>().Play();
        _hp -= damage;
        UpdateHPBar();
        if (_hp <= 0)
        {
            Death();
        }
    }

    private void UpdateHPBar()
    {
        var hpText = _hp < 0 ? 0 : _hp;

        _hpText.text = hpText.ToString() + "/" + _maxHP;
        float fill = (((float)hpText * 100) / (float)_maxHP) / 100;
        _hpBarImage.fillAmount = fill;
    }

    public void TakingBombDamage(int damage)
    {
        if (_isDead) return;
        _hp -= damage;
        UpdateHPBar();
        if (_hp <= 0)
        {
            Invoke("Death", 1f);
        }
    }

    private void Death()
    {
        animator.SetTrigger("Death");
        _isDead = true;
        AudioListener.volume = 0;
        Cursor.lockState = CursorLockMode.None;
        _endGamePanel.SetActive(true);
    }

    public void GetHeal(int healCount)
    {
        Debug.Log("Was " + _hp);
        _hp += healCount;
        if (_hp > _maxHP) _hp = _maxHP;
        UpdateHPBar();
        Debug.Log("Became " + _hp);
    }

    public void GetAmmo(int sgAmmoCount, int mgAmmoCount)
    {
        _sgAmmo += sgAmmoCount;       
        _mgAmmo += mgAmmoCount;

        if (_mgAmmo > _maxMGAmmo) _mgAmmo = _maxMGAmmo;
        if (_sgAmmo > _maxSGAmmo) _sgAmmo = _maxSGAmmo;

        if (animator.GetBool("MGun"))
        {
            _curentWeaponAmmo = _mgAmmo;
        }
        else
        {
            _curentWeaponAmmo = _sgAmmo;
        }
    }

    public void AddLeverCount()
    {
        LeverCount += 1;
        Debug.Log("Add Lever. Now " + LeverCount);
    }

    public void AddKey(Color color)
    {
        if(_keyImages.TryGetValue(color, out Image image))
        {
            image.color = color;
        }

        if (_keyContainer.ContainsKey(color))
        {
            _keyContainer[color] = 1;
        }

        foreach (var item in _keyContainer)
        {
            Debug.Log(item.Key + ": " + item.Value);
        }
    }

    private void WeaponSoundStart()
    {

        if (!_weaponAudioSource.isPlaying)
            _weaponAudioSource.Play();
    }

    private void WeaponSoundStop()
    {
        _weaponAudioSource.Stop();
    }
}
