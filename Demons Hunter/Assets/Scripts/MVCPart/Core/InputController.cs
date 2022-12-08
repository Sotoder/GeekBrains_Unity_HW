using UnityEngine;

public class InputController : IUpdateble
{
    public IInputModel InputModel => _inputControllerModel;

    private InputControllerModel _inputControllerModel;

    public void Update(float deltaTime)
    {

        _inputControllerModel.SetHorizontalValue(Input.GetAxis("Horizontal"));
        _inputControllerModel.SetVerticalValue(Input.GetAxis("Vertical"));
        _inputControllerModel.SetMouseXValue(Input.GetAxis("Mouse X"));
        _inputControllerModel.SetMouseYValue(Input.GetAxis("Mouse Y"));
        _inputControllerModel.SetLMBValue(Input.GetAxis("Fire1"));
        _inputControllerModel.SetRMBValue(Input.GetAxis("Fire2"));
        _inputControllerModel.SetCMBValue(Input.GetAxis("Fire3"));
        _inputControllerModel.SetJumpValue(Input.GetAxis("Jump"));
        
        _inputControllerModel.SetFValue(Input.GetKeyDown(KeyCode.F));
        _inputControllerModel.SetYValue(Input.GetKeyDown(KeyCode.Y));
        _inputControllerModel.SetFirstWeaponValue(Input.GetButton("Weapon1"));
        _inputControllerModel.SetSecondWeaponValue(Input.GetButton("Weapon2"));
        _inputControllerModel.SetEscValue(Input.GetButton("Cancel"));
        _inputControllerModel.SetSprintValue(Input.GetButton("Sprint"));
    }
}
