using System;

public interface ISubscribavleProperty
{
    void SetValue(float value);
    void Subscibe(Action<float> action);
    void Unsubscribe(Action<float> action);
}