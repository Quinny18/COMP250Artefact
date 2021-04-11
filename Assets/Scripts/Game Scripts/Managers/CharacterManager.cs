using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class CharacterManager : MonoBehaviour
    {
        public Transform lockOnTransform;

        [Header("Combat Flags")]
        public bool isParrying;
        public bool isBlocking;
    }
}
