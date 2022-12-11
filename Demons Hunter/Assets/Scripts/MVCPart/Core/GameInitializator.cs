using System;
using System.Threading;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class GameInitializator
{

    public GameInitializator(GameInitalizatorModel gameInitalizatorModel, GameController gameController)
    {
        var playerConfig = gameInitalizatorModel.SpawnControllerModel.PlayerSpawnModel.PlayerConfig;

        var spawnController = new SpawnController(gameInitalizatorModel.SpawnControllerModel);
        spawnController.StartSpawn();

        var inputController = new InputController();

        var weaponController = new WeaponController(spawnController.PlayerView);

        var playerModel = new PlayerModel(playerConfig, spawnController.PlayerView);

        var playerController = new PlayerController(playerModel, inputController.InputModel, playerConfig, weaponController);

        var timerController = new GameTimerController(gameInitalizatorModel.TimerModel);
        var uiAudioController = new UIAudioController(gameInitalizatorModel.UIAudioControllerModel);

        uiAudioController.SiginButtons();
        gameController.Add(timerController);
        gameController.Add(inputController);
        gameController.Add(playerController);
    }
}

public class PlayerController: IUpdateble, IFixedUpdateble, IClearable
{
    private readonly IInputModel _inputController;
    private PlayerModel _playerModel;
    private MovePlayerController _movePlayerController;
    private RotatePlayerController _rotatePlayerController;
    private PlayerAnimationController _playerAnimationController;

    public PlayerController(PlayerModel playerModel, IInputModel inputModel, PlayerConfiguration playerConfig, WeaponController weaponController)
    {
        _playerModel = playerModel;
        _inputController = inputModel;

        _movePlayerController = new MovePlayerController(playerModel, inputModel, playerConfig);
        _rotatePlayerController = new RotatePlayerController(playerModel, inputModel, playerConfig);
    }
    public void Update(float deltaTime)
    {
        _rotatePlayerController.PlayerLook(deltaTime);
        _playerAnimationController.Update();
    }

    public void FixedUpdate(float fixedDeltaTime)
    {
        _movePlayerController.Move(fixedDeltaTime);
        _movePlayerController.Jump();
        _playerAnimationController.FixedUpdate();
    }

    public void Clear()
    {
        
    }
}
