using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class DamageCollider : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        Collider damageCollider;
        CharacterManager characterManager;
        PlayerManager playerManager;
        PlayerStats playerStats;

        public int currentWeaponDamage = 25;

        private void Awake()
        {
            animatorHandler = FindObjectOfType<AnimatorHandler>();
            playerStats = FindObjectOfType<PlayerStats>();
            characterManager = GetComponent<CharacterManager>();
            playerManager = FindObjectOfType<PlayerManager>();
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player" && playerManager.isBlocking == false)
            {
                Debug.Log("Hit the Player");
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if (shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                        if (playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                            return;
                        }
                    }
                }

                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
                
            }
            else if (collision.tag == "Player" && playerManager.isBlocking == true)
            {
                playerStats.TakeStaminaDamage(currentWeaponDamage);
                playerManager.isBlocking = false;
                playerManager.isPreformingAction = true;
                animatorHandler.PlayTargetAnimation("PlayerStagger", true);
                // stop block
            }


            if (collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

                if(enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }

            }
        }
    }

}
