using System.Collections;
using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController
{
    private readonly Player _player;
    private readonly AnimationController<PlayerAnimationParameter> _animationController;
    private readonly AudioController<PlayerSound> _audioController;
    
    private const float MoveSpeed = 5f;
    private const float JumpForce = 6.5f;
    
    private GameObject _groundCheck;
    private float _groundCheckRadius;
    private LayerMask _groundLayer;
    private Camera _camera;
    
    private const float WallCheckDistance = 0.1f;
    
    private Rigidbody2D _rb;
    private PlayerInput _playerInput;
    private Vector2 _moveInput;

    private bool _isGrounded;
    private bool _attackInProgress;
    
    private Vector3 _baseScale;
    
    
    private const float AttackDuration = 0.3f;
    
    public PlayerController(Player player, AnimationController<PlayerAnimationParameter> animationController,
        AudioController<PlayerSound> audioController)
    {
        _player = player;
        _animationController = animationController;
        _audioController = audioController;
    }

    public void Awake()
    {
        _groundCheck = GameObject.Find("GroundCheck");
        _groundCheckRadius = 0.2f;
        _groundLayer = LayerMask.GetMask("Ground");
        
        _camera = GameObject.Find("MainCamera").GetComponent<Camera>();
        
        _rb = _player.GetPlayerObject().GetComponent<Rigidbody2D>();
        _playerInput = _player.GetComponent<PlayerInput>();
        
        _baseScale = _player.GetPlayerObject().transform.localScale;
    }
    
    public void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, _groundCheckRadius, _groundLayer);
        _animationController.SetBool(PlayerAnimationParameter.IsGrounded, _isGrounded);
    }

    public void OnEnable()
    {
        _playerInput.enabled = true;
        
        _playerInput.actions["Movement"].performed += OnMovePerformed;
        _playerInput.actions["Movement"].canceled += OnMoveCanceled;
        _playerInput.actions["Jump"].performed += OnJumpPerformed;
        _playerInput.actions["Attack"].performed += OnAttackPerformed;
    }

    public void OnDisable()
    {
        _playerInput.actions["Movement"].performed -= OnMovePerformed;
        _playerInput.actions["Movement"].canceled -= OnMoveCanceled;
        _playerInput.actions["Jump"].performed -= OnJumpPerformed;
        _playerInput.actions["Attack"].performed -= OnAttackPerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _animationController.SetBool(PlayerAnimationParameter.IsRunning, true);
        
        _moveInput = context.ReadValue<Vector2>();
        
        FlipSprite();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _animationController.SetBool(PlayerAnimationParameter.IsRunning, false);
        _moveInput = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (!_isGrounded) return;
        _animationController.SetTrigger(PlayerAnimationParameter.Jump);
        _audioController.PlaySound(PlayerSound.Jump);

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, JumpForce);
    }
    
    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (_attackInProgress) return;
        
        var touching = new Collider2D[10];
        _player.GetAttackHitboxCollider().GetContacts(touching);
        foreach (var collider in touching)
        {
            if (collider == null) continue;
            var enemy = collider.GetComponentInParent<Enemy>();
           
            if (enemy == null) continue;
            _player.TryAttack(enemy);
        }

        _player.StartCoroutine(AttackRoutine());
        _animationController.SetTrigger(PlayerAnimationParameter.Attack);
        _audioController.PlaySound(PlayerSound.Attack);
    }
    
    private IEnumerator AttackRoutine()
    {
        _attackInProgress = true;
        _animationController.SetBool(PlayerAnimationParameter.IsAttacking, true);
        yield return new WaitForSeconds(AttackDuration);
        _attackInProgress = false;
        _animationController.SetBool(PlayerAnimationParameter.IsAttacking, false);
    }
    
    private bool IsWallBlocking(float horizontalInput)
    {
        if (horizontalInput == 0) return false;

        var playerCollider = _player.Collider();
        var bounds = playerCollider.bounds;
        
        Vector2 rayOrigin;
        Vector2 direction;

        var rayHeight = bounds.min.y;
        
        if (horizontalInput > 0)
        {
            rayOrigin = new Vector2(bounds.max.x, rayHeight);
            direction = Vector2.right;
        }
        else
        {
            rayOrigin = new Vector2(bounds.min.x, rayHeight);
            direction = Vector2.left;
        }
        
        var hit = Physics2D.Raycast(rayOrigin, direction, WallCheckDistance, _groundLayer);
        return hit.collider is not null;
    }
    
    public void FixedUpdate()
    {
        _camera.transform.position = new Vector3(_player.GetPlayerObject().transform.position.x,
            _camera.transform.position.y, _camera.transform.position.z);
        
        if (_player.CurrentKnockbackState() == KnockbackState.Active) return;
    
        var horizontalVelocity = _moveInput.x * MoveSpeed;
        if (IsWallBlocking(_moveInput.x)) horizontalVelocity = 0f;
        
        _rb.linearVelocity = new Vector2(horizontalVelocity, _rb.linearVelocity.y);
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
      
    }

    public void OnDeath()
    {
        _playerInput.enabled = false;
    }
    
    private void FlipSprite()
    {
        var playerObject = _player.GetPlayerObject();
        
        if (_moveInput.x > 0.01f) playerObject.transform.localScale = new Vector3(Mathf.Abs(_baseScale.x), _baseScale.y, _baseScale.z);
        else if (_moveInput.x < -0.01f) playerObject.transform.localScale = new Vector3(-Mathf.Abs(_baseScale.x), _baseScale.y, _baseScale.z);
    }
}