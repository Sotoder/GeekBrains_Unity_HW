using UnityEngine;

public class MovePlayerController
{
    private MovePlayerControllerModel _movePlayerControllerModel;
    private PlayerModel _playerModel;
    private IInputModel _inputModel;
    private float _jumpForce;

    private const float GRAVITY = 918f;

    public MovePlayerController(PlayerModel playerModel, IInputModel inputModel, PlayerConfiguration playerConfig)
    {
        _playerModel = playerModel;
        _inputModel = inputModel;
        _movePlayerControllerModel = new MovePlayerControllerModel(playerConfig.MoveSpeed, playerConfig.RunSpeedMult);
    }

    public void Move(float fixedDeltaTime)
    {
        var direction = new Vector3();

        direction.x = _inputModel.HorizontalValue;
        direction.z = _inputModel.VerticalValue;

        Vector3 gloabalSpeedVector;

        if (_inputModel.IsSprintPressed)
        {
            gloabalSpeedVector = direction * (_movePlayerControllerModel.MoveSpeed * _movePlayerControllerModel.RunSpeedMult) * fixedDeltaTime;
            _playerModel.SetIsRun(true);
            _playerModel.SetIsWalk(false);
        }
        else
        {
            gloabalSpeedVector = direction * _movePlayerControllerModel.MoveSpeed * fixedDeltaTime;
            _playerModel.SetIsRun(false);
            _playerModel.SetIsWalk(true);
        }

        var localSpeedVector = _playerModel.PlayerViewTransform.TransformDirection(gloabalSpeedVector);
        localSpeedVector.y = _playerModel.PlayerRB.velocity.y;

        _playerModel.PlayerRB.velocity = localSpeedVector;
    }

    public void Jump()
    {
        if (_inputModel.JumpValue > 0)
        {
            if (_playerModel.IsGrounded && !_playerModel.IsJumped)
            {
                _playerModel.PlayerRB.AddForce(Vector3.up * _jumpForce * 50);
                _playerModel.SetJumped(true);
            }
        }
        else if (_inputModel.JumpValue == 0 && _playerModel.IsJumped)
        {
            _playerModel.SetJumped(false);
        }

        if (!_playerModel.IsGrounded)
        {
            _playerModel.PlayerRB.AddForce(Vector3.down * GRAVITY);
        }
    }
}


