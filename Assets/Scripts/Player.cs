using System;
using System.Collections;
using Enums;
using Obstacles;
using PowerUps;
using UnityEngine;
using Utilities;

public class Player : MonoBehaviour
{
    [SerializeField] public float defaultRunningSpeed;
    [SerializeField] private float platformWidth;
    [SerializeField] private CameraMovements cameraMovements;
    [SerializeField] private float jumpHeight;
    [SerializeField] private bool cheatingMode;
    [SerializeField] private float rollbackSpeed;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float minSpeed;

    [NonSerialized] public float RunningSpeed;
    private float _leftBorder, _rightBorder;
    private Animator _animator;
    private float _screenWidth;
    private float _zDir, _xDir, _yDir;
    private float _touchStartPositionX;
    private bool _isDragging;
    private float _currentPositionX, _dif, _xMove, _projectedX, _clampedProjected, _moveAmount;
    private static readonly int RunningHash = Animator.StringToHash("Running");
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int FallHash = Animator.StringToHash("Fall");

    private void Idle() => _animator.SetTrigger(IdleHash);
    private void Running() => _animator.SetTrigger(RunningHash);
    private void Fall() => _animator.SetTrigger(FallHash);

    private void Awake()
    {
        _screenWidth = Screen.width;
        RunningSpeed = defaultRunningSpeed;
        _zDir = defaultRunningSpeed;
        _leftBorder = -platformWidth / 2;
        _rightBorder = platformWidth / 2;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        GameManager.Instance.OnGameStarted += Running;
        GameManager.Instance.OnPaused += Idle;
        GameManager.Instance.OnResumed += Running;
        GameManager.Instance.OnGameOver += Idle;
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing) return;

        RunningSpeed = Mathf.Lerp(RunningSpeed, defaultRunningSpeed, Time.deltaTime * rollbackSpeed);

        _zDir = RunningSpeed;
        if (Input.GetMouseButtonDown(0))
        {
            if (Helpers.IsCursorOverUI()) return;
            _isDragging = true;
            _touchStartPositionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            if (Helpers.IsCursorOverUI()) _isDragging = false;

            _currentPositionX = Input.mousePosition.x;
            _dif = Mathf.Clamp(_currentPositionX - _touchStartPositionX, -_screenWidth, _screenWidth);
            _xMove = _dif / _screenWidth * platformWidth;

            var position = transform.position;
            _projectedX = position.x + _xMove;
            _clampedProjected = Mathf.Clamp(_projectedX, _leftBorder, _rightBorder);
            _moveAmount = _clampedProjected - position.x;
            _xDir = _moveAmount;
            _touchStartPositionX = _currentPositionX;

            cameraMovements.Rotate(position.x, 0);
        }
        else _xDir = 0;

        transform.Translate(_xDir, 0, _zDir * Time.deltaTime);
    }

    private IEnumerator JumpCoroutine()
    {
        var journeyTime = 0.6f;
        var startPos = transform.position;
        // Jump to the jump height and then back down to the ground

        var t = 0.0f;
        while (t < journeyTime)
        {
            t += Time.deltaTime;
            var y = Mathf.Sin(t * Mathf.PI / journeyTime) * jumpHeight + startPos.y;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void JumpOverObstacle()
    {
        StartCoroutine(JumpCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Jumper"))
        {
            JumpOverObstacle();
        }
        else if (other.gameObject.CompareTag("ScoreTrigger"))
        {
            ScoreManager.instance.Score++;
        }
        else if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Moving"))
        {
            if (cheatingMode) return;
            other.gameObject.GetComponentInParent<ObstacleSetup>().DeactivateColliders();
            GameManager.Instance.LostLife();
        }
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            print("PowerUp trigger");
            other.gameObject.GetComponent<PowerUp>().Activate(this);
        }
        else if (other.gameObject.CompareTag("Missed"))
        {
            other.gameObject.GetComponentInParent<PowerUp>().Missed();
        }
    }
}