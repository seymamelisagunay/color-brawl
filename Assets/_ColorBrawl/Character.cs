using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed;
    public float JumpHeight;

    public Vector2 moveDirection;
    private float prevY;
    private float prevX;
    private Rigidbody2D rigidBody;
    public bool Jump;
    public GameObject touchingPlatform;
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

    void Awake()
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

    void Update()
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

    void FixedUpdate()
    {
        if (levelManager.Waiting) return;
        if (levelManager.Ended) return;

        if (Jump)
        {
            if (bottomDetector.Touching())
            {
                rigidBody.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
                rigidBody.velocity = Vector2.zero;
                Jump = false;
                rigidBody.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
            }
            else if (rightDetector.Touching() || leftDetector.Touching())
            {
                moveDirection = -moveDirection;
                rigidBody.velocity = Vector2.zero;
                Jump = false;
                rigidBody.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);

                if (moveDirection == Vector2.right)
                {
                    visual.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    visual.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
        else if ((rightDetector.Touching() || leftDetector.Touching())
                 && !bottomDetector.Touching())
        {
            var vel = rigidBody.velocity;
            vel.y /= 1.15f;
            rigidBody.velocity = vel;
        }

        if (rigidBody.isKinematic) return;
        var velocity = rigidBody.velocity;
        velocity.x = moveDirection.x * Speed * Time.fixedDeltaTime;
        rigidBody.velocity = velocity;
    }
}