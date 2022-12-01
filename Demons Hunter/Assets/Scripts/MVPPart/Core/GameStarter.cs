using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private GameInitalizatorModel _gameInitalizatorModel;
    [SerializeField] private GameLoaderModel _gameLoaderModel;

    private GameInitializator _gameInitializator;
    private GameLoader _gameLoader;

    private void Awake()
    {
        _gameLoader = new GameLoader(_gameLoaderModel);
        _gameLoader.IsLoad += StartGame;

        StartCoroutine(_gameLoader.LoadCoroutine());
    }

    private void StartGame()
    {
        _gameInitializator= new GameInitializator();
    }
}
