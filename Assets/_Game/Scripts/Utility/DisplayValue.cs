using System;
using UnityEngine;

namespace Game.UI
{
    public class DisplayValue
    {
        private float _startValue;
        private float _targetValue;
        private float _speed;
        private Action<float> _onTrigger;
        private Action _onDone;
        private bool _isRun;
        private float _lastTriggerValue;
        private float _triggerDelta;
        public float Value { get; private set; }
        public float Percent { get; private set; }


        public DisplayValue(float startValue, float targetValue, float time,
            Action<float> onTrigger = null,
            Action onDone = null,
            float triggerDelta = 1)
        {
            _startValue = startValue;
            _targetValue = targetValue;
            _speed = Mathf.Abs(targetValue - startValue) / time;
            _onTrigger = onTrigger;
            _onDone = onDone;
            _lastTriggerValue = startValue;
            _triggerDelta = triggerDelta;
            Value = startValue;
            _isRun = true;
        }

        public void Update()
        {
            if (!_isRun) return;

            Value = Mathf.MoveTowards(Value, _targetValue, _speed * Time.deltaTime);

            Percent = (Value - _startValue) / (_targetValue - _startValue);

            if (Math.Abs(_lastTriggerValue - Value) >= _triggerDelta)
            {
                _lastTriggerValue = Value;
                _onTrigger?.Invoke(_lastTriggerValue);
            }

            if (Value == _targetValue)
            {
                _onDone?.Invoke();
                _isRun = false;
            }
        }

        public void Stop()
        {
            _isRun = false;
        }
    }
}