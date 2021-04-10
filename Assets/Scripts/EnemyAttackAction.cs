using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    [CreateAssetMenu(menuName = "A.I/Enemy Actions/ Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 5;

        public float maximumAttackAngle = 70;
        public float minimumAttackAngle = -70;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;
    }
}
