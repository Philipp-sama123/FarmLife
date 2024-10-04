using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace KrazyKatgames
{
    public class PlayerManager : CharacterManager
    {
        [Header("Debug Menu")]
        [SerializeField] bool respawnCharacter = false;

        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Update()
        {
            base.Update();


            playerLocomotionManager.HandleAllMovement();
            playerStatsManager.RegenerateStamina();
            DebugMenu();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        public override void ReviveCharacter()
        {
            base.ReviveCharacter();
            playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
        }


        private void DebugMenu()
        {
            if (respawnCharacter)
            {
                respawnCharacter = false;
                ReviveCharacter();
            }
        }
        public void ExecuteAction()
        {
            playerAnimatorManager.PlayTargetActionAnimation("Chopping_01", true);
            // or:   playerAnimatorManager.PlayTargetActionAnimation("Working_01", true);
            // or: ... 
        }
    }
}