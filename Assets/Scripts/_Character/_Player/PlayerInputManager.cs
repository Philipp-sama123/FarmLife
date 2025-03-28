using UnityEngine;
using UnityEngine.SceneManagement;

namespace KrazyKatGames
{
    public class PlayerInputManager : MonoBehaviour
    {
        //  INPUT CONTROLS
        private PlayerControls playerControls;

        //  SINGLETON
        public static PlayerInputManager instance;

        //  LOCAL PLAYER
        public PlayerManager player;

        [Header("Camera Movement Inputs")]
        [SerializeField] Vector2 camera_Input;
        public float cameraVertical_Input;
        public float cameraHorizontal_Input;

        [Header("LockOn Inputs")]
        [SerializeField] bool lockOn_Input;
        [SerializeField] bool lockOn_Left_Input;
        [SerializeField] bool lockOn_Right_Input;
        private Coroutine lockOnCoroutine;

        [Header("Movement Inputs")]
        [SerializeField] Vector2 movementInput;
        public float vertical_Input;
        public float horizontal_Input;
        public float moveAmount;

        [Header("Action Inputs")]
        [SerializeField] bool dodge_Input = false;
        [SerializeField] bool sprint_Input = false;
        [SerializeField] bool jump_Input = false;
        [SerializeField] bool interaction_Input = false;
        [SerializeField] bool switch_Right_Weapon_Input = false;
        [SerializeField] bool switch_Left_Weapon_Input = false;

        [Header("Bumper Inputs")]
        [SerializeField] bool RB_Input = false;
        [SerializeField] bool LB_Input = false;

        [Header("Trigger Inputs")]
        [SerializeField] bool RT_Input = false;
        [SerializeField] bool Hold_RT_Input = false;

        [Header("Camera Zoom Input")]
        [SerializeField] float zoom_Input;
        public float ZoomInput => zoom_Input;

        [Header("Menu")]
        [SerializeField] bool openMenuInput = false;

        [Header("Debug")]
        [SerializeField] bool halfMovementInput = false;
        [SerializeField] bool debugStrafing = false;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                // Movement
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => camera_Input = i.ReadValue<Vector2>();
                // Actions
                playerControls.PlayerActions.Dodge.performed += i => dodge_Input = true;
                playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

                playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
                playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;

                // Bumpers
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;
                playerControls.PlayerActions.LB.performed += i => LB_Input = true;

                // Zoom - make sure you have added this binding in your PlayerControls
                playerControls.PlayerCamera.Zoom.performed += i => zoom_Input = i.ReadValue<float>();
                playerControls.PlayerCamera.Zoom.canceled += i => zoom_Input = 0f;

                // Menu
                playerControls.PlayerActions.OpenMenu.performed += i => openMenuInput = true;

                // Triggers
                playerControls.PlayerActions.RT.performed += i => RT_Input = true;

                // Interactions
                playerControls.PlayerActions.Interact.performed += i => interaction_Input = true;

                //  LOCK ON
                playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                playerControls.PlayerActions.LockOnLeft.performed += i => lockOn_Left_Input = true;
                playerControls.PlayerActions.LockOnRight.performed += i => lockOn_Right_Input = true;

                //  HOLDING THE INPUT, SETS THE BOOL TO TRUE
                playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
                playerControls.PlayerActions.Hold_RT.performed += i => Hold_RT_Input = true;
                //  RELEASING THE INPUT, SETS THE BOOL TO FALSE
                playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;
                playerControls.PlayerActions.Hold_RT.canceled += i => Hold_RT_Input = false;
            }

