using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace CQ
{
    public class BossLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;
        NavMeshAgent navmeshAgent;
        public Rigidbody enemyRigidBody;

        public CharacterStats currentTarget; 
        public LayerMask detectionLayer;

        public float distanceFromTarget;
        public float stoppingDistance = 8f;
        public float rotationSpeed = 15;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidBody = GetComponent<Rigidbody>();
        }
        
        private void Start()
        {
            navmeshAgent.enabled = false;
            enemyRigidBody.isKinematic = false;
        }
        
        public void HandleDetection()
        {
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    // Check for the team id
                    Vector3 targetDirection = characterStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        currentTarget = characterStats;
                    }
                }
            }
        }
        
        public void HandleMoveToTarget(bool move)
        {
            if (enemyManager.isPreformingAction)
                return;

            if (currentTarget != null)
            {
            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            float viewableAngle = Vector3.AngleBetween(targetDirection, transform.forward);

            // If we are preforming an action, stop our movement
            if (enemyManager.isPreformingAction || move == false)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                navmeshAgent.enabled = false;
            }
            else
            {
                if (distanceFromTarget > stoppingDistance)
                {
                    enemyAnimatorManager.anim.SetFloat("Vertical", 1, 1f, Time.deltaTime);
                }
                else if (distanceFromTarget <= stoppingDistance)
                {
                    enemyAnimatorManager.anim.SetFloat("Vertical", 0, 1f, Time.deltaTime);
                }
            }
            HandleRotateTowardsTarget();
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
            }
        }

        private void HandleRotateTowardsTarget()
        {
            //Rotate Manually
            if (enemyManager.isPreformingAction)
            {
                Vector3 direction = currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = enemyManager.transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, rotationSpeed/ Time.deltaTime);
            }
            //Rotate with pathfinding (navmesh)
            else
            {
                Vector3 relativeDirection = enemyManager.transform.InverseTransformDirection(navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyRigidBody.velocity;

                navmeshAgent.enabled = true;
                navmeshAgent.SetDestination(currentTarget.transform.position);
                enemyRigidBody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, navmeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }
        }
    }
}
