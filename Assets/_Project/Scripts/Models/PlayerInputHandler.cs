using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private float _horizontalAxis = 0.0f;
    private float _verticalAxis = 0.0f;

    private float fallTimeoutDelta = 0.0f;

    private bool _isJumpPressed = false;
    private bool _isHoldingLeftShift = false;

    private Animator _animator;
    private GameObject _avatar;
    private PlayerMovementHandler _playerMovementHandler;

    private void OnEnable()
    {
        RPM_AvatarLoader.OnAvatarLoaded += OnAvatarLoaded;
    }

    private void Start()
    {
        _isJumpPressed = false;
        _isHoldingLeftShift = false;

        _playerMovementHandler = GetComponent<PlayerMovementHandler>();
    }

    private void OnDisable()
    {
        RPM_AvatarLoader.OnAvatarLoaded -= OnAvatarLoaded;
    }

    private void OnAvatarLoaded(GameObject avatar)
    {
        _avatar = avatar;

        _animator = _avatar.GetComponent<Animator>();
    }

    private void Update()
    {
        _horizontalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
            _isHoldingLeftShift = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            _isHoldingLeftShift = false;

        if (Input.GetButtonDown("Jump"))
        {
            _isJumpPressed = true;
            _animator.SetTrigger(Animator.StringToHash("JumpTrigger"));
        }

        if (_animator != null)
            UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        var isGrounded = _playerMovementHandler.IsGrounded();

        _animator.SetFloat(Animator.StringToHash("MoveSpeed"), _playerMovementHandler.CurrentMoveSpeed);
        _animator.SetBool(Animator.StringToHash("IsGrounded"), isGrounded);

        if (isGrounded)
        {
            fallTimeoutDelta = 0.15f;
            _animator.SetBool(Animator.StringToHash("FreeFall"), false);
        }
        else
        {
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _animator.SetBool(Animator.StringToHash("FreeFall"), true);
            }
        }
    }

    public PlayerInputStruct GetPlayerInput()
    {
        PlayerInputStruct playerInputStruct = new PlayerInputStruct();

        playerInputStruct.horizontalAxis = _horizontalAxis;
        playerInputStruct.verticalAxis = _verticalAxis;

        playerInputStruct.isHoldingLeftShift = _isHoldingLeftShift;
        playerInputStruct.isJumpPressed = _isJumpPressed;

        _isJumpPressed = false;

        return playerInputStruct;
    }
}