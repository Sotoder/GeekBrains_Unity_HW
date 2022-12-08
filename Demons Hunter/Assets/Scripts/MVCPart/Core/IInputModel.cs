using System;

public interface IInputModel
{
    public bool IsFPressed { get; }
    public bool IsYPressed { get; }
    public bool IsEscPressed { get; }
    public bool IsFirstWeaponPressed { get; }
    public bool IsSecondWeaponPressed { get; }
    public bool IsSprintPressed { get; }
    public float HorizontalValue { get; }
    public float VerticalValue { get; }
    public float MouseXValue { get; }
    public float MouseYValue { get; }
    public float LMBValue { get; }
    public float RMBValue { get; }
    public float CMBValue { get; }
    public float JumpValue { get; }
}
