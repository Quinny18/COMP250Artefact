using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Absorption")]
        public float physicalDamageAbsorption;

        [Header("Idle Animations")]
        public string Right_Hand_Idle;
        public string Left_Hand_Idle;

        [Header("One Handed Attack Animations")]
        public string OH_Light_Attack_01;
        public string OH_Light_Attack_02;
        public string OH_Heavy_Attack_01;

        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

        [Header("Weapon Type")]
        public bool isMeleeWeapon;
        public bool isShieldWeapon;
    }
}
