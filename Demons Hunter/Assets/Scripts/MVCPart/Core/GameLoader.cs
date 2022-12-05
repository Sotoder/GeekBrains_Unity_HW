using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

internal class GameLoader: IClearable
{
    public Action OnLoad;

    private GameLoaderModel _gameLoaderModel;

    public GameLoader(GameLoaderModel gameLoaderModel)
    {
        _gameLoaderModel = gameLoaderModel;

        _gameLoaderModel.LoadScreen.SetActive(true);

        _gameLoaderModel.Settings.LoadPlayerSettings();
        OnLoad += _gameLoaderModel.Settings.OnGameLoaded;

        _gameLoaderModel.ContinueButton.interactable = false;
        _gameLoaderModel.ContinueButton.onClick.AddListener(StartLevel);

        Cursor.lockState = CursorLockMode.None;
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
        _gameLoaderModel.ContinueButton.interactable = true;
    }

    public void Clear()
    {
        OnLoad -= _gameLoaderModel.Settings.OnGameLoaded;
        _gameLoaderModel.ContinueButton.onClick.RemoveListener(StartLevel);
    }

    private void StartLevel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _gameLoaderModel.MusicAudioSource.Play();
        _gameLoaderModel.LoadScreen.SetActive(false);
        OnLoad?.Invoke();
    }
}
