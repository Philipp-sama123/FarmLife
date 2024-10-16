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
        [HideInInspector] public WeaponModelInstantiationSlot rightHandWeaponSlot;
        [HideInInspector] public WeaponModelInstantiationSlot leftHandShieldSlot;
        [HideInInspector] public WeaponModelInstantiationSlot backSlot;
        // For each new Weapon Type add here a new Slot:

        [Header("Weapon Models")]
        [HideInInspector] public GameObject rightHandWeaponModel;

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
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandWeaponSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandShieldSlot)
                {
                    leftHandShieldSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.BackSlot)
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
        }

        //  RIGHT WEAPON
        public void SwitchRightWeapon(int direction = 1)
        {
            // Add direction to index to switch to the next/previous potential weapon
            player.playerInventoryManager.rightHandWeaponIndex += direction;
            
            // Wrap around the index if out of bounds (circular switching)
            if (player.playerInventoryManager.rightHandWeaponIndex >= player.playerInventoryManager.weaponsInRightHandSlots.Length)
            {
                // Wrap to the first slot
                player.playerInventoryManager.rightHandWeaponIndex = 0;
            }
            else if (player.playerInventoryManager.rightHandWeaponIndex < 0)
            {
                // Wrap to the last slot
                player.playerInventoryManager.rightHandWeaponIndex = player.playerInventoryManager.weaponsInRightHandSlots.Length - 1;
            }

            // Get the selected weapon based on the updated index
            WeaponItem selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];

            // If there's no weapon in the slot, set to unarmed by default
            if (selectedWeapon == null)
            {
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
            }

            // Update current right-hand weapon
            player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
            
            // Play weapon swap animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);
           
            // Load the right weapon model
            LoadRightWeapon();

            // Log for debugging (optional)
            Debug.Log("Switched to weapon index: " + player.playerInventoryManager.rightHandWeaponIndex);
        }

        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                //  REMOVE THE OLD WEAPON
                rightHandWeaponSlot.UnloadWeapon();

                //  BRING IN THE NEW WEAPON
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);

                // Animator Controller is always depending on the Right Weapon (!) its the Main Weapon (!)
                player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

                player.isUsingRightHand = true;
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