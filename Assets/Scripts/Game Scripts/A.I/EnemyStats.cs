using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class EnemyStats : CharacterStats
    {
        EnemyManager enemyManager;
        Animator animator;

        public BossHealthBar bossHealthBar;

        public float weaponAttackRange = 8f;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            enemyManager = GetComponent<EnemyManager>();
            bossHealthBar = FindObjectOfType<BossHealthBar>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthLevelFromHealthLevel();
            currentHealth = maxHealth;
            bossHealthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthLevelFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth = currentHealth - damage;
            bossHealthBar.SetCurrentHealth(currentHealth);

            animator.Play("Damage_01");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Dead_01");
                isDead = true;
                enemyManager.enabled = false;
                //HANDLE PLAYER DEATH
            }
        }
    }
}
