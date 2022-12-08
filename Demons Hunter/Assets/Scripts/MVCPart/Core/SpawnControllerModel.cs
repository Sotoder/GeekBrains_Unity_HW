using System;
using UnityEngine;

[Serializable]
public class SpawnControllerModel
{
    [SerializeField] private PlayerSpawnModel _playerSpawnModel;

    public PlayerSpawnModel PlayerSpawnModel => _playerSpawnModel;
}
