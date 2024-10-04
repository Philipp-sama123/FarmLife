using System;
using System.Collections;
using System.Collections.Generic;
using HoaxGames;
using UnityEngine;

namespace KrazyKatgames
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Status")]
        public bool isDead = false;

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool isJumping;
        public bool isMoving;
        public bool isLockedOn;
        public bool isSprinting =false;
        public FootIK footIK;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();

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