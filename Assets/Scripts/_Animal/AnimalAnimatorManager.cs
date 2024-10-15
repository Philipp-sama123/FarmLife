using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatGames
{
    public class AnimalAnimatorManager : MonoBehaviour
    {
        AnimalManager animal;

        int vertical;
        int horizontal;
        [Header("Flags")]
        public bool applyRootMotion = false;

        [Header("Damage Animations")]
        public string lastDamageAnimationPlayed;

        public string hit_Forward = "Hit_Forward";
        public string hit_Backward = "Hit_Backward";
        public string hit = "Hit";


        protected virtual void Awake()
        {
            animal = GetComponent<AnimalManager>();

            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
        }
        protected virtual void Start()
        {
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            float snappedHorizontal;
            float snappedVertical;
            #region Snapping
            //This if chain will round the horizontal movement to -1, -0.5, 0, 0.5 or 1

            if (horizontalMovement > 0.25f && horizontalMovement <= 0.5f)
            {
                snappedHorizontal = 0.5f;
            }
            else if (horizontalMovement > 0.5f && horizontalMovement <= 1)
            {
                snappedHorizontal = 1;
            }
            else if (horizontalMovement < -0.25f && horizontalMovement >= -0.5f)
            {
                snappedHorizontal = -0.5f;
            }
            else if (horizontalMovement < -0.5f && horizontalMovement >= -1)
            {
                snappedHorizontal = -1;
            }
            else
            {
                snappedHorizontal = 0;
            }
            //This if chain will round the vertical movement to -1, -0.5, 0, 0.5 or 1

            if (verticalMovement > 0 && verticalMovement <= 0.5f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalMovement > 0.5f && verticalMovement <= 1)
            {
                snappedVertical = 1;
            }
            else if (verticalMovement < 0 && verticalMovement >= -0.5f)
            {
                snappedVertical = -0.5f;
            }
            else if (verticalMovement < -0.5f && verticalMovement >= -1)
            {
                snappedVertical = -1;
            }
            else
            {
                snappedVertical = 0;
            }

            if (isSprinting)
            {
                snappedVertical = 2;
            }
            #endregion
            
            animal.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            animal.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
        }
        public virtual void PlayTargetActionAnimation(
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            animal.animalAnimatorManager.applyRootMotion = applyRootMotion;
            animal.animator.CrossFade(targetAnimation, 0.2f);
            animal.isPerformingAction = isPerformingAction;
        }

        private void OnAnimatorMove()
        {
            if (!animal.animalLocomotionManager.isGrounded)
                return;
            // if (!applyRootMotion)
            //     return;

            Vector3 velocity = animal.animator.deltaPosition;

            animal.characterController.Move(velocity);
            animal.transform.rotation *= animal.animator.deltaRotation;
        }
    }
}