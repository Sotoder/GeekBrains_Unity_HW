using System;
using UnityEngine;

[Serializable]
public class PlayerSpawnModel
{
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private PlayerConfiguration _playerConfig;

    public Transform SpawnTransform => _spawnTransform;
    public PlayerConfiguration PlayerConfig => _playerConfig;
}
