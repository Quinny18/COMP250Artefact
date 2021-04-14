using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class EnemyManager : CharacterManager
    {
        BossEnemyBT bossEnemyBT;
        EnemyStats enemyStats;
        PlayerManager playerManager;
        BossLocomotionManager bossLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        public bool isPreformingAction;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        [Header("A.I Settings")]
        public float detectionRadius = 20;

        //The higher, and lower, respectively these angles are, the greater detection field of view
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 3;

        public float phaseOne = 99.9f;
        public float phaseTwo = 66.6f;
        public float phaseThree = 33.3f;


        private void Awake()
        {
            bossLocomotionManager = GetComponent<BossLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            bossEnemyBT = GetComponent<BossEnemyBT>();
            playerManager = FindObjectOfType<PlayerManager>();
    }

        private void Update()
        {
            bossEnemyBT.Evaluate();
            HandleRecoveryTimer();

            if (currentRecoveryTime > 0)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (enemyStats.currentHealth > phaseTwo)
            {
                currentAttack = enemyAttacks[0];
            }
            else if (enemyStats.currentHealth > phaseThree && enemyStats.currentHealth < phaseTwo)
            {
                currentAttack = enemyAttacks[1];
            }
            else
            {
                currentAttack = enemyAttacks[2];
            }
        }

        public void HandleCurrentAction()
        {
            if (bossLocomotionManager.currentTarget != null)
            {
                bossLocomotionManager.distanceFromTarget = 
                    Vector3.Distance(bossLocomotionManager.currentTarget.transform.position, transform.position);
            }          
            if (bossLocomotionManager.currentTarget == null)
            {
                bossLocomotionManager.HandleDetection();   
            }
            else if (bossLocomotionManager.distanceFromTarget <= bossLocomotionManager.stoppingDistance)
            {
                AttackTarget();
            }
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPreformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPreformingAction = false;
                }
            }
        }

        #region Attacks

        private void AttackTarget()
        {
            if (isPreformingAction)
                return;

            isPreformingAction = true;
            currentRecoveryTime = currentAttack.recoveryTime;
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            currentRecoveryTime = 3f;
        }

        public void rapidStrikes()
        {
            if (isPreformingAction)
                return;

            isPreformingAction = true;
            currentRecoveryTime = currentAttack.recoveryTime;
            enemyAnimatorManager.PlayTargetAnimation("EnemyBossRapidStrikes", true);
            currentRecoveryTime = 1f;
        }
        #endregion
    }

    /*Vector3 targetsDirection = bossLocomotionManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            bossLocomotionManager.distanceFromTarget = Vector3.Distance(bossLocomotionManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (bossLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack 
                    && bossLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (bossLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && bossLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }*/
}
