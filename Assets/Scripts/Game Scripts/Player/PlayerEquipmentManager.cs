using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        public BlockingCollider blockingCollider;

        private void Awake()
        {
            playerInventory = GetComponentInParent<PlayerInventory>();
            inputHandler = GetComponentInParent<InputHandler>();
        }

        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
            }
            
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
