using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerEquipmentManager playerEquipmentManager;
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        PlayerManager playerManager;

        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
        }

        #region Input Actions
        public void HandleLBAction()
        {
            PerformLBBlockAction();
        }
        #endregion

        #region Attack Actions
        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                if (lastAttack == weapon.OH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_02, true);
                }
            }  
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_01, true);
            lastAttack = weapon.OH_Light_Attack_01;
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_01, true);
            lastAttack = weapon.OH_Heavy_Attack_01;
        }
        #endregion

        #region Defense Actions
        private void PerformLBBlockAction()
        {
            if (playerManager.isInteracting)
                return;
                

            if (playerManager.isBlocking)
                return;

            animatorHandler.PlayTargetAnimation("Block", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }
        #endregion
    }
}
