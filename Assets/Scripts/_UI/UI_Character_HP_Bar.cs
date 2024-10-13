using System;
using TMPro;
using UnityEngine;

namespace KrazyKatgames
{
    // identically to UI_Stats_Bar -> except this one appears and disappears (will always face the camera) 
    public class UI_Character_HP_Bar : UI_StatBar
    {
        private CharacterManager character;
        //private AICharacterManager aiCharacter;
        private PlayerManager playerCharacter;

        [SerializeField] bool displayCharacterNameOnDamage = false;
        [SerializeField] float defaultTimeBeforeBarHides = 5f;
        [SerializeField] float hideTimer = 0f;
        [SerializeField] int currentDamageTaken = 0;
        [SerializeField] TextMeshProUGUI characterName;
        [SerializeField] TextMeshProUGUI characterDamage;

        [HideInInspector] public int oldHealthValue = 0;
        protected override void Awake()
        {
            base.Awake();

            character = GetComponentInParent<CharacterManager>();
            //
            // if (character != null)
            //     aiCharacter = character as AICharacterManager;
            if (character != null)
                playerCharacter = character as PlayerManager;
        }
        protected override void Start()
        {
            base.Start();

            gameObject.SetActive(false);
        }
        public override void SetStat(int newValue)
        {
            if (displayCharacterNameOnDamage)
            {
                characterName.text = playerCharacter.name;
            }

            slider.maxValue = character.characterStatsManager.maxHealth;

            // total damage taken while bar is active
            currentDamageTaken = Mathf.RoundToInt(currentDamageTaken + (oldHealthValue - newValue));
            if (currentDamageTaken < 0)
            {
                currentDamageTaken = Mathf.Abs(currentDamageTaken);
                characterDamage.text = "+ " + currentDamageTaken.ToString();
            }
            else
            {
                currentDamageTaken = Mathf.Abs(currentDamageTaken);
                characterDamage.text = "- " + currentDamageTaken.ToString();
            }
            slider.value = newValue;

            if (character.characterStatsManager.currentHealth != character.characterStatsManager.maxHealth)
            {
                hideTimer = defaultTimeBeforeBarHides;
                gameObject.SetActive(true);
            }
            base.SetStat(newValue);
        }
        private void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);

            if (hideTimer > 0)
            {
                hideTimer -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        private void OnDisable()
        {
            currentDamageTaken = 0;
        }
        public override void SetMaxStat(float maxValue)
        {
            base.SetMaxStat(maxValue);
        }
    }
}