using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class EnemyStats : CharacterStats
    {

        Animator animator;
        public float weaponAttackRange = 8f;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthLevelFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthLevelFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            animator.Play("Damage_01");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Dead_01");
                //HANDLE PLAYER DEATH
            }
        }
    }
}
