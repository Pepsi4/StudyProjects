using UnityEngine;
using System.Collections;
using System;
using StarterAssets;
using UnityEngine.InputSystem;
using System.Linq;

public class AstronautPlayerMove : MonoBehaviour
{
    [SerializeField] float _jumpForce;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _disableGCTime;
    [SerializeField] private float _sphereRadius;
    [SerializeField] private float _sphereOffset;

    [SerializeField] private float _speed = 600.0f;
    [SerializeField] private float _turnSpeed = 400.0f;
    [SerializeField] private float _gravity = 20.0f;

    private Rigidbody _rbody;
    private Animator _anim;
    private AstronautPlayerInput _inputActions;

    private bool _groundCheckEnabled = true;
    private WaitForSeconds _wait;
    private bool _isGrounded = true;

    private Vector2 _moveInput;
    private Vector3 _moveDirection = Vector3.zero;
    private float _yVelocity;

    private void Awake()
    {
        _inputActions = new AstronautPlayerInput();
        _rbody = GetComponent<Rigidbody>();
        _wait = new WaitForSeconds(_disableGCTime);
        _anim = gameObject.GetComponent<Animator>();

        _inputActions.Player.Jump.performed += JumpPerformed;
    }

    private void FixedUpdate()
    {
        _moveInput = _inputActions.Player.Move.ReadValue<Vector2>();
    }

    void Update()
    {
        GroundedCheck();


        if (_isGrounded)
        {
            Move();
            Rotate();
        }


        ApplyMovement();
        ApplyGravity();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            _yVelocity = _jumpForce;
            _anim.SetTrigger("Jump");
            StartCoroutine(EnableGroundCheckAfterJump());
        }
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _sphereOffset,
            transform.position.z);
        _isGrounded = Physics.CheckSphere(spherePosition, _sphereRadius, _groundMask,
            QueryTriggerInteraction.Ignore);

        _anim.SetBool("Grounded", _isGrounded);
    }

    private IEnumerator EnableGroundCheckAfterJump()
    {
        _groundCheckEnabled = false;
        yield return _wait;
        _groundCheckEnabled = true;
    }

    private void Move()
    {
        if (_inputActions.Player.Move.inProgress)
        {
            _anim.SetBool("Run", true);
        }
        else
        {
            _anim.SetBool("Run", false);
        }

        _moveDirection = transform.forward * _moveInput.y * _speed;
    }

    private void Rotate()
    {
        transform.Rotate(0, _moveInput.x * _turnSpeed * Time.deltaTime, 0);
    }

    private void ApplyMovement()
    {
        _rbody.velocity = new Vector3(_moveDirection.x, _yVelocity, _moveDirection.z);
    }

    private void ApplyGravity()
    {

        if (_isGrounded && _groundCheckEnabled)
        {
            _yVelocity = -_gravity * Time.deltaTime;
        }
        else
        {
            _yVelocity -= _gravity * Time.deltaTime;
        }
    }
}
