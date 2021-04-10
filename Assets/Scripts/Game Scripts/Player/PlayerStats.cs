using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class PlayerStats : CharacterStats
    {
        public float distanceToBoss;

        public HealthBar healthBar;
        public StaminaBar staminaBar;


        EnemyManager enemyManager;
        AnimatorHandler animatorHandler;

        private void Awake()
        {
            enemyManager = FindObjectOfType<EnemyManager>();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthLevelFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetStaminaLevelFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }

        public void Update()
        {
            distanceToBoss = Vector3.Distance(transform.position, enemyManager.transform.position);
        }

        private int SetMaxHealthLevelFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private int SetStaminaLevelFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            currentHealth = currentHealth - damage;
            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Dead_01", true);
                //HANDLE PLAYER DEATH
            }
        }
        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }
    }
}
