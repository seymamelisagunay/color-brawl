using UnityEngine;

public interface IRepeater
{
    bool Set(float rateSecond);
}

public class Repeater : IRepeater
{
    private float _targetTime;
    public bool Set(float rateSecond)
    {
        if (Time.time < _targetTime) return false;

        _targetTime = Time.time + rateSecond;
        return true;
    }
}
