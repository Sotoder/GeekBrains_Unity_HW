using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

internal class GameLoader
{
    public Action OnLoad;

    private GameLoaderModel _gameLoaderModel;

    public GameLoader(GameLoaderModel gameLoaderModel)
    {
        _gameLoaderModel = gameLoaderModel;
        _gameLoaderModel.LoadScreen.SetActive(true);
        _gameLoaderModel.Settings.LoadPlayerSettings();
    }

    public IEnumerator LoadCoroutine()
    {
        var load = 0f;
        while (load < 100)
        {
            load += Random.Range(10f, 40f);

            var fill = ((load * 100) / 100) / 100;
            _gameLoaderModel.LoadImage.fillAmount = fill;

            yield return new WaitForSeconds(1f);
        }

        _gameLoaderModel.MusicAudioSource.Play();
        _gameLoaderModel.LoadScreen.SetActive(false);
        OnLoad?.Invoke();
    }
}