using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour,ITakingDamage, IEnemy
{

    [SerializeField] private int _maxHP = 300;
    [SerializeField] private bool _onAttack;
    [SerializeField] private int _damage = 60;
    [SerializeField] private float _attackSpeed = 0.5f;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private bool _isSecret;
    [SerializeField] private bool _isMain;

    private float _rangeAttack = 2f;
    private GameObject player;
    private int _hp;
    private NavMeshAgent _agent;
    private bool _isTired = false;
    private bool _isChangeKinematic = false;
    Rigidbody _rb;


    private void Awake()
    {
        _hp = _maxHP;
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _onAttack = false;
    }


    private void Update()
    {
        if (_onAttack)
        {
            _agent.SetDestination(player.transform.position);
            if (Vector3.Distance(player.transform.position, transform.position) <= _rangeAttack && !_isTired)
            {
                Debug.Log(_agent.remainingDistance);
                BitePlayer();
                _isTired = true;
                Invoke("Tired", _attackSpeed);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isChangeKinematic == true)
        {
            _isChangeKinematic = false;
            Invoke("ReturnKinematic", 2f);
        }
    }

    private void BitePlayer()
    {
        player.GetComponent<PlayerActions>().TakingDamage(_damage);
        Debug.Log("BITE!!!");
    }

    private void Tired()
    {
        _isTired = false;
    }

    public void Attack(GameObject _player)
    {
        _onAttack = true;
        player = _player;
        
    }

    public void EndAttack()
    {
        if (_onAttack == true)
        {
            _onAttack = false;
            _agent.SetDestination(_startPosition.position);
            if (_agent.remainingDistance <= _agent.stoppingDistance)
                transform.rotation = Quaternion.Euler(0f, _startPosition.rotation.eulerAngles.y, 0f); // пока так, после сделать плавный поворот в стартовую позицию
        }
    }

    public void TakingDamage(int damage)
    {
        _hp -= damage;
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
            _agent.isStopped = true;
            Invoke("Death", 1f);
        }
    }

    private void Death()
    {
        if (_isSecret)
        {
            player.GetComponent<PlayerActions>().SecretBossDamageModifer = 3;
            Destroy(gameObject);
        }

        if (_isMain)
        {
            Debug.Log("GameOver");
            Destroy(gameObject);
            Application.Quit();
        }
        
    }

    public void IsBombed()
    {
        _rb.isKinematic = false;
        _isChangeKinematic = true;
    }

    private void ReturnKinematic()
    {
        _rb.isKinematic = true;
        _isChangeKinematic = false;
    }
}

