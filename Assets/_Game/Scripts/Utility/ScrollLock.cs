using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollLock : MonoBehaviour
{
    public Transform lockPivot;
    public ScrollRect Scroll { get; private set; }
    private Vector3 _targetPosition;
    private float _deltaMagnitude;
    private bool _isRun;
    private float _speed;
    private Transform _lockItem;
    private void Awake()
    {
        Scroll = GetComponent<ScrollRect>();
    }
    private void Update()
    {
        if (_isRun)
        {
            Scroll.content.position = Vector3.Lerp(Scroll.content.position, _targetPosition, Time.deltaTime * 10f * _speed);
            if (Vector3.Distance(_targetPosition, Scroll.content.position) < (50f))
            {
                _isRun = false;
                Scroll.enabled = true;
            }
        }
    }
    public void Lock(Transform lockItem, float delay = 0f, float speed = 1f)
    {
        _lockItem = lockItem;
        _speed = speed;
        if (delay > 0)
            Invoke(nameof(Lock), delay);
        else
            Lock();
    }

    private void Lock()
    {
        Scroll.velocity = Vector2.zero;
        Scroll.enabled = false;
        var deltaPosition = lockPivot.position - _lockItem.position;
        deltaPosition.x = 0;
        _deltaMagnitude = deltaPosition.magnitude;
        _targetPosition = Scroll.content.position + deltaPosition;
        _isRun = true;
    }
}
