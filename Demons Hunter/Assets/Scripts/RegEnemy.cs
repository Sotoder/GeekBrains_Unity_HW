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
    [SerializeField] private int _damage = 20;
    [SerializeField] private float _attackSpeed = 0.5f;

    private Vector3 playerPosition;
    private GameObject player;
    private int _hp;
    private Transform _spawnPosition;
    private float _spawnAngle;
    private Transform[] _patrolPoints;
    private NavMeshAgent _agent;
    private int currentPatrolPoint;
    private bool _isChangeKinematic = false;
    private bool _isTired = false;

    public Transform SpawnPosition { get => _spawnPosition; set => _spawnPosition = value; }
    public Transform[] PatrolPoints { get => _patrolPoints; }

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
            if (_agent.remainingDistance <= _agent.stoppingDistance && _isTired == false)
            {
                BitePlayer();
                _isTired = true;
                Invoke("Tired", _attackSpeed);
            }
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
        playerPosition = _player.transform.position;
        player = _player;
    }

    public void EndAttack(Transform spawnPosition)
    {
        if (_onAttack == true)
        {

            if (_patrolPoints.Length > 0)
            {
                _onAttack = false;
                StartPatrol();
            }
            else
            {
                _onAttack = false;
                _agent.SetDestination(_spawnPosition.position);
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                    transform.rotation = Quaternion.Euler(0f, _spawnAngle, 0f); // пока так, после сделать плавный поворот в стартовую позицию
            }
        }
    }

    public void StopPatrol()
    {
        _onPatrol = false;
    }

    public void StartPatrol()
    {
        _onPatrol = true;
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
