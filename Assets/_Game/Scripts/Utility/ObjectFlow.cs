using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ObjectPool helps to reduce the object creation overhead 
/// by allowing to use the previously created objects of the same type if available.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T>
{
    /// <summary>
    /// InstantiateEvent defines the object creation logic.
    /// </summary>
    /// <returns>Returns a new object by using the given method.</returns>
    public delegate T InstantiateEvent();

    private InstantiateEvent _instantiateEvent;

    // Pool of the previously created objects.
    private Queue<T> _pool;

    /// <summary>
    /// Create the object pool with the object creation logic. 
    /// </summary>
    /// <param name="instantiateEvent"></param>
    public ObjectPool(InstantiateEvent instantiateEvent)
    {
        _pool = new Queue<T>();
        _instantiateEvent = instantiateEvent;
    }

    /// <summary>
    /// Get an object from the pool.
    /// </summary>
    /// <returns>Returns an existing object if available in the pool or creates a new one.</returns>
    public T Get()
    {
        var selected = _pool.Count > 0 ? _pool.Dequeue() : _instantiateEvent();
        return selected;
    }

    /// <summary>
    /// Put the object back into the pool to be used later.
    /// </summary>
    /// <param name="selected"></param>
    public void Return(T selected)
    {
        _pool.Enqueue(selected);
    }

    public void Clear()
    {
        _pool.Clear();
    }
}

/// <summary>
/// FlowObject is the signle moving object in a flow.
/// </summary>
public class FlowObject
{
    public enum State
    {
        explosion,
        flow
    }

    public Vector3 target;
    public Transform transform;
    public Vector3 startPosition;
    public Vector3 direction;
    public float startTime;
    public AnimationCurve curveX;
    public AnimationCurve curveY;
    public float speed;
    public State state;

    /// <summary>
    /// Init resets the values of the object with given parameters.
    /// </summary>
    /// <param name="curveX">Defines how the object moves along the X axis over time.</param>
    /// <param name="curveY">Defines how the object moves along the Y axis over time.</param>
    /// <param name="startPosition">Initial position of the object.</param>
    /// <param name="target">The position in the space that the object will be after the flow.</param>
    /// <param name="speed">Flowing speed of the individual object.</param>
    internal void Init(AnimationCurve curveX, AnimationCurve curveY, Vector3 startPosition, Vector3 target, float speed)
    {
        this.curveX = curveX;
        this.curveY = curveY;
        startTime = 0f;
        this.startPosition = startPosition;
        this.target = target;
        direction = target - startPosition;
        this.speed = speed;
        transform.position = startPosition;
    }

    /// <summary>
    /// Changes the state of the flow object to Activate or Deactive. 
    /// </summary>
    /// <param name="state"></param>
    public void SetActive(bool state)
    {
        transform.gameObject.SetActive(state);
    }
}

/// <summary>
/// ObjectFlow generates desired amount of objects in a location in the canvas and moves them to target location
/// with predefined settings. 
/// </summary>
public class ObjectFlow : MonoBehaviour
{
    [Header("Projectile Setup")]
    /// <summary>
    /// Base instance of the objects in the flow. 
    /// </summary>
    public GameObject Projectile;

    public int ProjectileAmount;

    /// <summary>
    /// Minimum possible speed of an object that flows to the target. 
    /// </summary>
    public float MinSpeed;

    /// <summary>
    /// Maximum possible speed of an object that flows to the target. 
    /// </summary>
    public float MaxSpeed;

    [Header("Explosion Stage")]
    /// <summary>
    /// Scattering velocity of the objects. 
    /// </summary>
    public float ExplosionSpeed;

    /// <summary>
    /// The maximum possible distance between two objects.
    /// </summary>
    public float SpreadRadius;

    /// <summary>
    /// Changes how the objects spread along the X axis.
    /// </summary>
    public AnimationCurve ExplosionCurveX;

    /// <summary>
    /// Changes how the objects spread along the Y axis.
    /// </summary>
    public AnimationCurve ExplosionCurveY;

    [Header("Flowing Stage")]
    /// <summary>
    /// Target location that the objects are going to flow.
    /// </summary>
    public Transform Target;

    /// <summary>
    /// Changes how the objects flow along the X axis.
    /// </summary>
    public AnimationCurve FlowCurveX;

    /// <summary>
    /// Changes how the objects flow along the Y axis.
    /// </summary>
    public AnimationCurve FlowCurveY;

