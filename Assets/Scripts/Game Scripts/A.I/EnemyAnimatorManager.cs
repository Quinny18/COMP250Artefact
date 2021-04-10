using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        BossLocomotionManager bossLocomotionManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            bossLocomotionManager = GetComponentInParent<BossLocomotionManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            bossLocomotionManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            bossLocomotionManager.enemyRigidBody.velocity = velocity;
        }
    }
}
