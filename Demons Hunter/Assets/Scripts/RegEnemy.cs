using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RegEnemy : MonoBehaviour
{
    [SerializeField] private int _maxHP = 100;

    private int _hp;
    private Transform[] _patrolPoints;
    private NavMeshAgent _agent;
    int currentPatrolPoint;

    private void Awake()
    {
        _hp = _maxHP;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(!(_patrolPoints is null))
        {
            if (_agent.remainingDistance < _agent.stoppingDistance)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % _patrolPoints.Length;
                _agent.SetDestination(_patrolPoints[currentPatrolPoint].position);
            }
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

    private void Death()
    {
        Destroy(gameObject);
    }

    public void PatrolStart(Transform[] points)
    {
        _patrolPoints = points;
        _agent.SetDestination(_patrolPoints[0].position);
    }
}