    /// <summary>
    /// Container of the currently flowing objects. 
    /// </summary>
    List<FlowObject> projectiles;

    /// <summary>
    /// Uses the existing projectiles to reduce the UI overhead
    /// </summary>
    ObjectPool<FlowObject> pool;

    private Action _onEnd;

    void Start()
    {
        enabled = false;
        pool = new ObjectPool<FlowObject>(CreateProjectile);
    }

    /// <summary>
    ///  Helps projectile pool to create new projectiles
    /// </summary>
    /// <returns>A new projectile that has the same transform with the original one.</returns>
    private FlowObject CreateProjectile()
    {
        var icon = Instantiate(Projectile, transform);
        return new FlowObject() {transform = icon.transform};
    }

    /// <summary>
    /// Moves the spawned projectiles towards the target
    /// </summary>
    private void MoveObjects()
    {
        // Move each object to the target within the flow duration
        for (int i = 0; i < projectiles.Count; i++)
        {
            var projectile = projectiles[i];
            projectile.startTime += Time.deltaTime / projectile.speed;
            var passTime = Mathf.Clamp(projectile.startTime, 0, 1);

            if (projectile.state == FlowObject.State.explosion)
            {
                var posY = projectile.startPosition.y + (projectile.curveY.Evaluate(passTime) * projectile.direction.y);
                var posX = projectile.startPosition.x + (projectile.curveX.Evaluate(passTime) * projectile.direction.x);
                var currentPosition = new Vector3(posX, posY, projectile.transform.position.z);
                projectile.transform.position = currentPosition;

                // Start flowing to the target if the particle is close enough to the expected explosion radius
                if (Vector3.Distance(projectile.transform.position, projectile.target) < 0.01f)
                {
                    var projectileSpeed = UnityEngine.Random.Range(1 / MinSpeed, 1 / MaxSpeed);
                    projectile.Init(FlowCurveX, FlowCurveY, projectile.transform.position, Target.position,
                        projectileSpeed);
                    projectile.state = FlowObject.State.flow;
                }
            }
            else if (projectile.state == FlowObject.State.flow)
            {
                projectile.direction = Target.position - projectile.startPosition;
                projectile.target = Target.position;
                var posY = projectile.startPosition.y + (projectile.curveY.Evaluate(passTime) * projectile.direction.y);
                var posX = projectile.startPosition.x + (projectile.curveX.Evaluate(passTime) * projectile.direction.x);
                var currentPosition = new Vector3(posX, posY, projectile.transform.position.z);
                projectile.transform.position = currentPosition;

                // Start flowing to the target if the particle is close enough to the expected explosion radius
                if (Vector3.Distance(projectile.transform.position, projectile.target) < 0.01f)
                {
                    projectiles.RemoveAt(i);
                    i--;
                    projectile.transform.gameObject.SetActive(false);
                    pool.Return(projectile);

                    if (projectiles.Count == 0)
                    {
                        enabled = false;
                        _onEnd?.Invoke();
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Spawns projectiles and spread them randomly.
    /// </summary>
    public void Flow(Action onEnd = null)
    {
        if (enabled)
        {
            Stop();
        }

        _onEnd = onEnd;
        projectiles = new List<FlowObject>();
        for (int i = 0; i < ProjectileAmount; i++)
        {
            var marginX = UnityEngine.Random.Range(-SpreadRadius, SpreadRadius);
            var marginY = UnityEngine.Random.Range(-SpreadRadius, SpreadRadius);
            var flowObject = pool.Get();
            flowObject.SetActive(true);

            flowObject.transform.position = transform.position;
            var target = new Vector3(transform.position.x + marginX, transform.position.y + marginY,
                transform.position.z);

            flowObject.Init(ExplosionCurveX, ExplosionCurveY, transform.position, target, 1 / ExplosionSpeed);
            flowObject.state = FlowObject.State.explosion;
            projectiles.Add(flowObject);
        }

        enabled = true;
    }

    void Update()
    {
        MoveObjects();
    }

    public void Stop()
    {
        if (enabled)
        {
            enabled = false;
            Clear();
            _onEnd?.Invoke();
        }
    }

    private void Clear()
    {
        foreach (var projectile in projectiles)
        {
            projectile.transform.gameObject.SetActive(false);
            pool.Return(projectile);
        }

        projectiles.Clear();
    }
}