using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatGames
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        [Header("Weapon Model Instantiation Slots")]
        [HideInInspector] public EquippableModelInstantiationSlot rightHandSlot;
        [HideInInspector] public EquippableModelInstantiationSlot leftHandSlot;
        [HideInInspector] public EquippableModelInstantiationSlot backSlot;
        // For each new Weapon Type add here a new Slot:

        [Header("Weapon Models")]
        [HideInInspector] public GameObject rightHandWeaponModel;
        [HideInInspector] public GameObject leftHandWeaponModel;

        [Header("Weapon Managers")]
        [SerializeField] WeaponManager rightWeaponManager;
        [SerializeField] WeaponManager leftWeaponManager;

        [Header("DEBUGGING")]
        [SerializeField]
        private bool equipNewItem = false;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            InitializeWeaponSlots();

            CloseDamageCollider();
        }

        protected override void Start()
        {
            base.Start();
            LoadWeaponsOnBothHands();
        }

        private void Update()
        {
            if (equipNewItem)
            {
                equipNewItem = false;
                DebugEquipNewItems();
            }
        }
        private void InitializeWeaponSlots()
        {
            EquippableModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<EquippableModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.equippableSlot == EquippableModelSlot.RightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.equippableSlot == EquippableModelSlot.LeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.equippableSlot == EquippableModelSlot.BackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }

        private void DebugEquipNewItems()
        {
            LoadWeaponsOnBothHands();
        }

        public void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon(); // Main Weapon 
            LoadLeftWeapon();
        }

        //  RIGHT WEAPON
        public void SwitchEquippableItem(int direction = 1)
        {
            // Add direction to index to switch to the next/previous potential weapon
            player.playerInventoryManager.rightHandEquipmentIndex += direction;

            // Wrap around the index if out of bounds (circular switching)
            if (player.playerInventoryManager.rightHandEquipmentIndex >= player.playerInventoryManager.equipmentsInRightHandSlots.Length)
            {
                // Wrap to the first slot
                player.playerInventoryManager.rightHandEquipmentIndex = 0;
            }
            else if (player.playerInventoryManager.rightHandEquipmentIndex < 0)
            {
                // Wrap to the last slot
                player.playerInventoryManager.rightHandEquipmentIndex = player.playerInventoryManager.equipmentsInRightHandSlots.Length - 1;
            }

            // Get the selected weapon based on the updated index
            EquippableItem selectedEquippable =
                player.playerInventoryManager.equipmentsInRightHandSlots[player.playerInventoryManager.rightHandEquipmentIndex];

            // If there's no weapon in the slot, set to unarmed by default
            if (selectedEquippable == null)
            {
                selectedEquippable = WorldItemDatabase.Instance.unarmedEquippable;
            }

            // Update current right-hand weapon
            player.playerInventoryManager.currentRightHandEquipment = selectedEquippable;

            // Play weapon swap animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

            // Load the right weapon model
            LoadRightWeapon();

            // Log for debugging (optional)
            Debug.Log("Switched to weapon index: " + player.playerInventoryManager.rightHandEquipmentIndex);
        }

        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandEquipment != null)
            {
                //  REMOVE THE OLD WEAPON
                rightHandSlot.UnloadWeapon();

                //  BRING IN THE NEW WEAPON
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandEquipment.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandEquipment);

                // Animator Controller is always depending on the Right Weapon (!) its the Main Weapon (!)
                player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandEquipment.weaponAnimator);

                player.isUsingRightHand = true;
            }
        }
        public void LoadLeftWeapon()
        {
            if (player.playerInventoryManager.currentRightHandEquipment != null)
            {
               leftHandSlot.UnloadWeapon();

                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandEquipment.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandEquipment);

                // Animator Controller is always depending on the Right Weapon (!) its the Main Weapon (!)
              //  player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandEquipment.weaponAnimator);

            }
        }

        #region Animation Events
        /**
         * Animation Events
         */
        public void OpenDamageCollider()
        {
            if (player.isUsingRightHand)
            {
                rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
                // Play whoosh sfx (!)

                // player.characterSoundFXManager.PlaySoundFX(
                //     WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentRightHandWeapon.whooshes));
            }
            else if (player.isUsingLeftHand)
            {
                leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
                // Play whoosh sfx (!)

                // player.characterSoundFXManager.PlaySoundFX(
                //     WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentLeftHandWeapon.whooshes));
            }
        }
        public void CloseDamageCollider()
        {
            if (player.isUsingRightHand)
            {
                rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
            else if (player.isUsingLeftHand)
            {
                leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
        }
        #endregion
    }
}