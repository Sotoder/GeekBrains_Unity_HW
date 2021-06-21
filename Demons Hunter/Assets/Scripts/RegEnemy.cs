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

    private Vector3 playerPosition;
    private int _hp;
    private Transform _spawnPosition;
    private float _spawnAngle;
    private Transform[] _patrolPoints;
    private NavMeshAgent _agent;
    [SerializeField] private int currentPatrolPoint;
    private bool _isChangeKinematic = false;

    public Transform SpawnPosition { get => _spawnPosition; set => _spawnPosition = value; }

    public bool IsChangeKinematic { set => _isChangeKinematic = value; }
    public float SpawnAngle { set => _spawnAngle = value; }

    private void Awake()
    {
        _hp = _maxHP;
        _agent = GetComponent<NavMeshAgent>();
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
            _agent.SetDestination(playerPosition);
        }
    }

    public void StartAttack(Vector3 direction)
    {
        _onAttack = true;
        playerPosition = direction;
    }

    public void EndAttack(Transform spawnPosition)
    {
        if (_patrolPoints.Length > 0)
        {
            _onAttack = false;
            _onPatrol = true;
        } else
        {
            _agent.SetDestination(_spawnPosition.position);
            if (_agent.remainingDistance <= _agent.stoppingDistance)
                transform.rotation = Quaternion.Euler (0f, _spawnAngle, 0f); // пока так, после сделать плавный поворот в стартовую позицию
        }
    }

    public void StopPatrol()
    {
        _onPatrol = false;
    }


    private void FixedUpdate()
    {
        if (_isChangeKinematic == true)
        {
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
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        _isChangeKinematic = false;
    }
}
