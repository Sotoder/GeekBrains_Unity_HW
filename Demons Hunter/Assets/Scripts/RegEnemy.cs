using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegEnemy : MonoBehaviour
{
    [SerializeField] private int _maxHP = 100;

    private int _hp;

    private void Awake()
    {
        _hp = _maxHP;
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
}
