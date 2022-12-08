using UnityEngine;

public class PlayerAnimationController
{
    private readonly IInputModel _inputModel;
    private PlayerModel _playerModel;
    private WeaponController _weaponController;
    private IWeaponModel _currentWeaponModel => _weaponController.CurrentWeaponModel;

    public PlayerAnimationController(PlayerModel playerModel, IInputModel inputModel, WeaponController weaponController)
    {
        _playerModel = playerModel;
        _inputModel = inputModel;
        _weaponController = weaponController;
    }

    public void Update()
    {
        if(_inputModel.IsFirstWeaponPressed)
        {
            _playerModel.PlayerAnimator.SetBool("MGun", true);
            _playerModel.PlayerAnimator.SetBool("SGun", false);
        }
        else if (_inputModel.IsSecondWeaponPressed)
        {
            _playerModel.PlayerAnimator.SetBool("MGun", false);
            _playerModel.PlayerAnimator.SetBool("SGun", true);
        }

        if (_inputModel.LMBValue == 1f && _currentWeaponModel.IsReload && _currentWeaponModel.CurrentWeaponAmmoCount > 0)
        {
            _playerModel.PlayerAnimator.SetBool("Fire", true);
        }
        else if (_inputModel.LMBValue == 0f || _currentWeaponModel.CurrentWeaponAmmoCount == 0)
        {
            _playerModel.PlayerAnimator.SetBool("Fire", false);
        }
    }

    public void FixedUpdate()
    {
        if (!_playerModel.IsJumped)
        {
            if (_playerModel.PlayerRB.velocity != Vector3.zero)
            {
                _playerModel.PlayerAnimator.SetBool("Run", true);
            }
            else
            {
                _playerModel.PlayerAnimator.SetBool("Run", false);
            }
        }
    }
}
