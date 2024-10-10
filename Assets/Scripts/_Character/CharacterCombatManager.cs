using UnityEngine;

namespace KrazyKatgames
{
    public class CharacterCombatManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Last Attack Animation Performed")]
        public string lastAttackAnimationPerformed;

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Attack Flags")]
        public bool canPerformRollingAttack = false;
        public bool canPerformBackstepAttack = false;
        public bool canBlock = true;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }


        #region Animation Events
        public void EnableIsInvulnerable()
        {
            character.isInvulnerable = true;
        }
        public void DisableIsInvulnerable()
        {
            character.isInvulnerable = false;
        }

        public void EnableCanDoRollingAttack()
        {
            canPerformRollingAttack = true;
        }
        public void DisableCanDoRollingAttack()
        {
            canPerformRollingAttack = false;
        }
        public void EnableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = true;
        }
        public void DisableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = false;
        }

        public virtual void DisableCanDoCombo()
        {
        }

        public virtual void EnableCanDoCombo()
        {
        }
        #endregion
    }
}