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
        PlayerManager playerManager;

        public ActionNode playerHealthNode;
        public ActionNode playerDetection;
        public ActionNode distanceToPlayer;
        public ActionNode attack;
        public Sequence bossAttackSequence;
        public Selector rootNode;

        public delegate void TreeExecuted();
        public event TreeExecuted onTreeExecuted;

        public delegate void NodePassed(string trigger);

        void Start()
        {
            // Begin by performing checks for playerdetection and if we are within range to attack
            playerDetection = new ActionNode(bossDetectPlayer);
            distanceToPlayer = new ActionNode(distanceToPlayerCheck);
            attack = new ActionNode(phaseOneAttack);

            bossAttackSequence = new Sequence(new List<Node> {playerDetection, distanceToPlayer, attack});

            //Root Node Comes Last
            rootNode = new Selector(new List<Node> {bossAttackSequence});
        }

        void Awake()
        {
            bossLocomotionManager = GetComponent<BossLocomotionManager>();
            enemyManager = GetComponent<EnemyManager>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

        public void Evaluate()
        {
            rootNode.Evaluate();
            StartCoroutine(Execute());
        }

        // The coroutine runs code from other scripts if the conditions of the Nodestates are met.
        private IEnumerator Execute()
        {
            yield return new WaitForSeconds(2.5f);

            if (playerDetection.nodeState == NodeStates.FAILURE)
            {
                bossLocomotionManager.HandleDetection();
            }
            else if (distanceToPlayer.nodeState == NodeStates.FAILURE)
            {
                bossLocomotionManager.HandleMoveToTarget(true);
            }
            else if (attack.nodeState == NodeStates.SUCCESS)
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
