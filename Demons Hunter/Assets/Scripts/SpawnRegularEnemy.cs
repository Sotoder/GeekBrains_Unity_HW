using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRegularEnemy : MonoBehaviour

{
    [SerializeField] private GameObject _regEnemyRef;
    [SerializeField] private Transform _enemyPosition;
    [SerializeField] private float _enemyAnglePosition;
    [SerializeField] private bool _isPatrol;
    [SerializeField] private Transform[] _patrolWayPoints;
    private void Awake()
    {
        if (_isPatrol)
        {
            var patrolEnemy = Instantiate(_regEnemyRef, _enemyPosition.position, Quaternion.Euler(0f, _enemyAnglePosition, 0f)).GetComponent<RegEnemy>() ;
            patrolEnemy.PatrolStart(_patrolWayPoints);

        } else
        {
            Instantiate(_regEnemyRef, _enemyPosition.position, Quaternion.Euler(0f, _enemyAnglePosition, 0f));
        }
    }
}
