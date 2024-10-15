using UnityEngine;

namespace KrazyKatGames
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Stamina Regeneration")]
        [SerializeField] float staminaRegenerationAmount = 2;
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;

        [Header("Stats")]
        public float currentStamina = 100;
        public float maxStamina = 100;

        public float currentHealth = 100;
        public float maxHealth = 100;
        
        public int vitality = 100;
        public int endurance = 100;
        
        public int blockingStability = 10;
        // todo more absorptions
        public int blockingPhysicalAbsorption = 10;

        [Header("Poise")]
        public float totalPoiseDamage = 5;
        public float basePoiseDefense = 5;
        public float offensivePoiseBonus = 5;
        public float poiseResetTimer = 5;
        public float defaultPoiseResetTime = 5;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {
        }
        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            //  CREATE AN EQUATION FOR HOW YOU WANT YOUR STAMINA TO BE CALCULATED

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }
        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = 0;

            //  CREATE AN EQUATION FOR HOW YOU WANT YOUR STAMINA TO BE CALCULATED

            health = vitality * 10;

            return Mathf.RoundToInt(health);
        }

        public virtual void RegenerateStamina()
        {
            //  WE DO NOT WANT TO REGENERATE STAMINA IF WE ARE USING IT

            if (character.isPerformingAction)
                return;

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (currentStamina < maxStamina)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        currentStamina += staminaRegenerationAmount;
                    }
                }
            }
        }

        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            //  WE ONLY WANT TO RESET THE REGENERATION IF THE ACTION USED STAMINA
            //  WE DONT WANT TO RESET THE REGENERATION IF WE ARE ALREADY REGENERATING STAMINA
            if (currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }
        }
    }
}