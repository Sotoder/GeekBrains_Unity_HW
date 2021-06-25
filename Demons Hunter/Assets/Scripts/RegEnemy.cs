using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RegEnemy : MonoBehaviour, ITakingDamage, IEnemy
{
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private bool _onPatrol;
    [SerializeField] private bool _onAttack;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackSpeed = 0.5f;

    private GameObject player;
    private int _hp;
    private Transform _spawnPosition;
    private float _spawnAngle;
    private Transform[] _patrolPoints;
    private NavMeshAgent _agent;
    private int currentPatrolPoint;
    Rigidbody _rb;
    private bool _isChangeKinematic = false;
    private bool _isTired = false;

    public Transform SpawnPosition { get => _spawnPosition; set => _spawnPosition = value; }

    public float SpawnAngle { set => _spawnAngle = value; }

    private void Awake()
    {
        _hp = _maxHP;
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _onPatrol = false;
        _onAttack = false;
        _patrolPoints = new Transform[0];
    }


    private void Update()
    {
        if(_patrolPoints.Length > 0 && _onPatrol && !_onAttack)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % _patrolPoints.Length;
                _agent.SetDestination(_patrolPoints[currentPatrolPoint].position);
            }
        }

        if (_onAttack)
        {
            _agent.SetDestination(player.transform.position);
            if (_agent.remainingDistance <= _agent.stoppingDistance && _isTired == false)
            {
                BitePlayer();
                _isTired = true;
                Invoke("Tired", _attackSpeed);
            }
        }

        if (!_onAttack && !_onPatrol)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
                transform.rotation = Quaternion.Euler(0f, _spawnAngle, 0f); // пока так, после сделать плавный поворот в стартовую позицию
        }
    }

    private void BitePlayer()
    {
        if (!(player is null))
        {
            player.GetComponent<PlayerActions>().TakingDamage(_damage);
            Debug.Log("BITE!!!");
        } else
        {
            EndAttack(_spawnPosition);
        }
    }

    private void Tired()
    {
        _isTired = false;
    }

    public void StartAttack(GameObject _player)
    {
        _onAttack = true;
        player = _player;
    }

    public void EndAttack(Transform spawnPosition)
    {
        if (_onAttack == true)
        {

            if (_patrolPoints.Length > 0)
            {
                _onAttack = false;
                ContinuePatrol();
            }
            else
            {
                _onAttack = false;
                _agent.SetDestination(_spawnPosition.position);
            }
        }
    }

    public void StopPatrol()
    {
        if (_patrolPoints.Length > 0)
        {
            _onPatrol = false;
        }
    }

    public void ContinuePatrol()
    {
        if (_patrolPoints.Length > 0)
        {
            _onPatrol = true;
            _agent.isStopped = false;
        } else
        {
            _agent.isStopped = false;
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

    public void TakingDamage(int damage)
    {
        _hp -= damage;
        if (_hp <=0)
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
        Destroy(gameObject);

    }

    public void PatrolStart(Transform[] points)
    {
        _onPatrol = true;
        _patrolPoints = points;
        _agent.SetDestination(_patrolPoints[0].position);
    }

    private void ReturnKinematic()
    {
        _rb.isKinematic = true;
        _isChangeKinematic = false;
    }

    public void IsBombed()
    {
        _rb.isKinematic = false;
        StopPatrol();
        _agent.isStopped = true;
        _isChangeKinematic = true;
        Invoke("ContinuePatrol", 2f);
    }
}
