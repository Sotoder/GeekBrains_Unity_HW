using System;

public class InputControllerModel: IInputModel
{
    public bool IsFPressed { get; private set; }
    public bool IsYPressed { get; private set; }
    public bool IsEscPressed { get; private set; }
    public bool IsFirstWeaponPressed { get; private set; }
    public bool IsSecondWeaponPressed { get; private set; }
    public bool IsSprintPressed { get; private set; }

    public float LMBValue { get; private set; }
    public float RMBValue { get; private set; }
    public float CMBValue { get; private set; }
    public float JumpValue { get; private set; }
    public float HorizontalValue { get; private set; }
    public float VerticalValue { get; private set; }
    public float MouseXValue { get; private set; }
    public float MouseYValue { get; private set; }

    public void SetHorizontalValue(float value)
    {
        HorizontalValue = value;
    }

    public void SetVerticalValue(float value)
    {
        VerticalValue = value;
    }

    public void SetMouseXValue(float value)
    {
        MouseXValue = value;
    }

    public void SetMouseYValue(float value)
    {
        MouseYValue = value;
    }

    public void SetLMBValue(float value)
    {
        LMBValue = value;
    }

    public void SetRMBValue(float value)
    {
        RMBValue = value;
    }

    public void SetCMBValue(float value)
    {
        CMBValue = value;
    }

    public void SetJumpValue(float value)
    {
        JumpValue = value;
    }

    public void SetFValue(bool value)
    {
        IsFPressed = value;
    }

    public void SetYValue(bool value)
    {
        IsYPressed = value;
    }

    public void SetEscValue(bool value)
    {
        IsEscPressed = value;
    }

    public void SetFirstWeaponValue(bool value)
    {
        IsFirstWeaponPressed = value;
    }

    public void SetSecondWeaponValue(bool value)
    {
        IsSecondWeaponPressed = value;
    }

    public void SetSprintValue(bool value)
    {
        IsSprintPressed = value;
    }
}
