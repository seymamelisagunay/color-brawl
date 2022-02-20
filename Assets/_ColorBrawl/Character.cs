using System.Collections;
using System.Collections.Generic;
using _ColorBrawl;
using UnityEditor.Rendering;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    private float Speed = 250f;
    private float JumpHeight = 600f;
    private float JumpRange = 300f;
    private float sideSlipSpeed = 2f;

    public Vector2 moveDirection;
    private float prevY;
    private float prevX;
    private Rigidbody2D rigidBody;
    public bool Jump;
    public SpriteRenderer sprite;
    public TouchDetector leftDetector;
    public TouchDetector rightDetector;
    public TouchDetector bottomDetector;
    public bool Bot;
    public string characterID;
    public GameObject visual;
    public LevelManager levelManager;
    private float jumpTimeout = 0.2f;
    private float jumpExpiredTime;
    private float sliceTimer;
    private bool onSlice;
    private float sliceBeginTime;

    private void Awake()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;
    }

    public void EnableMovement()
    {
        if (!gameObject.active) return;

        rigidBody.isKinematic = false;
        rigidBody.gravityScale = 3f;
    }

    public void Stop()
    {
        if (gameObject.active)
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 0.0f;
        }
    }

    public void JumpUp()
    {
        Jump = true;
        jumpExpiredTime = Time.time + jumpTimeout;
    }

    private void Update()
    {
        if (levelManager.Ended)
        {
            Stop();
        }

        if (levelManager.Waiting) return;
        if (Bot) return;

        if (Input.GetMouseButtonDown(0))
        {
            JumpUp();
        }

        if (Jump && Time.time > jumpExpiredTime)
        {
            Jump = false;
        }
    }

    private void FixedUpdate()
    {
        if (levelManager.Waiting) return;
        if (levelManager.Ended) return;

        if (Jump)
        {
            if (bottomDetector.Touching())
            {
                Jump = false;
                rigidBody.velocity = Vector2.zero;
                rigidBody.AddForce(Vector2.up * JumpHeight);
            }
            else if (rightDetector.Touching()
                     || leftDetector.Touching())
            {
                Jump = false;
                onSlice = false;
                moveDirection = -moveDirection;
                rigidBody.velocity = Vector2.zero;
                rigidBody.AddForce(Vector2.up * JumpHeight
                                   + moveDirection * JumpRange);

                if (moveDirection == Vector2.right)
                    visual.transform.localRotation = Quaternion.Euler(0, 0, 0);
                else
                    visual.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if ((rightDetector.Touching() || leftDetector.Touching())
                 && !bottomDetector.Touching())
        {
            if (onSlice == false)
            {
                sliceBeginTime = Time.time;
                onSlice = true;
            }

            var slicePassTime = Time.time - sliceBeginTime;
            var vel = rigidBody.velocity;
            vel.y = sideSlipSpeed * Mathf.Lerp(0, -5, slicePassTime / 2f);
            rigidBody.velocity = vel;
        }
        else
        {
            if (bottomDetector.Touching() &&
                (rightDetector.Touching() || leftDetector.Touching()))
            {
                moveDirection = -moveDirection;
            }
        }

        if (rigidBody.isKinematic) return;

        if (bottomDetector.Touching())
        {
            var velocity = rigidBody.velocity;
            velocity.x = moveDirection.x * Speed * Time.fixedDeltaTime;
            rigidBody.velocity = velocity;
            onSlice = false;
        }
    }
}