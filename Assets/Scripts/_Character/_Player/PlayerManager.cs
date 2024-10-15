using UnityEngine;
using UnityEngine.SceneManagement;

namespace KrazyKatGames
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
        [HideInInspector] public PlayerSoundFXManager playerSoundFXManager;
        [HideInInspector] public PlayerEffectsManager playerEffectsManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerBodyManager playerBodyManager;

        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;
        public string characterName = "Character Name";

        protected override void Awake()
        {
            base.Awake();

            playerInteractionManager = GetComponent<PlayerInteractionManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerSoundFXManager = GetComponent<PlayerSoundFXManager>();
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

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currentCharacterData.characterName = characterName; // ToDo (!)
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            currentCharacterData.vitality = playerStatsManager.vitality;
            currentCharacterData.endurance = playerStatsManager.endurance;

            currentCharacterData.currentHealth = playerStatsManager.currentHealth;
            currentCharacterData.currentStamina = playerStatsManager.currentStamina;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            characterName = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;

            playerStatsManager.vitality = currentCharacterData.vitality;
            playerStatsManager.endurance = currentCharacterData.endurance;

            playerStatsManager.maxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(currentCharacterData.endurance);
            playerStatsManager.maxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(currentCharacterData.vitality);

            playerStatsManager.currentHealth = currentCharacterData.currentHealth;
            playerStatsManager.currentStamina = currentCharacterData.currentStamina;

            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerStatsManager.maxStamina);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(playerStatsManager.maxHealth);
        }
    }
}