using System.Collections;
using MyNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace KrazyKatgames
{
    public class PlayerManager : CharacterManager
    {
        [Header("Debug Menu")]
        [SerializeField] bool respawnCharacter = false;

        [HideInInspector] public PlayerInteractionManager playerInteractionManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerEffectsManager playerEffectsManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerBodyManager playerBodyManager;

        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        protected override void Awake()
        {
            base.Awake();

            playerInteractionManager = GetComponent<PlayerInteractionManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerBodyManager = GetComponent<PlayerBodyManager>();
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
            // ToDo: 
            // Make Inventory (!) 
            // Make Equipment Manager (!) 

            playerAnimatorManager.PlayTargetActionAnimation("Working_01", true);
            // or:   playerAnimatorManager.PlayTargetActionAnimation("Working_01", true);
            // or: ... 
        }
    }
}