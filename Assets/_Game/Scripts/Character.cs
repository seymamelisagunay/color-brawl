using _ColorBrawl;
using UnityEngine;

namespace _Game.Scripts
{
    public class Character : MonoBehaviour
    {
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
        public ILevelManager LevelManager { get; set; }
        public bool Bot;
        public string characterID;
        public GameObject visual;
        private float jumpTimeout = 0.2f;
        private float jumpExpiredTime;
        private float slideTimer;
        private bool Slide;
        private float slideBeginTime;
        private float _jumpDuration = 0.45f;
        private float _afterJumpSliceStartTime;

        private void Awake()
        {
            rigidBody = gameObject.GetComponent<Rigidbody2D>();
            rigidBody.isKinematic = true;
            leftDetector.OnFirstTouch += OnFirstTouch;
            rightDetector.OnFirstTouch += OnFirstTouch;
            bottomDetector.OnFirstTouch += OnBottomFirstTouch;
            enabled = false;
        }

        public void StartLevel(ILevelManager levelManager)
        {
            gameObject.SetActive(true);
            LevelManager = levelManager;
            enabled = true;
            rigidBody.isKinematic = false;
            rigidBody.gravityScale = 3f;
        }

        private void OnBottomFirstTouch()
        {
            Slide = false;
        }

        private void OnFirstTouch()
        {
            StartSlide();
        }

        private void StartSlide()
        {
            if (Slide) return;
            Slide = true;
            slideBeginTime = Time.time;
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
            if (LevelManager.Ended)
            {
                Stop();
            }

            if (LevelManager.Waiting) return;
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
            if (LevelManager.Waiting) return;
            if (LevelManager.Ended) return;
            if (rigidBody.isKinematic) return;


            if (_afterJumpSliceStartTime < Time.time
                && !bottomDetector.Touching() && (rightDetector.Touching() || leftDetector.Touching()))
            {
                StartSlide();
            }

            var velocity = rigidBody.velocity;
            velocity.x = moveDirection.x * Speed * Time.fixedDeltaTime;
            rigidBody.velocity = velocity;


            if (Jump)
            {
                if (bottomDetector.Touching())
                {
                    Jump = false;
                    Slide = false;
                    rigidBody.velocity = Vector2.zero;
                    rigidBody.AddForce(Vector2.up * JumpHeight);
                    _afterJumpSliceStartTime = Time.time + _jumpDuration;
                }
                else if (rightDetector.Touching()
                         || leftDetector.Touching())
                {
                    Jump = false;
                    Slide = false;
                    moveDirection = -moveDirection;
                    rigidBody.velocity = Vector2.zero;
                    rigidBody.AddForce(Vector2.up * JumpHeight);

                    if (moveDirection == Vector2.right)
                        visual.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    else
                        visual.transform.localRotation = Quaternion.Euler(0, 180, 0);

                    _afterJumpSliceStartTime = Time.time + _jumpDuration;
                }
            }


            if (Slide)
            {
                var slicePassTime = Time.time - slideBeginTime;
                var vel = rigidBody.velocity;
                vel.y = sideSlipSpeed * Mathf.Lerp(0, -5, slicePassTime / 2f);
                rigidBody.velocity = vel;
            }
        }
    }
}