using UnityEngine;

namespace KrazyKatGames
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;

        [Header("Weapon Attack Modifiers")]
        public float light_Attack_01_Modifier;
        public float light_Attack_02_Modifier;
        public float light_Attack_03_Modifier;
        public float light_Attack_04_Modifier;

        public float heavy_Attack_01_Modifier;
        public float heavy_Attack_02_Modifier;
        public float heavy_Attack_03_Modifier;
        public float heavy_Attack_04_Modifier;
        public float charge_Attack_01_Modifier;
        public float charge_Attack_02_Modifier;

        public float run_Attack_01_Modifier;
        public float roll_Attack_01_Modifier;
        public float backstep_Attack_01_Modifier;

        protected override void Awake()
        {
            base.Awake();
            damageCollider.enabled = false; // should be disabled on start
        }
        protected override void OnTriggerEnter(Collider other)
        {
            Debug.LogWarning("in MeleeWeaponDamageCollider.. OnTriggerEnter(Collider other): " + other.name);
            Debug.LogWarning("TAG: " + other.tag);

            
            if (other.CompareTag("HarvestGround"))
            {
                Debug.LogWarning("in MeleeWeaponDamageCollider.. Do something Attacked the ground");
            }
            
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                if (damageTarget == characterCausingDamage)
                    return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                //  FRIENDLY FIRE
                //  BLOCKING
                //  IS INVULNERABLE

                DamageTarget(damageTarget);
            }
        }
        protected override void GetBlockingDotValues(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = transform.position - characterCausingDamage.transform.position;
            dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
        }
        protected override void DamageTarget(CharacterManager damageTarget)
        {
            //  WE DON'T WANT TO DAMAGE THE SAME TARGET MORE THAN ONCE IN A SINGLE ATTACK
            //  SO WE ADD THEM TO A LIST THAT CHECKS BEFORE APPLYING DAMAGE
            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;
            
            damageEffect.poiseDamage = poiseDamage;
            
            damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.LightAttack02:
                    ApplyAttackDamageModifiers(light_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.LightAttack03:
                    ApplyAttackDamageModifiers(light_Attack_03_Modifier, damageEffect);
                    break;
                case AttackType.LightAttack04:
                    ApplyAttackDamageModifiers(light_Attack_04_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack01:
                    ApplyAttackDamageModifiers(heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack02:
                    ApplyAttackDamageModifiers(heavy_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack03:
                    ApplyAttackDamageModifiers(heavy_Attack_03_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack04:
                    ApplyAttackDamageModifiers(heavy_Attack_04_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack01:
                    ApplyAttackDamageModifiers(charge_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack02:
                    ApplyAttackDamageModifiers(charge_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.RollingAttack_01:
                    ApplyAttackDamageModifiers(roll_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.RunningAttack01:
                    ApplyAttackDamageModifiers(run_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.BackstepAttack_01:
                    ApplyAttackDamageModifiers(backstep_Attack_01_Modifier, damageEffect);
                    break;
                default:
                    break;
            }
        }
        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifier;
            damage.magicDamage *= modifier;
            damage.fireDamage *= modifier;
            damage.holyDamage *= modifier;
            damage.lightningDamage *= modifier;
            damage.poiseDamage *= modifier;

            // ToDo: fully charged Attacks 
        }
    }
}