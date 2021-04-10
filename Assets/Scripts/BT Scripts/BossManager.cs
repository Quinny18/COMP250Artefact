using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class BossManager : CharacterManager
    {
        private BossEnemyBT bossEnemyBT;
        EnemyManager enemyManager;

        public void Awake()
        {
            bossEnemyBT = GetComponent<BossEnemyBT>();
            enemyManager = GetComponent<EnemyManager>();
        }
        public void Update()
        {
            bossEnemyBT.Evaluate();
        }
    }
}
