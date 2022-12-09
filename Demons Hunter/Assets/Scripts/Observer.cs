using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Observer : MonoBehaviour
{
    [SerializeField] private RegEnemy _enemy;
    [SerializeField] private LayerMask _layerMask;
    bool m_IsPlayerInRange = false;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    private GameObject _player;
    private Coroutine _coroutine;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SearchPlayer(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SearchPlayer(other);
        }
    }

    private void SearchPlayer(Collider other)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, other.gameObject.transform.position - transform.position);
        Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask);

        if (hit.collider.CompareTag("Player"))
        {
            m_IsPlayerInRange = true;
            _player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update()
    {
        if (m_IsPlayerInRange && !_player.GetComponent<PlayerActions>().IsDead)
        {
            
            _enemy.StopPatrol();
            _enemy.StartAttack(_player);

            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        } 
        else
        {
            _coroutine = StartCoroutine(GoBackDelay(_enemy));
        }
    }

    private IEnumerator GoBackDelay(RegEnemy enemy)
    {
        int i = 0;
        while (i <= 20)
        {
            if (i == 20) enemy.EndAttack(enemy.SpawnPosition);
            yield return _waitForFixedUpdate;
            i++;
        }
    }
}
