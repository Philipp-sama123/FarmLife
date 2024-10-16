using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatGames
{
    public class AnimalManager : CharacterManager
    {
        [HideInInspector] public AnimalAnimatorManager animalAnimatorManager;
        [HideInInspector] public AnimalLocomotionManager animalLocomotionManager;
        [HideInInspector] public AnimalStatsManager animalStatsManager;
        [HideInInspector] public AnimalAI animalAI;
        
        protected override void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(this);
            
            animalLocomotionManager = GetComponent<AnimalLocomotionManager>();
            animalAnimatorManager = GetComponent<AnimalAnimatorManager>();
            animalStatsManager = GetComponent<AnimalStatsManager>();
            animalAI = GetComponent<AnimalAI>();
        }
        protected override void Start()
        {
            IgnoreMyOwnColliders();
        }
        protected override void FixedUpdate()
        {
            animalAI.ProcessStateMachine();
        }
        protected override void Update()
        {
            animator.SetBool("IsGrounded", animalLocomotionManager.isGrounded);
            animator.SetBool("IsMoving", isMoving);
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();
        }

        protected override void OnEnable()
        {
        }
        protected override void OnDisable()
        {
        }
    }
}