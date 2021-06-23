using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EliteEnemy : MonoBehaviour, ITakingDamage, IEnemy
{
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private bool _onAttack;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackSpeed = 0.5f;
    [SerializeField] public Transform _spawnPosition;
    [SerializeField] private Color _color;
    [SerializeField] private GameObject player;

    private float _rangeAttack = 1f;   
    private int _hp;
    private NavMeshAgent _agent;
    Rigidbody _rb;
    private bool _isChangeKinematic = false;
    private bool _isTired = false;


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
            if (Vector3.Distance(player.transform.position, transform.position) <= _rangeAttack && _isTired == false)
            {
                BitePlayer();
                _isTired = true;
                Invoke("Tired", _attackSpeed);
            }
        } else
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
                transform.rotation = Quaternion.Euler(0f, _spawnPosition.rotation.eulerAngles.y, 0f); // пока так, после сделать плавный поворот в стартовую позицию
        }
    }

    private void BitePlayer()
    {
        if (!(player is null))
        {
            player.GetComponent<PlayerActions>().TakingDamage(_damage);
            Debug.Log("BITE!!!");
        }
        else
        {
            EndAttack(_spawnPosition);
        }
    }

    private void Tired()
    {
        _isTired = false;
    }

    public void StartAttack()
    {
        _onAttack = true;
    }

    public void EndAttack(Transform spawnPosition)
    {
        if (_onAttack == true)
        {
            _onAttack = false;
            _agent.SetDestination(_spawnPosition.position);
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
        player.GetComponent<PlayerActions>().AddKey(_color);
        Destroy(gameObject);
    }


    private void ReturnKinematic()
    {
        _rb.isKinematic = true;
        _isChangeKinematic = false;
        _agent.isStopped = false;
    }

    public void IsBombed()
    {
        _rb.isKinematic = false;
        _agent.isStopped = true;
        _isChangeKinematic = true;
        Invoke("ReturnKinematic", 1f);
    }
}
