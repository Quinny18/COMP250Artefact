using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class BossEnemyBT : MonoBehaviour
    {
        
        public PlayerStats playerData;
        public EnemyStats enemyData;
        BossLocomotionManager bossLocomotionManager;
        EnemyManager enemyManager;

        public ActionNode playerHealthNode;
        public ActionNode playerDetection;
        public ActionNode distanceToPlayer;
        public ActionNode testOne;
        public ActionNode frontalSlamAttack;
        public Sequence bossAttackSequence;
        public Selector rootNode;

        public delegate void TreeExecuted();
        public event TreeExecuted onTreeExecuted;

        public delegate void NodePassed(string trigger);

        void Start()
        {
            // Begin by performing checks for bosshealth and player distance etc.

            playerDetection = new ActionNode(bossDetectPlayer);
            distanceToPlayer = new ActionNode(distanceToPlayerCheck);
            frontalSlamAttack = new ActionNode(phaseOneAttack);

            bossAttackSequence = new Sequence(new List<Node> {playerDetection, distanceToPlayer, frontalSlamAttack});

            //Root Node Comes Last
            rootNode = new Selector(new List<Node> {bossAttackSequence});
        }

        void Awake()
        {
            bossLocomotionManager = GetComponent<BossLocomotionManager>();
            enemyManager = GetComponent<EnemyManager>();
        }

        public void Evaluate()
        {
            rootNode.Evaluate();
            StartCoroutine(Execute());
        }

        private IEnumerator Execute()
        {
            //Debug.Log("AI is thinking...");
            yield return new WaitForSeconds(2.5f);

            if (playerDetection.nodeState == NodeStates.FAILURE)
            {
                bossLocomotionManager.HandleDetection();
            }
            else if (distanceToPlayer.nodeState == NodeStates.FAILURE)
            {
                bossLocomotionManager.HandleMoveToTarget(true);
            }
            else if (frontalSlamAttack.nodeState == NodeStates.SUCCESS)
            {
                Debug.Log("Attempting an Attack");
                enemyManager.HandleCurrentAction();
            }

            if (onTreeExecuted != null)
            {
                onTreeExecuted();
            }
        }
       
        private NodeStates bossDetectPlayer()
        {
            if (bossLocomotionManager.currentTarget == null)
            {
                Debug.Log("Successfully ran detect player");
                return NodeStates.FAILURE;
            }
            else
            {
                return NodeStates.SUCCESS;
            }

            #region Alternative Method
            /* ALTERNATIVE METHOD FOR DETECTING AND MOVING TO PLAYER
             * playerData.DistanceToAIBoss();
            bossLocomotionManager.HandleDetection();
            
            if (playerData.distanceToBoss <= bossLocomotionManager.stoppingDistance)
            {
                Debug.Log("Within Range");
                bossLocomotionManager.HandleMoveToTarget(false);
                return NodeStates.SUCCESS;
            }
            else
            {
                Debug.Log("Moving toward player");
                bossLocomotionManager.HandleMoveToTarget(true);
                return NodeStates.FAILURE;
            }*/
            #endregion
        }

        private NodeStates distanceToPlayerCheck()
        {
            if (playerData.distanceToBoss <= bossLocomotionManager.stoppingDistance)
            {
                Debug.Log("Within Attacking Range");
                return NodeStates.SUCCESS;
            }
            else
            {
                Debug.Log("Moving to Attacking Range");
                return NodeStates.FAILURE;
            }
        }
  
        private NodeStates phaseOneAttack()
        {
            return NodeStates.SUCCESS;
        }

    }
}
