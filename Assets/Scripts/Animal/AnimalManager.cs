using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatgames
{
    public class AnimalManager : MonoBehaviour
    {
        [Header("Status")]
        public bool isDead = false;

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public AnimalAnimatorManager animalAnimatorManager;
        [HideInInspector] public AnimalLocomotionManager animalLocomotionManager;
        [HideInInspector] public AnimalStatsManager animalStatsManager;
        [HideInInspector] public AnimalAI animalAI;


        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool isJumping;

        public bool isMoving;
        public bool isSprinting = false;


        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            animalLocomotionManager = GetComponent<AnimalLocomotionManager>();
            animalAnimatorManager = GetComponent<AnimalAnimatorManager>();
            animalStatsManager = GetComponent<AnimalStatsManager>();
            animalAI = GetComponent<AnimalAI>();
        }
        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }
        protected virtual void FixedUpdate()
        {
            animalAI.ProcessStateMachine();
        }
        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", animalLocomotionManager.isGrounded);
            animator.SetBool("IsMoving", isMoving);
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