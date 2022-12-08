using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private PlayerView _player;
    [SerializeField] private GameInitalizatorModel _gameInitalizatorModel;
    [SerializeField] private GameLoaderModel _gameLoaderModel;

    private GameLoader _gameLoader;
    private GameController _gameController;

    private void Start()
    {
        Time.timeScale = 1f;
        _gameController = new GameController();

        AudioListener.volume = 0f;
        _gameLoader = new GameLoader(_gameLoaderModel);
        _gameController.Add(_gameLoader);

        _gameLoader.OnLoad += StartGame;
        StartCoroutine(_gameLoader.LoadCoroutine());
    }

    private void StartGame()
    {
        new GameInitializator(_gameInitalizatorModel, _gameController);
        _player.StartGame();
    }

    private void Update()
    {
        var t = Time.timeScale;
        var deltaTime = Time.deltaTime;
        _gameController.Update(deltaTime);
    }

    private void FixedUpdate()
    {
        var fixedDeltaTime = Time.fixedDeltaTime;
        _gameController.FixedUpdate(fixedDeltaTime);
    }

    private void OnDestroy()
    {
        _gameController.Clear();
    }

}
