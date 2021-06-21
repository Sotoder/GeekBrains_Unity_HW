using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField] private GameObject _body;
    Transform player;
    bool m_IsPlayerInRange = false;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            m_IsPlayerInRange = true;
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
        var enemy = _body.GetComponent<RegEnemy>();
        if (m_IsPlayerInRange)
        {
            
            enemy.StopPatrol();
            Vector3 direction = player.position;
            enemy.StartAttack(direction);

        } else
        {
            StartCoroutine(GoBackDelay(enemy));
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
