using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Camera _camera;

    [Header("Movement Values")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _rotationSmoothTime;
    [SerializeField] private float _runModifier;

    private Vector2 _directionInput;
    private float _currentAngle;
    private float _currentAngleVelocity;
    private bool _isJumping;
    private bool _isRunning;

    [Header("Gravity")]
    [SerializeField] private float _gravity;
    [SerializeField] private float _gravityModifier;

    [Header("Camera")]

    private Vector3 _direction;
    private float _yVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main;
    }
    void Start()
    {
    }

    private void Update()
    {
        Movement();
        HandleGravity();
    }

    public void GetMovementValues(InputAction.CallbackContext context)
    {
        _directionInput = context.ReadValue<Vector2>();
    }

    private void Movement()
    {
        _direction = new Vector3(_directionInput.x, 0, _directionInput.y);

        if (_direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _currentAngle = Mathf.SmoothDampAngle(_currentAngle, targetAngle, ref _currentAngleVelocity, _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, _currentAngle, 0);
            Vector3 rotatedMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(rotatedMovement * _speed * Time.deltaTime);
        }
    }

    private void HandleGravity()
    {
        if (_controller.isGrounded && _isJumping)
        {
            _isJumping = false;
        }

        _yVelocity -= _gravity * _gravityModifier * Time.deltaTime;

        _controller.Move(Vector3.up * _yVelocity * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && _controller.isGrounded)
        {
            _yVelocity = Mathf.Sqrt(_jumpHeight * 2f * _gravity);
            _isJumping = true;
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.started && !_isJumping)
        {
            _speed *= _runModifier;
            _isRunning = true;
        }
        if (context.canceled && _isRunning)
        {
            _speed /= _runModifier;
            _isRunning = false;
        }
    }

    public void MovePlayer(Transform inTransform)
    {
        Debug.Log("orig pos : " + transform.position);
        Debug.Log("in transform : " + inTransform.position);
        //transform.position = new Vector3(inTransform.position.x, inTransform.position.y, inTransform.position.z);
        //_controller.transform.position = inTransform.position;
        _controller.enabled = false;
        transform.position = inTransform.position;
        _controller.enabled = true;
        Debug.Log("player pos : " + transform.position);
    }
}
