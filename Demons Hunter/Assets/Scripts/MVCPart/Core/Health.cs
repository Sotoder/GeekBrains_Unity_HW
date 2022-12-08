using System;

public class Health: ISubscribavleProperty
{
    private Action<float> _onValueChange;
    private float _currentValue;
    private float _maxValue;

    public float CurrentValue => _currentValue;
    public float MaxValue => _maxValue;

    public Health(float maxHP)
    {
        _currentValue = maxHP;
        _maxValue = maxHP;
    }

    public void SetValue(float value)
    {
        if(value < 0)
        {
            _currentValue = 0;
        }
        else 
        {
            _currentValue = value;
        }
        _onValueChange?.Invoke(value);
    }

    public void Subscibe(Action<float> action)
    {
        _onValueChange += action;
    }

    public void Unsubscribe(Action<float> action)
    {
        _onValueChange -= action;
    }
}