            playerControls.Enable();
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            // HandleLockOnInput();
            // HandleLockOnSwitchTargetInput();
            //
            HandlePlayerMovementInput();
            HandleCameraMovementInput();

            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleInteractionInput();
            //
            HandleRBInput();
            HandleLBInput();
            HandleRTInput();
            // HandleChargeRTInput();
            //
            HandleSwitchEquippableInput();
            HandleOpenCharacterMenuInput();
        }
        private void HandleRTInput()
        {
            if (RT_Input)
            {
                RT_Input = false;

                //  FUTURE NOTE: RETURN (DO NOTHING) IF MENU OR UI WINDOW IS OPEN

                //   player.ExecuteAction();
            }
        }

        //  MOVEMENT
        private void HandlePlayerMovementInput()
        {
            vertical_Input = movementInput.y;
            horizontal_Input = movementInput.x;

            //  RETURNS THE ABSOLUTE NUMBER, (Meaning number without the negative sign, so its always positive)
            moveAmount = Mathf.Clamp01(Mathf.Abs(vertical_Input) + Mathf.Abs(horizontal_Input));

            //  WE CLAMP THE VALUES, SO THEY ARE 0, 0.5 OR 1 (OPTIONAL)
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            if (halfMovementInput)
            {
                moveAmount /= 2;
                horizontal_Input /= 2;
                vertical_Input /= 2;
            }
            player.isLockedOn = debugStrafing;

            // WHY DO WE PASS 0 ON THE HORIZONTAL? BECAUSE WE ONLY WANT NON-STRAFING MOVEMENT
            // WE USE THE HORIZONTAL WHEN WE ARE STRAFING OR LOCKED ON

            if (player == null)
                return;

            if (moveAmount != 0)
                player.isMoving = true;
            //  IF WE ARE NOT LOCKED ON, ONLY USE THE MOVE AMOUNT

            if (!debugStrafing || player.isSprinting)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);
            }
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontal_Input, vertical_Input,
                    player.isSprinting);
            }
        }
        private void HandleCameraMovementInput()
        {
            cameraVertical_Input = camera_Input.y;
            cameraHorizontal_Input = camera_Input.x;
        }

        //  ACTION
        private void HandleDodgeInput()
        {
            //  FUTURE NOTE: RETURN (DO NOTHING) IF MENU OR UI WINDOW IS OPEN

            if (dodge_Input)
            {
                dodge_Input = false;
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }
        private void HandleSprintInput()
        {
            //  FUTURE NOTE: RETURN (DO NOTHING) IF MENU OR UI WINDOW IS OPEN

            if (sprint_Input)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.isSprinting = false;
            }
        }
        private void HandleJumpInput()
        {
            //  FUTURE NOTE: RETURN (DO NOTHING) IF MENU OR UI WINDOW IS OPEN

            if (jump_Input)
            {
                jump_Input = false;
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }
        private void HandleInteractionInput()
        {
            if (interaction_Input)
            {
                interaction_Input = false;

                player.playerInteractionManager.Interact();
            }
        }
        private void HandleRBInput()
        {
            if (RB_Input)
            {
                RB_Input = false;

                //  TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

                //  player.playerNetworkManager.SetCharacterActionHand(true);

                //  TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

                player.playerCombatManager.PerformEquipmentBasedAction(player.playerInventoryManager.currentRightHandEquipment.oh_RB_Action,
                    player.playerInventoryManager.currentRightHandEquipment);
            }
        }
        private void HandleLBInput()
        {
            if (LB_Input)
            {
                LB_Input = false;

                //  TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

                player.playerCombatManager.PerformEquipmentBasedAction(
                    player.playerInventoryManager.currentRightHandEquipment.oh_LB_Action,
                    player.playerInventoryManager.currentRightHandEquipment);
            }
        }
        private void HandleSwitchEquippableInput()
        {
            if (switch_Right_Weapon_Input)
            {
                switch_Right_Weapon_Input = false;
                player.playerEquipmentManager.SwitchEquippableItem(+1);
            }
            if (switch_Left_Weapon_Input)
            {
                switch_Left_Weapon_Input = false;
                player.playerEquipmentManager.SwitchEquippableItem(-1);
            }
        }
        private void HandleOpenCharacterMenuInput()
        {
            if (openMenuInput)
            {
                openMenuInput = false;

                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopupWindows();

                if (!PlayerUIManager.instance.mainMenuWindowIsOpen)
                    PlayerUIManager.instance.playerUIMainMenuManager.OpenMainMenu();
                else
                    PlayerUIManager.instance.playerUIMainMenuManager.CloseMainMenu();
            }
        }
    }
}