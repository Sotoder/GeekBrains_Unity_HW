using UnityEngine;

public class RotatePlayerController
{
    private RotatePlayerControllerModel _rotatePlayerControllerModel;
    private PlayerModel _playerModel;
    private IInputModel _inputModel;

    public RotatePlayerController(PlayerModel playerModel, IInputModel inputModel, PlayerConfiguration playerConfig)
    {
        _playerModel = playerModel;
        _inputModel = inputModel;
        _rotatePlayerControllerModel = new RotatePlayerControllerModel(playerConfig.MouseSensetivity);
    }

    public void PlayerLook(float deltaTime)
    {
        var mouseLookX = _inputModel.MouseXValue * _rotatePlayerControllerModel.Sensetivity * deltaTime;
        var mouseLookY = _inputModel.MouseYValue * _rotatePlayerControllerModel.Sensetivity * deltaTime;

        _playerModel.SetPlayerRotation(mouseLookX);

        var headRotation = _playerModel.PlayerHeadRotation + mouseLookY;
        headRotation = Mathf.Clamp(headRotation, -40f, 25f);
        _playerModel.SetPlayerHeadRotation(headRotation);

        _playerModel.PlayerViewTransform.Rotate(0, _playerModel.PlayerRotation, 0);
        _playerModel.PlayerHeadTransform.transform.localRotation = Quaternion.Euler(_playerModel.PlayerHeadRotation, 0, 0);
    }
}
