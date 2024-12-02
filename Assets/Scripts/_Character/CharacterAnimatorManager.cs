using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatGames
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;
        [Header("Flags")]
        public bool applyRootMotion = false;

        [Header("Damage Animations")]
        public string lastDamageAnimationPlayed;

        [SerializeField] public string forward_Medium_Damage = "Hit_Forward_Medium_01";
        [SerializeField] public string backward_Medium_Damage = "Hit_Backward_Medium_01";
        [SerializeField] public string left_Medium_Damage = "Hit_Left_Medium_01";
        [SerializeField] public string right_Medium_Damage = "Hit_Right_Medium_01";

        [SerializeField] public string forward_Ping_Damage = "Hit_Forward_Ping_01";
        [SerializeField] public string backward_Ping_Damage = "Hit_Backward_Ping_01";
        [SerializeField] public string left_Ping_Damage = "Hit_Left_Ping_01";
        [SerializeField] public string right_Ping_Damage = "Hit_Right_Ping_01";


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
        }
        protected virtual void Start()
        {
        }
        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new List<string>();

            foreach (var item in animationList)
            {
                finalList.Add(item);
            }

            //  CHECK IF WE HAVE ALREADY PLAYED THIS DAMAGE ANIMATION SO IT DOESNT REPEAT
            finalList.Remove(lastDamageAnimationPlayed);

            //  CHECK THE LIST FOR NULL ENTRIES, AND REMOVE THEM
            for (int i = finalList.Count - 1; i > -1; i--)
            {
                if (finalList[i] == null)
                {
                    finalList.RemoveAt(i);
                }
            }

            int randomValue = Random.Range(0, finalList.Count);

            return finalList[randomValue];
        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            float snappedHorizontal;
            float snappedVertical;
            #region Snapping
            //This if chain will round the horizontal movement to -1, -0.5, 0, 0.5 or 1

            if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
            {
                snappedHorizontal = 0.5f;
            }
            else if (horizontalMovement > 0.5f && horizontalMovement <= 1)
            {
                snappedHorizontal = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
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

            character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
        }
        public virtual void PlayTargetActionAnimation(
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            character.isPerformingAction = isPerformingAction;
            character.characterLocomotionManager.canRotate = canRotate;
            character.characterLocomotionManager.canMove = canMove;
        }
        public virtual void PlayTargetAttackActionAnimation(
            EquippableItem equippable,
            AttackType attackType,
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            //ToDo's: 
            // Keep track of last performed for Combos
            // Keep track of current Attack Type
            // update AnimationSet to current Weapon Animations
            // Decide if the Attack can be parried 
            // Tell the Network --> in Attacking FLAG 
            character.characterCombatManager.currentAttackType = attackType;
            character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
            UpdateAnimatorController(equippable.weaponAnimator);
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.1f);
            character.isPerformingAction = isPerformingAction;
            character.characterLocomotionManager.canRotate = canRotate;
            character.characterLocomotionManager.canMove = canMove;
        }
        public void UpdateAnimatorController(AnimatorOverrideController weaponController)
        {
            if (weaponController != null)
                character.animator.runtimeAnimatorController = weaponController;
        }
    }
}