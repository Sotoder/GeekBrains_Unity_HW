using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _maxLifeTime = 10f;

    public void Init()
    {
        Debug.Log("BombIsOut");
        Destroy(gameObject, _maxLifeTime);
    }
}
