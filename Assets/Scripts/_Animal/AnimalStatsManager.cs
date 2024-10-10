using UnityEngine;

namespace KrazyKatgames
{
    public class AnimalStatsManager : MonoBehaviour
    {
        AnimalManager animal;

        [Header("Stamina Regeneration")]
        [SerializeField] float staminaRegenerationAmount = 2;
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;

        [Header("Hunger and Thirst")]
        [SerializeField] float hungerRegenerationAmount = 2;
        private float hungerRegenerationTimer = 0;
        private float hungerRegenerationTickTimer = 0;
        [SerializeField] float hungerRegenerationDelay = 5;


        [Header("Stats")]
        private float currentStamina = 100;
        public float maxStamina = 100;

        private float currentHunger = 0;
        private float maxHunger = 100;

        public int vitality = 100;
        public int endurance = 100;
        public float CurrentHunger => currentHunger;
        public float CurrentStamina => currentStamina;
        protected virtual void Awake()
        {
            animal = GetComponent<AnimalManager>();
        }

        public void ReduceHunger(float hungerReduced)
        {
            currentHunger -= hungerReduced;
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
            if (animal.isPerformingAction)
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
        public virtual void RegenerateHunger()
        {
            if (animal.isPerformingAction)
                return;

            hungerRegenerationTimer += Time.deltaTime;

            if (hungerRegenerationTimer >= hungerRegenerationDelay)
            {
                if (currentHunger < maxHunger)
                {
                    hungerRegenerationTickTimer += Time.deltaTime;

                    if (hungerRegenerationTickTimer >= 0.1)
                    {
                        hungerRegenerationTickTimer = 0;
                        currentHunger += hungerRegenerationAmount;
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
        public virtual void ResetStaminaHungerRegenerationTimer()
        {
            hungerRegenerationTimer = 0;
        }
    }
}