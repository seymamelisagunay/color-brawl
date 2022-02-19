using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed;
    public float JumpHeight;

    public bool Touching;
    public string touchSide;
    public Vector2 moveDirection;
    private float prevY;
    private float prevX;
    public bool InAir;
    private Rigidbody2D rigidBody;
    public bool Jump;
    public bool OnEdge;
    public GameObject touchingPlatform;
    public SpriteRenderer sprite;
    public TouchDetector leftDetector;
    public TouchDetector rightDetector;
    public TouchDetector bottomDetector;
    public bool Bot;
    public string characterID;
    public GameObject visual;
    public LevelManager levelManager;
    private float jumpTimeout = 0.1f;
    private float jumpTapTime;
    void Awake()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;

    }
    public void EnableMovement() {
        if(gameObject.active)
        rigidBody.isKinematic = false;
    }
    public void JumpUp()
    {
        if (Touching == false)
        {
            Jump = true;
        }

    }
    void Update()
    {

        if (levelManager.Ended) return;
        if (levelManager.Waiting) return;
        if (Bot) return;

        if (Input.GetMouseButtonDown(0))
        {
            JumpUp();
            Touching = true;
            jumpTapTime = Time.time + jumpTimeout;


        }
        else
        {
            Touching = false;
            jumpTapTime = Time.time;
        }

    }
    void FixedUpdate()
    {
        if (levelManager.Waiting) return;
        if (levelManager.Ended) return;

        if (Jump )
        {
            if(Time.time < jumpTimeout) return;
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
        else if ((rightDetector.Touching() || leftDetector.Touching()) && !bottomDetector.Touching())
        {
            var vel = rigidBody.velocity;
            vel.y /= 1.15f;
            rigidBody.velocity = vel;
        }
        var velocity = rigidBody.velocity;
        velocity.x = moveDirection.x * Speed * Time.fixedDeltaTime;
        rigidBody.velocity = velocity;

    }
}
