using UnityEngine;

namespace KrazyKatgames
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Settings")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float sprintingSpeed = 7.5f;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] int sprintingStaminaCost = 2;

        [Header("Dodge")]
        private Vector3 rollDirection;
        [SerializeField] int dodgeStaminaCost = 10;
        [SerializeField] float jumpStaminaCost = 10f;
        [SerializeField] float jumpHeight = 3.5f;
        [SerializeField] float inAirMovementSpeedMultiplier = .35f;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }
        protected override void Update()
        {
            base.Update();

            player.verticalMovement = verticalMovement;
            player.horizontalMovement = horizontalMovement;
            player.moveAmount = moveAmount;

            // update for Network Player - because for the usual player it is handled in PlayerInputManager (!)
            verticalMovement = player.verticalMovement;
            horizontalMovement = player.horizontalMovement;
            moveAmount = player.moveAmount;

            if (!player.isLockedOn || player.isSprinting)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(
                    0,
                    moveAmount,
                    player.isSprinting
                );
            }
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(
                    horizontalMovement,
                    verticalMovement,
                    player.isSprinting
                );
            }
        }
        public void HandleAllMovement()
        {
            if (player.playerLocomotionManager.canMove || player.playerLocomotionManager.canRotate)
                CalculateMovementDirection();

            HandleMovement();
            HandleRotation();
        }
        private void HandleMovement()
        {
            if (player.playerLocomotionManager.isGrounded && !player.isJumping)
                if (player.isLockedOn)
                    HandleStrafeMovement();
                else
                    HandleGroundedMovement();
            else
                HandleInAirMovement();
        }
        private void HandleInAirMovement()
        {
            if (player.isSprinting)
            {
                // ToDo: think of rider suggestion (whats the benefit) 
                player.characterController.Move(moveDirection * sprintingSpeed * inAirMovementSpeedMultiplier * Time.deltaTime);
            }
            else
            {
                if (moveAmount > 0.5f)
                {
                    player.characterController.Move(moveDirection * runningSpeed * inAirMovementSpeedMultiplier * Time.deltaTime);
                }
                else
                {
                    player.characterController.Move(moveDirection * walkingSpeed * inAirMovementSpeedMultiplier * Time.deltaTime);
                }
            }
        }
        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.vertical_Input;
            horizontalMovement = PlayerInputManager.instance.horizontal_Input;
            moveAmount = PlayerInputManager.instance.moveAmount;
            //  CLAMP THE MOVEMENTS
        }
        private void HandleGroundedMovement()
        {
            if (player.isDead)
                return;

            if (!player.playerLocomotionManager.canMove)
                return;

            if (player.isSprinting)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }
        }
        private void HandleStrafeMovement()
        {
            if (player.playerLocomotionManager.isGrounded && !player.isJumping)
            {
                // Get the camera's forward and right directions (ignoring the vertical component).
                Vector3 cameraForward = PlayerCamera.instance.cameraObject.transform.forward;
                Vector3 cameraRight = PlayerCamera.instance.cameraObject.transform.right;

                // Zero out the Y components to prevent the player from moving up or down based on camera tilt.
                cameraForward.y = 0;
                cameraRight.y = 0;

                // Normalize the vectors to maintain consistent movement speed.
                cameraForward.Normalize();
                cameraRight.Normalize();

                // Combine the forward/backward and left/right movement using the input and camera directions.
                Vector3 movementDirection = cameraForward * verticalMovement + cameraRight * horizontalMovement;

                // Normalize the movement direction to ensure consistent speed when moving diagonally.
                movementDirection.Normalize();

                // Apply the appropriate speed based on whether the player is sprinting or walking.
                float speed = player.isSprinting ? sprintingSpeed : (moveAmount > 0.5f ? runningSpeed : walkingSpeed);

                // Move the character.
                player.characterController.Move(movementDirection * speed * Time.deltaTime);
            }
        }

        private void CalculateMovementDirection()
        {
            GetMovementValues();
            //  OUR MOVE DIRECTION IS BASED ON OUR CAMERAS FACING PERSPECTIVE & OUR MOVEMENT INPUTS
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;
        }
        // ToDo: this can be cleaned up (!) more function - with proper names HandleStrafeMovement(),HandleStrafeSprinting(),HandleFreeMovement()
        private void HandleRotation()
        {
            if (player.isDead || !player.playerLocomotionManager.canRotate)
                return;

            if (player.isLockedOn)
            {
                // Rotate the player to always face the camera's forward direction when locked on.
                Vector3 cameraForward = PlayerCamera.instance.cameraObject.transform.forward;
                cameraForward.y = 0; // Ignore vertical tilt to keep the player on the horizontal plane
                cameraForward.Normalize(); // Normalize to ensure a valid direction

                if (cameraForward != Vector3.zero)
                {
                    // Create a rotation based on the camera's forward direction
                    Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

                    // Smoothly rotate the player towards the target direction
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                // Regular free movement rotation (already working fine).
                Vector3 movementDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement +
                                            PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                movementDirection.y = 0; // Lock rotation to the horizontal plane

                if (movementDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }


        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                player.isSprinting = false;
            }

            if (player.playerStatsManager.currentStamina <= 0)
            {
                player.isSprinting = false;
                return;
            }

            //  IF WE ARE MOVING, SPRINTING IS TRUE
            if (moveAmount >= 0.5)
            {
                player.isSprinting = true;
            }
            //  IF WE ARE STATIONARY/MOVING SLOWLY SPRINTING IS FALSE
            else
            {
                player.isSprinting = false;
            }

            if (player.isSprinting)
            {
                player.playerStatsManager.currentStamina -= sprintingStaminaCost * Time.deltaTime;
            }
        }
        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
                return;

            if (player.playerStatsManager.currentStamina <= 0)
                return;

            //  IF WE ARE MOVING WHEN WE ATTEMPT TO DODGE, WE PERFORM A ROLL
            if (PlayerInputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.vertical_Input;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontal_Input;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true, true, false);
                player.playerLocomotionManager.isRolling = true; // ToDo: maybe just isRolling (!)
            }
            //  IF WE ARE STATIONARY, WE PERFORM A BACKSTEP
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Backward_01", true, true, true, false);
            }

            player.playerStatsManager.currentStamina -= dodgeStaminaCost;
        }
        public void AttemptToPerformJump()
        {
            if (player.isPerformingAction) // ToDo: AttackJump
                return;
            if (player.playerStatsManager.currentStamina <= 0)
                return;
            if (player.isJumping) // ToDo: DoubleJump
                return;
            if (!player.playerLocomotionManager.isGrounded)
                return;

            player.playerStatsManager.currentStamina -= jumpStaminaCost;

            player.playerAnimatorManager.PlayTargetActionAnimation("Jump_Start", false, true, true, true);
            player.isJumping = true;
        }
        /***
         * Animation Event
         */
        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
}