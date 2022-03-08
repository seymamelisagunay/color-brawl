using UnityEngine;

public class Alarm
{
    private float _targetTime;
    public bool IsStart { get; private set; }
    public bool IsPause { get; private set; }
    public float RemainingTime => _targetTime - Time.time;
    public float Percent => (_fullTime - (IsPause ? _resumeTime : RemainingTime)) / _fullTime;
    private float _resumeTime;
    private float _fullTime;

    public void Start(float rateSecond)
    {
        _fullTime = rateSecond;
        _targetTime = Time.time + rateSecond;
        IsStart = true;
    }

    public void Stop()
    {
        IsStart = false;
    }

    public bool Check()
    {
        if (!IsStart) return false;
        if (IsPause) return false;
        if (Time.time < _targetTime) return false;

        IsStart = false;
        return true;
    }

    public void Resume()
    {
        if (IsPause)
        {
            IsPause = false;
            _targetTime = Time.time + _resumeTime;
        }
    }

    public void Pause()
    {
        if (!IsPause)
        {
            IsPause = true;
            _resumeTime = RemainingTime;
        }
    }
}