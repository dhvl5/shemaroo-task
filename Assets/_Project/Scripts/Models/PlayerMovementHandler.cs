using Fusion;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [HideInInspector] public float CurrentMoveSpeed;

    public float walkSpeed = 3f;
    public float runSpeed = 8f;
    public float gravity = -18f;
    public float jumpHeight = 3f;

    private float _verticalVelocity;
    private float _turnSmoothVelocity;

    private Transform _cameraTransform;
    private NetworkRunner _networkRunner;
    private CharacterController _characterController;

    private bool _jumpTrigger = false;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
        _networkRunner = GameObject.Find("NetworkManager").GetComponent<NetworkRunner>();
        _characterController = GetComponent<CharacterController>();
    }

    public void Move(PlayerInputStruct playerInputStruct)
    {
        if (PlayerBehaviour.Local)
        {
            if (_cameraTransform != null)
            {
                var moveDirection = _cameraTransform.right * playerInputStruct.horizontalAxis + _cameraTransform.forward * playerInputStruct.verticalAxis;
                var moveSpeed = playerInputStruct.isHoldingLeftShift ? runSpeed : walkSpeed;

                _jumpTrigger = playerInputStruct.isJumpPressed;
                JumpAndGravity();

                _characterController.Move(moveDirection.normalized * (moveSpeed * _networkRunner.DeltaTime) + new Vector3(0.0f, _verticalVelocity * _networkRunner.DeltaTime, 0.0f));

                var moveMagnitude = moveDirection.magnitude;
                CurrentMoveSpeed = playerInputStruct.isHoldingLeftShift ? runSpeed * moveMagnitude : walkSpeed * moveMagnitude;

                if (moveDirection.magnitude > 0)
                    RotateAvatarTowardsMoveDirection(moveDirection);
            }
        }
    }

    private void RotateAvatarTowardsMoveDirection(Vector3 moveDirection)
    {
        if (PlayerBehaviour.Local)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + transform.rotation.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, .005f);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    private void JumpAndGravity()
    {
        if (_characterController.isGrounded && _verticalVelocity < 0)
            _verticalVelocity = -2f;

        if (_jumpTrigger && _characterController.isGrounded)
        {
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _jumpTrigger = false;
        }

        _verticalVelocity += gravity * _networkRunner.DeltaTime;
    }

    public bool IsGrounded()
    {
        if (_verticalVelocity > 0)
            return false;

        return true;
    }
}