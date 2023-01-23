using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Camera _camera;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _character;

    [Header("Movement Values")]
    [SerializeField] private float _speed; // 7, 3.5
    [SerializeField] private float _jumpHeight; // 7, 2
    [SerializeField] private float _rotationSmoothTime; // 0.05
    [SerializeField] private float _runModifier; // 1.5

    private Vector2 _directionInput;
    private float _currentAngle;
    private float _currentAngleVelocity;

    private bool _isJumping;
    private bool _isRunning;
    [SerializeField] private bool _isGrounded;

    [Header("Gravity")]
    [SerializeField] private float _gravity; // 9.8
    [SerializeField] private float _gravityModifier;  // 3
    private float _coyoteTime = 0;
    [Tooltip("Value In Frames")]
    [SerializeField] float _maxCoyoteTime;

    private Vector3 _direction;
    private float _yVelocity;

    private bool _footstepsPlaying;
    public bool acceptingInputs;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _animator = _character.GetComponent<Animator>();
        acceptingInputs = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(_character.transform.position);
        //Debug.Log(Vector3.down * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        _direction = Vector3.zero;
        if (acceptingInputs)
        {
            Movement();    
        }
        HandleGravity();
    }

    private void OnDrawGizmos()
    {
        float scale = transform.localScale.y;
        //Debug.Log("scale : " + scale);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_character.transform.position + (Vector3.up * 0.5f * scale), 0.5f * scale);
    }

    private void FixedUpdate()
    {
        float scale = transform.localScale.y;
        int layerMask = 1 << 3;

        RaycastHit hit;
        if (Physics.SphereCast(_character.transform.position + (Vector3.up * 0.5f * scale), 0.5f * scale, Vector3.down, out hit, 0.1f, layerMask))
        {
            _isGrounded = true;
        }
        else
        {
            if (_isGrounded && !_isJumping)
            {
                _coyoteTime += 1;
                if (_coyoteTime > _maxCoyoteTime)
                {
                    _isGrounded = false;
                    _coyoteTime = 0;
                }
            }
            if (_isJumping)
            {
                _isGrounded = false;
            }
        }
    }

    public void GetMovementValues(InputAction.CallbackContext context)
    {
        _directionInput = context.ReadValue<Vector2>();
    }

    private void Movement()
    {
        _direction = new Vector3(_directionInput.x, 0, _directionInput.y);

        if(_direction.magnitude >= 0.1f)
        {
            if (!_footstepsPlaying)
            {
                if (AudioManager.Instance)
                {
                    AudioManager.Instance.Play("Footsteps");
                }
                _footstepsPlaying = true;
            }

            if (_animator != null)
            {
                _animator.SetBool("Moving", true);
            }

            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _currentAngle = Mathf.SmoothDampAngle(_currentAngle, targetAngle, ref _currentAngleVelocity, _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, _currentAngle, 0);
            Vector3 rotatedMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(rotatedMovement * _speed * Time.deltaTime);
        }
        else
        {
            if (_footstepsPlaying)
            {
                if (AudioManager.Instance)
                {
                    AudioManager.Instance.Stop("Footsteps");
                }
                _footstepsPlaying = false;
            }

            if (_animator != null)
            {
                _animator.SetBool("Moving", false);
            }
        }
    }

    private void HandleGravity()
    {
        if (_isGrounded)
        {
            if (_isJumping)
            {
                _isJumping = false;
            }
            else
            {
                _yVelocity = -0.1f;
            }
        }

        _yVelocity -= _gravity * _gravityModifier * Time.deltaTime;
        _controller.Move(Vector3.up * _yVelocity * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (acceptingInputs)
        {
            if (context.started && _isGrounded)
            {
                _yVelocity = Mathf.Sqrt(_jumpHeight * 2f * _gravity);
                _isJumping = true;
                _isGrounded = false;
                _coyoteTime = 0;
                if (_animator != null)
                {
                    _animator.SetTrigger("Jump");
                }
            }
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.started && !_isJumping)
        {
            _speed *= _runModifier;
            _isRunning = true;
            if (_animator != null)
            {
                _animator.SetFloat("Speed", 1);
                _animator.SetFloat("WalkingMultiplier", 1.0f);
            }
        }

        if (context.canceled && _isRunning)
        {
            _speed /= _runModifier;
            _isRunning = false;
            if (_animator != null)
            {
                _animator.SetFloat("Speed", 0);
                _animator.SetFloat("WalkingMultiplier", 1.3f);
            }
        }
    }
}
