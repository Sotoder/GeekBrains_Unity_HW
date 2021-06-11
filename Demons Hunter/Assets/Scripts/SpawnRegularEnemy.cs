using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRegularEnemy : MonoBehaviour

{
    [SerializeField] private GameObject _regEnemyRef;
    [SerializeField] private Transform _enemyPosition;
    [SerializeField] private float _enemyAnglePosition;
    private void Awake()
    {
        Instantiate(_regEnemyRef, _enemyPosition.position, Quaternion.Euler(0f, _enemyAnglePosition, 0f));
    }
}
