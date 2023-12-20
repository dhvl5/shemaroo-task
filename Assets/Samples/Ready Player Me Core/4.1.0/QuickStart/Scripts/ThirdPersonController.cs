using Fusion;
using UnityEngine;

namespace ReadyPlayerMe.Samples
{
    [RequireComponent(typeof(ThirdPersonMovement),typeof(PlayerInput))]
    public class ThirdPersonController : MonoBehaviour
    {
        private const float FALL_TIMEOUT = 0.15f;
            
        private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
        private static readonly int JumpHash = Animator.StringToHash("JumpTrigger");
        private static readonly int FreeFallHash = Animator.StringToHash("FreeFall");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        
        private Transform playerCamera;
        private Animator animator;
        private Vector2 inputVector;
        private Vector3 moveVector;
        private GameObject avatar;
        private ThirdPersonMovement thirdPersonMovement;
        private PlayerInput playerInput;
        
        private float fallTimeoutDelta;
        
        [SerializeField][Tooltip("Useful to toggle input detection in editor")]
        private bool inputEnabled = true;
        private bool isInitialized;

        private void Init()
        {
            thirdPersonMovement = GetComponent<ThirdPersonMovement>();
            playerInput = GetComponent<PlayerInput>();
            playerInput.OnJumpPress += OnJump;
            isInitialized = true;
        }

        public void Setup(GameObject target, RuntimeAnimatorController runtimeAnimatorController)
        {
            if (!isInitialized)
            {
                Init();
            }
            
            avatar = target;
            animator = avatar.GetComponent<Animator>();
            thirdPersonMovement.Setup(avatar);
            animator = avatar.GetComponent<Animator>();
            animator.runtimeAnimatorController = runtimeAnimatorController;
            animator.applyRootMotion = false;
            
        }
        
        public void Update()
        {
            if (avatar == null)
            {
                return;
            }

            if (inputEnabled)
            {
                playerInput.CheckInput();
                //var xAxisInput = playerInput.AxisHorizontal;
                //var yAxisInput = playerInput.AxisVertical;
                //thirdPersonMovement.Move(xAxisInput, yAxisInput);
                //thirdPersonMovement.SetIsRunning(playerInput.IsHoldingLeftShift);
            }

            UpdateAnimator();
        }

        //public PlayerInputStruct GetPlayerInput()
        //{
        //    PlayerInputStruct playerInputStruct = new PlayerInputStruct();

        //    playerInputStruct.horizontalAxis = playerInput.AxisHorizontal;
        //    playerInputStruct.verticalAxis = playerInput.AxisVertical;
        //    playerInputStruct.IsHoldingLeftShift = playerInput.IsHoldingLeftShift;

        //    return playerInputStruct;
        //}

        private void UpdateAnimator()
        {
            var isGrounded = thirdPersonMovement.IsGrounded();
            animator.SetFloat(MoveSpeedHash, thirdPersonMovement.CurrentMoveSpeed);
            animator.SetBool(IsGroundedHash, isGrounded);

            if (isGrounded)
            {
                fallTimeoutDelta = FALL_TIMEOUT;
                animator.SetBool(FreeFallHash, false);
            }
            else
            {
                if (fallTimeoutDelta >= 0.0f)
                {
                    fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    animator.SetBool(FreeFallHash, true);
                }
            }
        }

        private void OnJump()
        {
            if (thirdPersonMovement.TryJump())
            {
                animator.SetTrigger(JumpHash);
            }
        }
    }
}
