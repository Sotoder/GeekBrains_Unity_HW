using GameObject = UnityEngine.Object;

public class SpawnController
{
    public PlayerView PlayerView { get; private set; }
    
    private SpawnControllerModel _spawnControllerModel;

    public SpawnController(SpawnControllerModel spawnControllerModel)
    {
        _spawnControllerModel = spawnControllerModel;
    }

    public void StartSpawn()
    {
        SpawnPlayerView();
    }

    private void SpawnPlayerView()
    {
        var playerViewPref = _spawnControllerModel.PlayerSpawnModel.PlayerConfig.PlayerPref;
        var playerSpawnTransform = _spawnControllerModel.PlayerSpawnModel.SpawnTransform;
        var playerObject = GameObject.Instantiate(playerViewPref, playerSpawnTransform);
        PlayerView = playerViewPref.GetComponent<PlayerView>();
    }
}
