using System.Collections.Generic;
using HoaxGames;
using UnityEngine;

namespace KrazyKatGames
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Status")]
        public bool isDead = false;

        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterStatsManager characterStatsManager;
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public FootIK footIK;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool isJumping;
        public bool isMoving;
        public bool isLockedOn;
        public bool isSprinting = false;
        public bool isInvulnerable = false;
        public bool isUsingLeftHand = false;
        public bool isUsingRightHand = false;
        public bool isBlocking;
        public bool isAttacking;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            footIK = GetComponent<FootIK>();
        }
        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }
        protected virtual void FixedUpdate()
        {
        }
        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", characterLocomotionManager.isGrounded);
        }

        protected virtual void LateUpdate()
        {
        }

        public virtual void ReviveCharacter()
        {
        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

            List<Collider> ignoreColliders = new List<Collider>();

            foreach (var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }
            ignoreColliders.Add(characterControllerCollider);


            foreach (var collider in ignoreColliders)
            {
                foreach (var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }
        protected virtual void OnEnable()
        {
        }
        protected virtual void OnDisable()
        {
        }
    }
}